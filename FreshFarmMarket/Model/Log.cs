namespace FreshFarmMarket.Model
{
    public class Log
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string User { get; set; } = string.Empty;
        public string Action { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
