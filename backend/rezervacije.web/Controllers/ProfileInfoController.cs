using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rezervacije.Data;
using Rezervacije.Dtos;
using System.Text.Json.Serialization;
using System.Text.Json;
using Rezervacije.Web.Services;
using Rezervacije.Web.ResponseDtos;

namespace Rezervacije.Controllers;

[Authorize]
[Route("api/profile")]
[ApiController]
public class ProfileInfoController : ControllerBase
{
    private RezervacijeDbContext _context;
    private IUserService _user;
    private IBusinessService _business;


    public ProfileInfoController(RezervacijeDbContext context, IUserService user, IBusinessService business)
    {
        _context = context;
        _user = user;
        _business = business;
    }

    [HttpGet, Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> GetUserProfileInfo([FromQuery] GetProfileInfoDto request)
    {
        var jsonOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
        };

        var user = await _user.FindUserByEmailIncludeEverything(request.Email);

        if (user is null)
        {
            return NotFound(new { response = "Nema korisnika!" });
        }

        var business = await _business.GetBusinessByBusinessId(user.BusinessId ?? -1);

        if (business is null)
        {
            var responseUser = new ProfileInfoResponse
            {
                Address = user.Address,
                DialingCode = user.DialingCode,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Name = user.Name + " " + user.LastName,
            };

            var jsonResultUser = JsonSerializer.Serialize(new { data = responseUser, response = "Nema obrta!" }, jsonOptions);

            return NotFound(jsonResultUser);
        }

        var response = new ProfileInfoResponse
        {
            Address = user.Address,
            BusinessActivities = business.BusinessActivities,
            BusinessIdentificationNumber = business.BusinessIdentificationNumber,
            BusinessName = business.BusinessName,
            DialingCode = user.DialingCode,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Name = user.Name + " " + user.LastName,
        };

        var jsonResult = JsonSerializer.Serialize(new { data = response }, jsonOptions);

        return new OkObjectResult(jsonResult);
    }
}
