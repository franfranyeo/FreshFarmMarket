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
            _accountSid = configuration["Twilio:AccountSid"]
                ?? throw new ArgumentNullException("Twilio:AccountSid configuration is missing");
            _authToken = configuration["Twilio:AuthToken"]
                ?? throw new ArgumentNullException("Twilio:AuthToken configuration is missing");
            _twilioNumber = configuration["Twilio:PhoneNumber"]
                ?? throw new ArgumentNullException("Twilio:PhoneNumber configuration is missing");
        }

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


