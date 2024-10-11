using Microsoft.AspNetCore.Mvc;
using Rezervacije.Dtos;
using Rezervacije.Models;

namespace Rezervacije.Services;

public interface IAuthentificationService
{
    public string CreateToken(User user, bool isAdmin);
    public string GetSixDigitRandomNumber();
    public Task<bool> IsCorrectAuthorizationCode(string enteredSixDigitNumberCode, List<SixDigitAuthorization>? userActiveCodes);
}
