using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace FreshFarmMarket.Services
{
    public class SmsService(IConfiguration configuration)
    {
        private readonly string _accountSid = configuration["Twilio:AccountSid"];
        private readonly string _authToken = configuration["Twilio:AuthToken"];
        private readonly string _twilioNumber = configuration["Twilio:PhoneNumber"];

        public Task SendSms(string toPhoneNumber, string otp)
        {
            TwilioClient.Init(_accountSid, _authToken);
            var message = MessageResource.CreateAsync(
              to: new PhoneNumber(toPhoneNumber),
            from: new PhoneNumber(_twilioNumber),
              body: $"Your verification code is {otp}");

            return message;
        }
    }
}


