using FreshFarmMarket.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;

namespace FreshFarmMarket.Services
{
    public class GoogleCaptchaService
    {
        private readonly IOptionsMonitor<GCaptchaConfig> _config;
        public GoogleCaptchaService(IOptionsMonitor<GCaptchaConfig> config) => _config = config;
        public async Task<GCaptchaResponse> VerifyToken(string token)
        {
            try
            {
                var url = $"https://www.google.com/recaptcha/api/siteverify?secret={_config.CurrentValue.SecretKey}&response={token}";

                using (var client = new HttpClient())
                {
                    var httpResult = await client.GetAsync(url);

                    var responseString = await httpResult.Content.ReadAsStringAsync();

                    var googleResult = JsonConvert.DeserializeObject<GCaptchaResponse>(responseString);

                    return new GCaptchaResponse
                    {
                        success = googleResult.success,
                        score = googleResult.score
                    };
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new GCaptchaResponse
                {
                    success = false,
                    score = 0
                };
            }
        }
    }
}
