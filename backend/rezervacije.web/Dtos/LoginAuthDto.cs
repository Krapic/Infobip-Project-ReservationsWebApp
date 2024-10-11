namespace Rezervacije.Dtos;

public class LoginAuthDto
{
    public required string Email { get; set; }
    public required string SixDigitCode { get; set; }
}
