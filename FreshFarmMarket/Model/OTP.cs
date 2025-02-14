namespace FreshFarmMarket.Model
{
    public class OTP
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; } = false;
    }
}
