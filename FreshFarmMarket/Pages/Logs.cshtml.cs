using FreshFarmMarket.Model;
using FreshFarmMarket.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FreshFarmMarket.Pages
{
    [Authorize(Roles = "Admin")]
    public class LogsModel : PageModel
    {

        private readonly LogService _logService;

        public LogsModel(LogService logService)
        {
            _logService = logService;
        }

        public List<Log> Loglist { get; set; } = new();
        
        public async Task<IActionResult> OnGetAsync()
        {
            Loglist = await _logService.RetrieveAllLogs();

            return Page();
        }
    }
}
