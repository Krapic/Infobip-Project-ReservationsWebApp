using Microsoft.AspNetCore.Mvc;
using Rezervacije.Data;
using Rezervacije.Dtos;
using Rezervacije.Models;
using Rezervacije.Services;
using Rezervacije.Web.Dtos;
using Rezervacije.Web.Models;
using Rezervacije.Web.Services;

namespace Rezervacije.Controllers;

[Route("/api/auth")]
[ApiController]
public class AuthentificationController : Controller
{
    private IAuthentificationService _auth;
    private IUserService _user;
    private RezervacijeDbContext _context;
    private IInfobipApiService _sender;

    public AuthentificationController
    (
        RezervacijeDbContext context,
        IAuthentificationService auth,
        IInfobipApiService sender,
        IUserService user
    )
    {
        _context = context;
        _auth = auth;
        _sender = sender;
        _user = user;
    }


    [HttpPost("login/request")]
    public async Task<IActionResult> RequestUserLogin([FromBody] LoginDto request)
    {
        var validUser = await _user.FindUserByEmail(request.Email);

        if (validUser is null || !BCrypt.Net.BCrypt.Verify(request.Password, validUser.Password))
        {
            return BadRequest(new { response = "Unijeli ste krivu lozinku ili niste kreirali račun!" });

        }

        if (validUser.IsAdmin)
        {
            string token = _auth.CreateToken(validUser, validUser.IsAdmin);

            return Ok(new { jwt = token, response = "Uspješno ste ulogirani!" });

        }

        var sixDigitAuth = new SixDigitAuthorization
        {
            UserId = validUser.Id,
            SixDigitNumber = _auth.GetSixDigitRandomNumber()
        };

        string message = "Tvoj 6-znamenkasti autentifikacijski kod je: " + sixDigitAuth.SixDigitNumber;

        _sender.SendSMS(validUser.DialingCode + validUser.PhoneNumber, message);

        Console.WriteLine(message);

        sixDigitAuth.SixDigitNumber = BCrypt.Net.BCrypt.HashPassword(sixDigitAuth.SixDigitNumber);

        await _context.SixDigitAuthorizations.AddAsync(sixDigitAuth);
        await _context.SaveChangesAsync();

        return Ok(new { response = "SMS je poslan" });
    }

    [HttpPost("login/auth")]
    public async Task<IActionResult> AuthentificateUserLogin([FromBody] LoginAuthDto request)
    {
        var user = await _user.FindUserByEmail(request.Email);

        if (user is null)
        {
            return BadRequest(new { response = "Netočni podaci o korisniku, molimo vas pokušajte ponovno!" });
        }

        var userActiveCodes = await _user.GetAllUserActiveAuthorizationCodes(user.Id);

        bool isCorrectCode = await _auth.IsCorrectAuthorizationCode(request.SixDigitCode, userActiveCodes);

        if (!isCorrectCode)
        {
            return BadRequest(new { response = "Netočni verifikacijski kod ili podaci o korisniku, molimo vas pokušajte ponovno!" });

        }

        string token = _auth.CreateToken(user, user.IsAdmin);

        return Ok(new { jwt = token, response = "Uspješno ste ulogirani!" });

    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterNewUser([FromBody] RegisterDto request)
    {
        if (_user.DoesUserByEmailExist(request.Email))
        {
            return BadRequest(new { response = "Email već postoji, želite li se ulogirati?" });
        }

        request.PhoneNumber = _user.TrimZeroFromPhoneNumber(request.PhoneNumber);

        var hashPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var address = new Address
        {
            Country = request.Country,
            PostalCode = request.PostalCode,
            Town = request.Town,
            HouseNumber = request.HouseNumber,
            Street = request.Street
        };

        var user = new User
        {
            Email = request.Email,
            Name = request.Name,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Address = address,
            Password = hashPassword,
            BusinessId = null,
            DialingCode = request.DialingCode
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return Ok(new { response = "Uspješno ste registrirani!" });
    }

    [HttpPost("change-password/request")]
    public async Task<IActionResult> RequestNewPassword([FromBody] RequestPasswordChangeDto request)
    {
        var user = await _user.FindUserByEmail(request.Email);

        if (user is null)
        {
            return BadRequest(new { response = "Vaš email ne postoji u našem sustavu!" });
        }

        var sixDigitAuth = new SixDigitAuthorization
        {
            UserId = user.Id,
            SixDigitNumber = _auth.GetSixDigitRandomNumber()
        };

        var subject = "Potvrda emaila Rezervacije";
        string message = "Tvoj 6-znamenkasti autentifikacijski kod za potvrdu emaila je: " + sixDigitAuth.SixDigitNumber;

        _sender.SendEmail(request.Email, subject, message);

        Console.WriteLine(message);

        sixDigitAuth.SixDigitNumber = BCrypt.Net.BCrypt.HashPassword(sixDigitAuth.SixDigitNumber);

        await _context.SixDigitAuthorizations.AddAsync(sixDigitAuth);
        await _context.SaveChangesAsync();

        return Ok(new { response = "Email uspješno poslan." });
    }

    [HttpPost("change-password/authentification")]
    public async Task<IActionResult> AuthentificateEmailForPasswordChange([FromBody] AuthentificateEmailDto request)
    {
        var user = await _user.FindUserByEmail(request.Email);

        if (user is null)
        {
            return BadRequest(new { response = "Netočni podaci o korisniku, molimo vas pokušajte ponovno!" });
        }

        var userActiveCodes = await _user.GetAllUserActiveAuthorizationCodes(user.Id);

        bool isCorrectCode = await _auth.IsCorrectAuthorizationCode(request.SixDigitCode, userActiveCodes);

        if (!isCorrectCode)
        {
            return BadRequest(new { response = "Netočni verifikacijski kod ili podaci o korisniku, molimo vas pokušajte ponovno!" });

        }

        return Ok(new { response = "Uspješno ste potvrdili svoj identitet! Molimo vas unesite novu lozinku." });
    }

    [HttpPost("change-password/set-new-password")]
    public async Task<IActionResult> SetNewPassword([FromBody] SetNewPasswordDto request)
    {
        var user = await _user.FindUserByEmail(request.Email);

        if (user is null)
        {
            return BadRequest(new { response = "Netočni podaci o korisniku, molimo vas pokušajte ponovno!" });
        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

        await _context.SaveChangesAsync();

        return Ok(new { response = "Lozinka je uspješno promijenjena!" });
    }
}
