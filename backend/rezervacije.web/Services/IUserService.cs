using Rezervacije.Models;
using Rezervacije.Web.Models;

namespace Rezervacije.Web.Services;

public interface IUserService
{
    public Task<User?> FindUserByEmail(string email);
    public Task<List<SixDigitAuthorization>> GetAllUserActiveAuthorizationCodes(int userId);
    public bool DoesUserByEmailExist(string email);
    public string TrimZeroFromPhoneNumber(string phoneNumber);
    public Task<User?> FindUserByEmailIncludeAddress(string email);
    public Task<User?> FindUserByEmailIncludeEverything(string email);
    public Task<List<Appointment>> GetAllAppointmentsForThisUser(string email);
}
