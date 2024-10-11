namespace Rezervacije.Web.Dtos;

public class RequestPasswordChangeDto
{
    public string Email { get; set; } = string.Empty;
}

public class AuthentificateEmailDto
{
    public string Email { get; set; } = string.Empty;
    public string SixDigitCode { get; set; } = string.Empty;
}

public class SetNewPasswordDto
{
    public string Email { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}