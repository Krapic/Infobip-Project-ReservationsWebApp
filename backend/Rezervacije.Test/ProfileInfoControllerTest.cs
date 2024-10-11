using Microsoft.Extensions.Configuration;
using Rezervacije.Controllers;
using Rezervacije.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using Rezervacije.Web.Services;
using Rezervacije.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Rezervacije.Test;

[TestFixture]
public class ProfileInfoControllerTest
{
    private RezervacijeDbContext _context;
    private ProfileInfoController _controller;
    private IConfiguration _config;

    [SetUp]
    public void Setup()
    {
        var userService = new Mock<IUserService>();
        var businessService = new Mock<IBusinessService>();

        _config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var options = new DbContextOptionsBuilder<RezervacijeDbContext>()
            .UseNpgsql(_config.GetConnectionString("DefaultConnection"))
            .Options;

        _context = new RezervacijeDbContext(options);

        _controller = new ProfileInfoController(_context, userService.Object, businessService.Object);

        _context.Database.BeginTransaction();
    }

    [Test]
    public async Task Get_User_Profile_Info_User_Is_Null()
    {
        // Arrange
        var request = new GetProfileInfoDto
        {
            Email = ""
        };

        // Act
        var response = await _controller.GetUserProfileInfo(request);

        // Assert
        Assert.That(response, Is.InstanceOf<NotFoundObjectResult>());
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.CurrentTransaction!.Rollback();
    }
}

