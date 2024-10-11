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
using Rezervacije.Web.Dtos;

namespace Rezervacije.Test;

[TestFixture]
public class AuthentificationControllerTest
{
    private RezervacijeDbContext _context;
    private AuthentificationController _controller;
    private IConfiguration _config;

    [SetUp]
    public async Task Setup()
    {
        _config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var options = new DbContextOptionsBuilder<RezervacijeDbContext>()
            .UseNpgsql(_config.GetConnectionString("DefaultConnection"))
            .Options;

        _context = new RezervacijeDbContext(options);

        _context.Database.BeginTransaction();

        var myUser = new User
        {
            Id = 999999,
            Address = new Address
            {
                Country = "Croatia",
                HouseNumber = "3B",
                PostalCode = "12345",
                Street = "Ul. Ive Maline",
                Town = "Osijek"
            },
            DialingCode = "+385",
            Email = "osijek@gmail.com",
            LastName = "nezz",
            Name = "Name",
            Password = BCrypt.Net.BCrypt.HashPassword("123"),
            PhoneNumber = "993325806"

        };

        _context.Add(myUser);

        _context.Add(new SixDigitAuthorization
        {
            UserId = 999999,
            IsUsed = false,
            SixDigitNumber = BCrypt.Net.BCrypt.HashPassword("123456")
        });

        await _context.SaveChangesAsync();

        var authService = new Mock<IAuthentificationService>();
        var infobipService = new Mock<IInfobipApiService>();
        var userService = new Mock<IUserService>();
        userService.Setup(x => x.FindUserByEmail("osijek@gmail.com")).Returns(Task.FromResult(myUser)!);
        userService.Setup(x => x.DoesUserByEmailExist("osijek@gmail.com")).Returns(true);

        _controller = new AuthentificationController(_context, authService.Object, infobipService.Object, userService.Object);
    }

    [Test]
    public async Task Request_User_Login_User_Is_Null()
    {
        // Arrange
        var request = new LoginDto
        {
            Email = "",
            Password = ""
        };

        // Act
        var response = await _controller.RequestUserLogin(request);

        // Assert
        Assert.That(response, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Request_User_Login_Invalid_Password()
    {
        // Arrange
        var request = new LoginDto
        {
            Email = "osijek@gmail.com",
            Password = "1"
        };

        // Act
        var response = await _controller.RequestUserLogin(request);

        // Assert
        Assert.That(response, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Authentificate_User_Login_Incorrect_Authorization_Code()
    {
        // Arrange
        var request = new LoginAuthDto
        {
            Email = "osijek@gmail.com",
            SixDigitCode = "123455"
        };

        // Act
        var response = await _controller.AuthentificateUserLogin(request);

        // Assert
        Assert.That(response, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Register_New_User_User_Already_Exists()
    {
        // Arrange
        var request = new RegisterDto
        {
            Email = "osijek@gmail.com",
            Country = "",
            DialingCode = "",
            HouseNumber = "",
            LastName = "",
            Name = "Name",
            Password = "Password",
            PhoneNumber = "what",
            PostalCode = "12345",
            Street = "1234567890",
            Town = "1234567890",
        };

        // Act
        var response = await _controller.RegisterNewUser(request);

        // Assert
        Assert.That(response, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Request_New_Password_User_Is_Null()
    {
        // Arrange
        var request = new RequestPasswordChangeDto
        {
            Email = ""
        };

        // Act
        var response = await _controller.RequestNewPassword(request);

        // Assert
        Assert.That(response, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Authentificate_Email_For_Password_Change_User_Is_Null()
    {
        // Arrange
        var request = new AuthentificateEmailDto
        {
            Email = "",
            SixDigitCode = "123456"
        };

        // Act
        var response = await _controller.AuthentificateEmailForPasswordChange(request);

        // Assert
        Assert.That(response, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Authentificate_Email_For_Password_Change_Incorrect_Auth_Code()
    {
        // Arrange
        var request = new AuthentificateEmailDto
        {
            Email = "osijek@gmail.com",
            SixDigitCode = "123455"
        };

        // Act
        var response = await _controller.AuthentificateEmailForPasswordChange(request);

        // Assert
        Assert.That(response, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Set_New_Password_User_Is_Null()
    {
        // Arrange
        var request = new SetNewPasswordDto
        {
            Email = "",
            NewPassword = "Password"
        };

        // Act
        var response = await _controller.SetNewPassword(request);

        // Assert
        Assert.That(response, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Set_New_Password_Password_Successfully_Changed()
    {
        // Arrange
        var request = new SetNewPasswordDto
        {
            Email = "osijek@gmail.com",
            NewPassword = "Password"
        };

        // Act
        var response = await _controller.SetNewPassword(request);

        // Assert
        Assert.That(response, Is.InstanceOf<OkObjectResult>());

        var user = await _context
            .Users
            .FirstAsync(x => x.Email == "osijek@gmail.com");

        Assert.That(BCrypt.Net.BCrypt.Verify("Password", user.Password), Is.EqualTo(true));
    }

    [TearDown] 
    public void TearDown() 
    {
        _context.Database.CurrentTransaction!.Rollback();
    }
}
