using Microsoft.EntityFrameworkCore;
using FreshFarmMarket.Model;


namespace FreshFarmMarket.Services
{
    public class LogService
    {
        private readonly AuthDbContext _context;
        public LogService(AuthDbContext dataContext)
        {
            _context = dataContext;
        }
        public async Task<List<Log>> RetrieveAllLogs()
        {
            return await _context.Logs.OrderByDescending(log => log.CreateTime).ToListAsync();
        }
        public async Task RecordLogs(string action, string email)
        {
            if (action == "Login")
            {
                await _context.Logs.AddAsync(new Log()
                {
                    Action = "Login",
                    Description = string.Format("User: {0} has logged in.", email),
                    User = email,
                });
            }

            else if (action == "Logout")
            {
                await _context.Logs.AddAsync(new Log()
                {
                    Action = "Logout",
                    Description = string.Format("User {0} has logged out.", email),
                    User = email,
                });
            }

            else if (action == "Change Password")
            {
                await _context.Logs.AddAsync(new Log()
                {
                    Action = "Change Password",
                    Description = string.Format("User {0} has changed their password.", email),
                    User = email,
                });
            }

            else if (action == "Password Reset")
            {
                await _context.Logs.AddAsync(new Log()
                {
                    Action = "Password Reset",
                    Description = string.Format("User {0} has reset their password.", email),
                    User = email,
                });
            }

            else if (action == "Account Lockout")
            {
                await _context.Logs.AddAsync(new Log()
                {
                    Action = "Account Lockout",
                    Description = string.Format("User {0} has been locked out.", email),
                    User = email,
                });
            }

            else if (action == "Login (2FA)")
            {
                await _context.Logs.AddAsync(new Log()
                {
                    Action = "Login (2FA)",
                    Description = string.Format("User {0} has logged in using 2FA.", email),
                    User = email,
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
