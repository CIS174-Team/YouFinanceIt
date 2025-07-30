// Controllers/DashboardController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YouFinanceIt.Controllers
{
    // [Authorize] attribute ensures only logged-in users can access actions in this controller
    [Authorize]
    public class DashboardController : Controller
    {
        // GET: /Dashboard/Index
        // This will be the landing page after successful login
        public IActionResult Index()
        {
            return View();
        }
    }
}
