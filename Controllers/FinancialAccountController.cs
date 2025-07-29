using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouFinanceIt.Data;

namespace YouFinanceIt.Controllers
{
    public class FinancialAccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FinancialAccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var accounts = await _context.Accounts
                .Include(a => a.User)
                .ToListAsync();

            return View(accounts);
        }

        // REVIEW: Add View/Edit/Delete methods?
    }
}