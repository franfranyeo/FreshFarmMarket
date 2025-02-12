using Microsoft.AspNetCore.HttpsPolicy;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;


namespace FreshFarmMarket.Services
{
    public class SmsService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _twilioNumber;
        public SmsService(IConfiguration configuration)
        {
            _accountSid = configuration["Twilio:AccountSid"];
            _authToken = configuration["Twilio:AuthToken"];
            _twilioNumber = configuration["Twilio:PhoneNumber"];

        }

        public Task SendSms(string toPhoneNumber, string message)
        {
            TwilioClient.Init(_accountSid, _authToken);

            return MessageResource.CreateAsync(
              to: new PhoneNumber(toPhoneNumber),
              from: new PhoneNumber(_twilioNumber),
              body: message);
        }
    }
}


