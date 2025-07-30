// Controllers/FinancialAccountController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouFinanceIt.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using YouFinanceIt.Models; // Now includes Account

namespace YouFinanceIt.Controllers
{
    [Authorize]
    public class FinancialAccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FinancialAccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var accounts = await _context.Accounts
                .Where(a => a.UserID == userId)
                .Include(a => a.Transactions)
                .ToListAsync();

            return View(accounts);
        }

        // REVIEW: Add View/Edit/Delete methods?
    }
}