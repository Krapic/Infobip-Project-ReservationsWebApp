namespace Rezervacije.Dtos;

public class LoginDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class RegisterDto
{
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Town { get; set; }
    public required string Country { get; set; }
    public required string PostalCode { get; set; }
    public required string DialingCode { get; set; }
    public required string HouseNumber { get; set; }
    public required string Street { get; set; }
}
