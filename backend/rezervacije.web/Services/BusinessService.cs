using Microsoft.EntityFrameworkCore;
using Rezervacije.Data;
using Rezervacije.Dtos;
using Rezervacije.Models;
using Rezervacije.Web.Models;
using Rezervacije.Web.ResponseDtos;

namespace Rezervacije.Web.Services;

public class BusinessService : IBusinessService
{
    private RezervacijeDbContext _context;

    public BusinessService
    (
        RezervacijeDbContext context
    )
    {
        _context = context;
    }

    public async Task<Business?> GetBusinessWaitingForAuth()
    {
        return await _context
            .Businesses
            .Where(x => x.IsWaitingValidation == true)
            .Include(x => x.Address)
            .Include(x => x.WorkingDayStructures)
            .Include(x => x.BusinessActivities)
            .Include(x => x.User)
            .FirstOrDefaultAsync();
    }

    public async Task<Business?> GetBusinessByBusinessNameIncludingUser(string businessName)
    {
        return await _context
            .Businesses
            .Where(x => x.BusinessName == businessName)
            .Include(x => x.User)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Business>> GetAllValidBusinesses()
    {
        return await _context
            .Businesses
            .Where(x => x.IsWaitingValidation == false)
            .Include(x => x.Address)
            .Include(x => x.User)
            .Include(x => x.BusinessActivities)
            .Include(x => x.BusinessAppointments)
            .Include(x => x.WorkingDayStructures)
            .ToListAsync();
    }

    public List<Business> FilterBusinessesByCountry(List<Business> businesses, string country)
    {
        return (country == string.Empty) ? 
            businesses :
            businesses
                .Where(x => x.Address.Country == country)
                .ToList();
    }

    public List<Business> FilterBusinessesByTown(List<Business> businesses, string town)
    {
        return (town == string.Empty) ?
            businesses :
            businesses
                .Where(x => x.Address.Town == town)
                .ToList();
    }

    public List<Business> FilterBusinessesByUserData(List<Business> businesses, User? user)
    {
        return (user is null) ?
            businesses :
            businesses
                .Where(x => x.Address.Country == user.Address.Country && x.Address.Town == user.Address.Town)
                .ToList();
    }

    public List<Business> FilterBusinessesByBusinessType(List<Business> businesses, BusinessTypes businessType)
    {
        return (businessType is BusinessTypes.Empty) ?
            businesses :
            businesses
                .Where(x => x.BusinessType == businessType)
                .ToList();
    }

    public List<BusinessListResponse>? StructureBusinessListResponse(List<Business> businesses)
    {
        return businesses
            .Select(x =>
            new BusinessListResponse
            {
                Address = x.Address,
                BusinessName = x.BusinessName ?? "",
                PhoneNumber = x.User.PhoneNumber ?? "",
                DialingCode = x.User.DialingCode ?? "",
                BusinessActivities = x.BusinessActivities
                    .Select(activity => new BusinessActivitiesData
                    {
                        NameOfActivity = activity.NameOfActivity,
                        DescriptionOfActivity = activity.DescriptionOfActivity,
                        Price = activity.Price
                    })
                    .ToList() ?? new List<BusinessActivitiesData>(),
                WorkHours = x.WorkingDayStructures
                    .Where(y => y.IsWorkingDay)
                    .Select(workingStructure => new WorkHour
                    {
                        Day = workingStructure.Day,
                        Start = (workingStructure.StartingHours < 10) ? $"0{workingStructure.StartingHours}:00" : $"{workingStructure.StartingHours}:00",
                        End = (workingStructure.EndingHours < 10) ? $"0{workingStructure.EndingHours}:00" : $"{workingStructure.EndingHours}:00",
                    })
                    .ToList() ?? new List<WorkHour>(),
                Appointments = x.BusinessAppointments
                    .Where(x => x.StartTime!.Value.Day >= DateTime.UtcNow.Day)
                    .Select(appointment => new ActiveAppointment
                    {
                        IsAllDay = false,
                        EndTime = appointment.EndTime!.Value,
                        StartTime = appointment.StartTime!.Value,
                        EventType = appointment.EventType!,
                        Id = appointment.Id,
                        Subject = appointment.Subject!

                    })
                    .ToList() ?? new List<ActiveAppointment>(),
                MinimumHour = (x.WorkingDayStructures.Where(y => y.IsWorkingDay).Min(y => y.StartingHours) < 10)
                              ? $"0{x.WorkingDayStructures.Where(y => y.IsWorkingDay).Min(y => y.StartingHours)}:00"
                              : $"{x.WorkingDayStructures.Where(y => y.IsWorkingDay).Min(y => y.StartingHours)}:00",

                MaximumHour = (x.WorkingDayStructures.Where(y => y.IsWorkingDay).Max(y => y.EndingHours) < 10)
                              ? $"0{x.WorkingDayStructures.Where(y => y.IsWorkingDay).Max(y => y.EndingHours)}:00"
                              : $"{x.WorkingDayStructures.Where(y => y.IsWorkingDay).Max(y => y.EndingHours)}:00",

            })
            .ToList() ?? new List<BusinessListResponse>();
    }

    public async Task<Business?> GetBusinessByBusinessNameIncludingAppointments(string businessName)
    {
        return await _context
            .Businesses
            .Where(x => x.BusinessName == businessName)
            .Include(x => x.BusinessAppointments)
            .FirstOrDefaultAsync();
    }

    public Appointment? FindBusinessAppointmentById(Business business, int id)
    {
        return business
            .BusinessAppointments
            .Where(x => x.Id == id)
            .FirstOrDefault();
    }

    public Appointment? FindBusinessAppointmentByStartTime(Business business, DateTime startTime)
    {
        return business
            .BusinessAppointments
            .Where(x => x.StartTime == startTime)
            .FirstOrDefault();
    }

    public async Task<List<Appointment>> GetAllAppointmentsForBusinessById(int id)
    {
        return await _context
            .Appointments
            .Where(x => x.BusinessId == id)
            .ToListAsync();
    }

    public async Task<Business?> GetBusinessByBusinessId(int id)
    {
        return await _context
            .Businesses
            .Where(x => x.Id == id)
            .Include(x => x.BusinessActivities)
            .FirstOrDefaultAsync();
    }
}
