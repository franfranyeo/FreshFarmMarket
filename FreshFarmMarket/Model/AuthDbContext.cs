using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static System.Net.WebRequestMethods;

namespace FreshFarmMarket.Model
{
    public class AuthDbContext : IdentityDbContext<User>
    {
        public DbSet<OTP> OTPS { get; set; }
        public DbSet<Log> Logs { get; set; }

        private readonly IConfiguration _configuration;
        public AuthDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = _configuration.GetConnectionString("AuthConnectionString");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
