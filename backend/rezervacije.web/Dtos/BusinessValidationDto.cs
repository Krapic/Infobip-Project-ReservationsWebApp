
namespace Rezervacije.Dtos;

public class BusinessValidationDto
{
    public required bool IsValidBusiness { get; set; }
    public string AdminComment { get; set; } = string.Empty;
    public required string BusinessName { get; set; }
}
