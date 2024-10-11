using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rezervacije.Data;
using Rezervacije.Dtos;
using Rezervacije.Models;
using Rezervacije.Services;
using Rezervacije.Web.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using Rezervacije.Web.Services;
using Microsoft.EntityFrameworkCore;
using System;

namespace Rezervacije.Controllers;

[Route("/api/business")]
[ApiController]
public class BusinessController : ControllerBase
{
    private RezervacijeDbContext _context;
    private IInfobipApiService _sender;
    private IUserService _user;
    private IBusinessService _business;

    public BusinessController
    (
        RezervacijeDbContext context,
        IInfobipApiService sender,
        IUserService user,
        IBusinessService business
    )
    {
        _context = context;
        _sender = sender;
        _user = user;
        _business = business;
    }


    [HttpPost("register"), Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> CreateNewBusiness([FromBody] BusinessRegisterDto request)
    {
        _user.TrimZeroFromPhoneNumber(request.UserPhoneNumber);

        var user = await _user.FindUserByEmail(request.UserEmail);

        if (user is null)
        {
            return BadRequest(new { response = "Informacije o korisniku su pogrešne! Molimo vas unesite Email i broj mobitela iste kao za registraciju profila!" });
        }

        var address = new Address
        {
            Country = request.Country,
            HouseNumber = request.HouseNumber,
            PostalCode = request.PostalCode,
            Street = request.Street,
            Town = request.Town,
        };

        var newBusiness = new Business
        {
            BusinessIdentificationNumber = request.BusinessIdentificationNumber,
            BusinessName = request.BusinessName,
            BusinessType = request.BusinessType,
            Address = address,
            UserId = user.Id,
            IsWaitingValidation = true
        };

        await _context.Businesses.AddAsync(newBusiness);
        await _context.SaveChangesAsync();

        user.BusinessId = newBusiness.Id;

        for (int i = 0; i < 7; i++)
        {
            var workingDayStructure = new WorkingDayStructure
            {
                BusinessId = newBusiness.Id,
                IsWorkingDay = request.WorkingDays[i],
                StartingHours = request.StartingHours[i],
                EndingHours = request.EndingHours[i],
                Day = i
            };

            await _context.WorkingDaysStructure.AddAsync(workingDayStructure);
        }

        foreach (var activity in request.BusinessActivities)
        {
            var newActivity = new BusinessActivity
            {
                BusinessId = newBusiness.Id,
                DescriptionOfActivity = activity.DescriptionOfActivity,
                NameOfActivity = activity.NameOfActivity,
                Price = activity.Price
            };

            await _context.BusinessActivities.AddAsync(newActivity);
        }

        await _context.SaveChangesAsync();

        return Ok(new { response = "Podaci su poslani na pregled!" });
    }

    [HttpPost("auth"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> AuthentificateBusiness([FromBody] BusinessValidationDto request)
    {
        var business = await _business.GetBusinessByBusinessNameIncludingUser(request.BusinessName);

        if (business is null)
        {
            return NotFound(new { response = "Nismo mogli pronaći obrt pod tim imenom!", adminComment = request.AdminComment });
        }

        if (!request.IsValidBusiness)
        {
            _context.Businesses.Remove(business);
            await _context.SaveChangesAsync();

            var messageDenied = "Vaša prijava je odbačena, molimo vas provjerite podatke! Poruka admina: " + request.AdminComment;
            _sender.SendSMS(business.User.DialingCode + business.User.PhoneNumber, messageDenied);
            Console.WriteLine(messageDenied);

            return Ok(new { response = "Vaša prijava je odbačena, molimo vas provjerite podatke!", adminComment = request.AdminComment });
        }

        business.IsWaitingValidation = false;
        _context.Businesses.Update(business);
        await _context.SaveChangesAsync();

        var messageSuccess = "Vaš obrt je uspješno verificiran. Poruka admina: " + request.AdminComment;
        _sender.SendSMS(business.User.DialingCode + business.User.PhoneNumber, messageSuccess);
        Console.WriteLine(messageSuccess);

        return Ok(new { response = "Vaša prijava je uspješna!", adminComment = request.AdminComment });
    }

    [HttpGet("auth/list"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllBusinessesWaitingForAuthorization()
    {
        var businessWaitingForAuth = await _business.GetBusinessWaitingForAuth();

        if (businessWaitingForAuth is null)
        {
            return BadRequest(new { response = "Lista čekanja je prazna." });
        }

        var jsonOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
        };

        var jsonResult = JsonSerializer.Serialize(new { business = businessWaitingForAuth }, jsonOptions);

        return Ok(jsonResult);
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetAllBusinesses([FromQuery] GetBusinessListFiltersDto request)  
    {
        var user = await _user.FindUserByEmailIncludeAddress(request.UserEmail);

        var businesses = await _business.GetAllValidBusinesses();

        businesses = _business.FilterBusinessesByCountry(businesses, request.Country);

        businesses = _business.FilterBusinessesByTown(businesses, request.Town);

        if (request.Country == string.Empty && request.Town == string.Empty)
        {
            businesses = _business.FilterBusinessesByUserData(businesses, user);
        }

        businesses = _business.FilterBusinessesByBusinessType(businesses, request.BusinessType);

        if(!businesses.Any())
        {
            return NotFound(new { response = "Trenutno nema objavljenih obrta!" });
        }

        businesses = businesses
            .Take(request.BusinessCount)
            .ToList();

        var jsonOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
        };

        var responseObject = _business.StructureBusinessListResponse(businesses);

        var jsonResult = JsonSerializer.Serialize(new { listOfBusinesses = responseObject, businessCount = businesses.Count }, jsonOptions);

        return Ok(jsonResult);
    }

    [HttpGet("filters")]
    public async Task<IActionResult> GetAllUsebaleFilters()
    {
        var countries = await _context
            .Businesses
            .Select(x => x.Address.Country)
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync();

        var towns = await _context
            .Businesses
            .Select(x => x.Address.Town)
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync();

        var businessTypes = Enum.GetValues(typeof(BusinessTypes)).Cast<BusinessTypes>().Where(x => x > 0).ToList();

        var jsonOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
        };

        var jsonResult = JsonSerializer.Serialize(new { countries, towns, businessTypes }, jsonOptions);

        return Ok(jsonResult);
    }

}
