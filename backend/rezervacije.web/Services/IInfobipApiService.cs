namespace Rezervacije.Services;

public interface IInfobipApiService
{
    public void SendSMS(string phoneNumber, string SMSMesage);
    public void SendEmail(string email, string emailSubject, string emailText);
}
