using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Rezervacije.Data;
using Rezervacije.Models;
using Rezervacije.Web.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Rezervacije.Services;

public class AuthentificationService : IAuthentificationService
{
    private RezervacijeDbContext _context;
    private readonly IConfiguration _configuration;
    private IUserService _user;

    public AuthentificationService
    (
        RezervacijeDbContext context,
        IConfiguration configuration,
        IUserService user
    )
    {
        _context = context;
        _configuration = configuration;
        _user = user;
    }
    public string CreateToken(User user, bool isAdmin)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, $"{user.Name} {user.LastName}"),
            new Claim(ClaimTypes.Role, isAdmin ? "Admin" : "User")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
                issuer: _configuration.GetSection("AppSettings:Issuer").Value!,
                audience: _configuration.GetSection("AppSettings:Audience").Value!,
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials
            );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
    public string GetSixDigitRandomNumber()
    {
        string sixDigitNumber = string.Empty;
        Random rnd = new();

        sixDigitNumber = rnd.Next(100000, 999999).ToString();

        if (_context.SixDigitAuthorizations
            .Where(x => x.SixDigitNumber == sixDigitNumber && (x.IsUsed == false || x.ValidUntil > DateTime.UtcNow))
            .Any()
          )
        {
            sixDigitNumber = GetSixDigitRandomNumber();
        }

        return sixDigitNumber;
    }

    public async Task<bool> IsCorrectAuthorizationCode(string enteredSixDigitNumberCode, List<SixDigitAuthorization>? userActiveCodes)
    {
        if(userActiveCodes is null)
        {
            return false;
        }

        bool isCorrectCode = false;

        foreach (var code in userActiveCodes)
        {
            isCorrectCode = BCrypt.Net.BCrypt.Verify(enteredSixDigitNumberCode, code.SixDigitNumber);

            if (isCorrectCode)
            {
                userActiveCodes.Where(x => x.Id == code.Id).First().IsUsed = true;
                await _context.SaveChangesAsync();
                break;
            }
        }

        return isCorrectCode;
    }
}
