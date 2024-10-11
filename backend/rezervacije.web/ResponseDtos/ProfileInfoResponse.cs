using Rezervacije.Models;
using Rezervacije.Web.Models;

namespace Rezervacije.Web.ResponseDtos;

public class ProfileInfoResponse
{
    public required Address Address { get; set; }
    public required string DialingCode { get; set; }
    public required string Name { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public List<BusinessActivity>? BusinessActivities { get; set; }
    public string? BusinessName { get; set; }
    public int? BusinessIdentificationNumber { get; set; }

}
