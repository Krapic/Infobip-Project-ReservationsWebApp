using Infobip.Api.Client.Api;
using Infobip.Api.Client.Model;
using Infobip.Api.Client;

namespace Rezervacije.Services;

public class InfobipApiService : IInfobipApiService
{
    private readonly IConfiguration _config;
    public InfobipApiService(IConfiguration config)
    {
        _config = config;
    }

    public void SendEmail(string email, string emailSubject, string emailText)
    {
        var configuration = new Configuration()
        {
            BasePath = _config.GetSection("InfobipSMS:BaseUrl").Value!,
            ApiKeyPrefix = "App",
            ApiKey = _config.GetSection("InfobipSMS:ApiKey").Value!
        };

        SendEmailApi sendEmailApi = new SendEmailApi(configuration);

        sendEmailApi.SendEmail(
            from: _config.GetSection("InfobipSMS:EmailSender").Value!,
            to: email,
            subject: emailSubject,
            text: emailText
        );
    }

    public void SendSMS(string phoneNumber, string SMSMesage)
    {
        string SENDER = "ReservMe";

        var configuration = new Configuration()
        {
            BasePath = _config.GetSection("InfobipSMS:BaseUrl").Value!,
            ApiKeyPrefix = "App",
            ApiKey = _config.GetSection("InfobipSMS:ApiKey").Value!
        };

        var sendSmsApi = new SendSmsApi(configuration);

        var smsMessage = new SmsTextualMessage()
        {
            From = SENDER,
            Destinations = new List<SmsDestination>()
                {
                    new SmsDestination(to: phoneNumber)
                },
            Text = SMSMesage
        };

        var smsRequest = new SmsAdvancedTextualRequest()
        {
            Messages = new List<SmsTextualMessage>() { smsMessage }
        };

        sendSmsApi.SendSmsMessage(smsRequest);
    }
}
