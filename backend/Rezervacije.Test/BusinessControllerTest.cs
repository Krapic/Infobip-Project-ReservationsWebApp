using Microsoft.Extensions.Configuration;
using Rezervacije.Controllers;
using Rezervacije.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using Rezervacije.Services;
using Rezervacije.Web.Services;
using Rezervacije.Models;
using Rezervacije.Web.Models;
using Rezervacije.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Rezervacije.Test;

[TestFixture]
public class BusinessControllerTest
{
    private RezervacijeDbContext _context;
    private BusinessController _controller;
    private IConfiguration _config;

    [SetUp]
    public void Setup()
    {
        var businessService = new Mock<IBusinessService>();
        var infobipService = new Mock<IInfobipApiService>();
        var userService = new Mock<IUserService>();

        _config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var options = new DbContextOptionsBuilder<RezervacijeDbContext>()
            .UseNpgsql(_config.GetConnectionString("DefaultConnection"))
            .Options;

        _context = new RezervacijeDbContext(options);

        _controller = new BusinessController(_context, infobipService.Object, userService.Object, businessService.Object);

        _context.Database.BeginTransaction();

        var address = new Address
        {
            Country = "Croatia",
            HouseNumber = "3B",
            PostalCode = "44250",
            Street = "Ul. Ive Maline",
            Town = "Petrinja"
        };

        _context.Users.Add(new User
        {
            Email = "ivica@gmail.com",
            Address = address,
            DialingCode = "+385",
            Name = "Ivica",
            LastName = "Juric",
            IsAdmin = false,
            Password = BCrypt.Net.BCrypt.HashPassword("123"),
            PhoneNumber = "993325806"

        });

        _context.SaveChanges();
    }

    [Test]
    public async Task Create_New_Business_User_Is_Null()
    {
        // Arrange
        var request = new BusinessRegisterDto
        {
            BusinessIdentificationNumber = 955,
            BusinessName = "neki d.o.o",
            Country = "Croatia",
            HouseNumber = "10",
            PostalCode = "44250",
            Street = "Malinska",
            Town = "Petrinja",
            UserEmail = "",
            UserPhoneNumber = "993325806"
        };

        // Act
        var response = await _controller.CreateNewBusiness(request);

        // Assert
        Assert.That(response, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Authentificate_Business_Business_Is_Null()
    {
        // Arrange
        var request = new BusinessValidationDto
        {
            AdminComment = "nista",
            BusinessName = "",
            IsValidBusiness = true
        };

        // Act
        var response = await _controller.AuthentificateBusiness(request);

        // Assert
        Assert.That(response, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task Get_All_Businesses_Waiting_For_Authorization_Business_Is_Null()
    {
        // Arrange

        // Act
        var response = await _controller.GetAllBusinessesWaitingForAuthorization();

        // Assert
        Assert.That(response, Is.InstanceOf<BadRequestObjectResult>());
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.CurrentTransaction!.Rollback();
    }
}
