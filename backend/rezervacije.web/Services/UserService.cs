using Microsoft.EntityFrameworkCore;
using Rezervacije.Data;
using Rezervacije.Models;
using Rezervacije.Web.Models;

namespace Rezervacije.Web.Services;

public class UserService : IUserService
{
    private RezervacijeDbContext _context;

    public UserService
    (
        RezervacijeDbContext context
    )
    {
        _context = context;
    }

    public bool DoesUserByEmailExist(string email)
    {
        return _context
            .Users
            .Where(x => x.Email == email)
            .Any();
    }

    public async Task<User?> FindUserByEmail(string email)
    {
        return await _context
            .Users
            .Where(x => x.Email == email)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> FindUserByEmailIncludeAddress(string email)
    {
        return await _context
            .Users
            .Include(x => x.Address)
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<User?> FindUserByEmailIncludeEverything(string email)
    {
        return await _context
            .Users
            .Include(x => x.Address)
            .Include(x => x.Business)
            .Include(x => x.Business!.BusinessActivities)
            .Include(x => x.Business!.BusinessAppointments)
            .Include(x => x.Business!.WorkingDayStructures)
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<List<Appointment>> GetAllAppointmentsForThisUser(string email)
    {
        return await _context
            .Appointments
            .Where(x => x.ReservedUser!.Email == email && x.StartTime >= DateTime.Today.AddDays(-1))
            .Include(x => x.Business)
            .Select(x => new Appointment
            {
                EventType = x.EventType,
                BusinessId = x.BusinessId,
                EndTime = x.EndTime,
                Business = x.Business,
                ReservedUser = x.ReservedUser,
                Id = x.Id,
                ReservedUserId = x.ReservedUserId,
                StartTime = x.StartTime,
                Subject = x.Business.BusinessName,
            })
            .ToListAsync();
    }

    public async Task<List<SixDigitAuthorization>> GetAllUserActiveAuthorizationCodes(int userId)
    {
        return await _context.SixDigitAuthorizations
            .Where(x => x.UserId == userId
                     && x.IsUsed == false
                     && x.ValidUntil > DateTime.UtcNow)
            .ToListAsync();
    }

    public string TrimZeroFromPhoneNumber(string phoneNumber)
    {
        return phoneNumber.StartsWith("0") ? 
            new string(phoneNumber.Skip(1).ToArray()) : 
            phoneNumber;
    }
}
