using Rezervacije.Dtos;
using Rezervacije.Models;
using Rezervacije.Web.Models;
using Rezervacije.Web.ResponseDtos;

namespace Rezervacije.Web.Services;

public interface IBusinessService
{
    public Task<Business?> GetBusinessByBusinessNameIncludingUser(string businessName);
    public Task<Business?> GetBusinessByBusinessId(int id);
    public Task<Business?> GetBusinessByBusinessNameIncludingAppointments(string businessName);
    public Task<Business?> GetBusinessWaitingForAuth();
    public Task<List<Business>> GetAllValidBusinesses();
    public List<Business> FilterBusinessesByCountry(List<Business> businesses, string country);
    public List<Business> FilterBusinessesByTown(List<Business> businesses, string town);
    public List<Business> FilterBusinessesByUserData(List<Business> businesses, User? user);
    public List<Business> FilterBusinessesByBusinessType(List<Business> businesses, BusinessTypes businessType);
    public List<BusinessListResponse>? StructureBusinessListResponse(List<Business> businesses);
    public Appointment? FindBusinessAppointmentById(Business business, int id);
    public Appointment? FindBusinessAppointmentByStartTime(Business business, DateTime startTime);
    public Task<List<Appointment>> GetAllAppointmentsForBusinessById(int id);
}
