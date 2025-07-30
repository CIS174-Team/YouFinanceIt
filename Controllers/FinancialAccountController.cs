// Controllers/FinancialAccountController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouFinanceIt.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using YouFinanceIt.Models; // Now includes Account
using System.Diagnostics;

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

        // GET: FinancialAccount/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FinancialAccount/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Account account)
        {
            account.UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            account.CreatedDate = DateTime.UtcNow;

            Debug.WriteLine("Create POST called");  //debugging
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            Debug.WriteLine("ModelState invalid:");
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Debug.WriteLine($"{state.Key}: {error.ErrorMessage}");
                }
            }

            return View(account);
        }
    }
}