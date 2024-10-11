using Microsoft.Extensions.Configuration;
using Rezervacije.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using Rezervacije.Web.Services;
using Rezervacije.Web.Controllers;
using Rezervacije.Models;
using Rezervacije.Web.Models;
using Rezervacije.Web.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Rezervacije.Test;

[TestFixture]
public class ReservationControllerTest
{
    private RezervacijeDbContext _context;
    private ReservationController _controller;
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

        var address = new Address
        {
            Country = "Croatia",
            HouseNumber = "3B",
            PostalCode = "44250",
            Street = "Malinova",
            Town = "Petrinja"
        };

        _context.Add(address);

        var user = new User
        {
            Address = address,
            DialingCode = "+385",
            Email = "nista@gmail.com",
            Name = "Test",
            LastName = "Test",
            Password = BCrypt.Net.BCrypt.HashPassword("Test"),
            PhoneNumber = "993325806"
        };

        var addressNew = new Address
        {
            Id = 10,
            Country = "2",
            HouseNumber = "3B",
            PostalCode = "44250",
            Street = "e32",
            Town = "e23r3"
        };

        _context.Add(addressNew);

        var userNew = new User
        {
            Id = 484848,
            Address = addressNew,
            DialingCode = "+385",
            Email = "zagreb@gmail.com",
            Name = "Test",
            LastName = "Test",
            Password = BCrypt.Net.BCrypt.HashPassword("Test"),
            PhoneNumber = "993325806"
        };

        _context.Add(userNew);

        var business = new Business
        {
            Address = address,
            BusinessIdentificationNumber = 98234,
            BusinessName = "Test",
            BusinessType = Dtos.BusinessTypes.Hairdresser,
            IsWaitingValidation = true,
            User = user
        };

        _context.Add(business);

        var appointment = new Appointment
        {
            Business = business,
            ReservedUser = userNew,
            Id = 99
        };

        _context.Add(appointment);

        await _context.SaveChangesAsync();

        var businessService = new Mock<IBusinessService>();
        businessService.Setup(x => x.GetBusinessByBusinessNameIncludingUser("Test")).Returns(Task.FromResult(business)!);
        businessService.Setup(x => x.GetBusinessByBusinessNameIncludingAppointments("Test")).Returns(Task.FromResult(business)!);
        businessService.Setup(x => x.FindBusinessAppointmentById(business, 99)).Returns(appointment);
        businessService.Setup(x => x.FindBusinessAppointmentByStartTime(business, DateTime.Parse("1-1-2022"))).Returns(appointment);
        
        var userService = new Mock<IUserService>();
        userService.Setup(x => x.FindUserByEmail("zagreb@gmail.com")).Returns(Task.FromResult(user)!);

        _controller = new ReservationController(_context, userService.Object, businessService.Object);

    }

    [Test]
    public async Task Reserve_This_Term_Business_Is_Null()
    {
        // Arrange
        var request = new ReserveTermDto
        {
            BusinessName = "",
            EndTime = DateTime.Now,
            StartTime = DateTime.Now,
            EventType = "",
            Subject = "efoekof",
            UserEmail = "Test",
        };

        // Act
        var response = await _controller.ReserveThisTerm(request);

        // Assert
        Assert.That(response, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task Reserve_This_Term_User_Is_Null()
    {
        // Arrange
        var request = new ReserveTermDto
        {
            BusinessName = "Test",
            EndTime = DateTime.Now,
            StartTime = DateTime.Now,
            EventType = "",
            Subject = "efoekof",
            UserEmail = "",
        };

        // Act
        var response = await _controller.ReserveThisTerm(request);

        // Assert
        Assert.That(response, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task Cancel_This_Reservation_Business_Is_Null()
    {
        // Arrange
        var request = new DeleteReservationDto
        {
            BusinessName = "",
            UserEmail = "zagreb@gmail.com",
            Id = 100
        };

        // Act
        var response = await _controller.CancelThisReservation(request);

        // Assert
        Assert.That(response, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task Cancel_This_Reservation_User_Is_Null()
    {
        // Arrange
        var request = new DeleteReservationDto
        {
            BusinessName = "Test",
            UserEmail = "",
            Id = 100
        };

        // Act
        var response = await _controller.CancelThisReservation(request);

        // Assert
        Assert.That(response, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task Cancel_This_Reservation_Appointment_Is_Null()
    {
        // Arrange
        var request = new DeleteReservationDto
        {
            BusinessName = "Test",
            UserEmail = "zagreb@gmail.com",
            Id = 100
        };

        // Act
        var response = await _controller.CancelThisReservation(request);

        // Assert
        Assert.That(response, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task Cancel_This_Reservation_Wrong_User()
    {
        // Arrange
        var request = new DeleteReservationDto
        {
            BusinessName = "Test",
            UserEmail = "zagreb@gmail.com",
            Id = 99
        };

        // Act
        var response = await _controller.CancelThisReservation(request);

        // Assert
        Assert.That(response, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Update_This_Reservation_Business_Is_Null()
    {
        // Arrange
        var request = new ReserveTermDto
        {
            BusinessName = "",
            EndTime = DateTime.Now,
            StartTime = DateTime.Now,
            EventType = "",
            Subject = "efoekof",
            UserEmail = "",
        };

        // Act
        var response = await _controller.UpdateThisReservation(request);

        // Assert
        Assert.That(response, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task Update_This_Reservation_User_Is_Null()
    {
        // Arrange
        var request = new ReserveTermDto
        {
            BusinessName = "Test",
            EndTime = DateTime.Now,
            StartTime = DateTime.Now,
            EventType = "",
            Subject = "efoekof",
            UserEmail = "",
        };

        // Act
        var response = await _controller.UpdateThisReservation(request);

        // Assert
        Assert.That(response, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task Update_This_Reservation_Appointment_Is_Null()
    {
        // Arrange
        var request = new ReserveTermDto
        {
            BusinessName = "Test",
            EndTime = DateTime.Now,
            StartTime = DateTime.MinValue,
            EventType = "t",
            Subject = "efoekof",
            UserEmail = "zagreb@gmail.com",
        };

        // Act
        var response = await _controller.UpdateThisReservation(request);

        // Assert
        Assert.That(response, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task Update_This_Reservation_Wrong_User()
    {
        // Arrange
        var request = new ReserveTermDto
        {
            BusinessName = "Test",
            EndTime = DateTime.Now,
            StartTime = DateTime.Parse("1-1-2022"),
            EventType = "t",
            Subject = "efoekof",
            UserEmail = "zagreb@gmail.com",
        };

        // Act
        var response = await _controller.UpdateThisReservation(request);

        // Assert
        Assert.That(response, Is.InstanceOf<BadRequestObjectResult>());
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.CurrentTransaction!.Rollback();
    }
}
