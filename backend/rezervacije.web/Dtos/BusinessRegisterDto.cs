
namespace Rezervacije.Dtos;

public class BusinessRegisterDto
{
    public required string UserEmail { get; set; }
    public required string UserPhoneNumber { get; set; }
    public required string Town { get; set; }
    public required string Country { get; set; }
    public required string Street { get; set; }
    public required string HouseNumber { get; set; }
    public required string PostalCode { get; set; }
    public required string BusinessName { get; set; }
    public required int BusinessIdentificationNumber { get; set; }
    public BusinessTypes BusinessType { get; set; } = BusinessTypes.Empty;
    public List<BusinessActivityDto> BusinessActivities { get; set; } = new List<BusinessActivityDto>();
    public bool[] WorkingDays { get; set; } = new bool[7];
    public int[] StartingHours { get; set; } = new int[7];
    public int[] EndingHours { get; set; } = new int[7];
}
