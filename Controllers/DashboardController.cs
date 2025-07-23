using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace YouFinanceIt.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
