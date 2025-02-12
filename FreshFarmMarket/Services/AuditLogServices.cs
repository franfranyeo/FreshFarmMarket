//using Microsoft.EntityFrameworkCore;
//using FreshFarmMarket.Model;


//namespace FreshFarmMarket.Services
//{
//    public class AuditLogServices
//    {
//        private readonly AuthDbContext _context;
//        public AuditLogServices(AuthDbContext dataContext)
//        {
//            _context = dataContext;
//        }
//        public async Task<List<Log>> RetrieveAllLogs()
//        {
//            return await _context.Logs.OrderByDescending(log => log.CreateTime).ToListAsync();
//        }
//        public async Task RecordLogs(Actions action, string email)
//        {
//            if (action == Actions.Login)
//            {
//                await _context.Logs.AddAsync(new Log()
//                {
//                    Action = Actions.Login,
//                    Description = string.Format("User: {0} logged in.", email),
//                    User = email,
//                });
//            }

//            else if (action == Actions.Logout)
//            {
//                await _context.Logs.AddAsync(new Log()
//                {
//                    Action = Actions.Logout,
//                    Description = string.Format("User: {0} logged out.", email),
//                    User = email,
//                });
//            }

//            await _context.SaveChangesAsync();
//        }
//    }
//}
