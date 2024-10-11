using Rezervacije.Data;

namespace Rezervacije.Dtos;

public class GetBusinessListFiltersDto
{
    public string UserEmail { get; set; } = string.Empty;
    public string Town { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public BusinessTypes BusinessType { get; set; } = BusinessTypes.Empty;
    public int BusinessCount { get; set; } = 0;
}
