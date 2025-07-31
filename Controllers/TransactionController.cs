using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YouFinanceIt.Data;
using YouFinanceIt.Models;
using System.Security.Claims;

namespace YouFinanceIt.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<IActionResult> Index()
        {
            string userId = GetUserId();
            var transactions = await _context.Transactions
                .Include(t => t.Account)
                .Where(t => t.UserID == userId)
                .ToListAsync();

            return View(transactions);
        }

        public IActionResult Create()
        {
            string userId = GetUserId();
            ViewBag.AccountID = new SelectList(_context.Accounts.Where(a => a.UserID == userId), "AccountID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Transaction transaction)
        {
            transaction.UserID = GetUserId(); // Set this BEFORE validation

            if (!ModelState.IsValid)
            {
                // Optional: log errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                Console.WriteLine(string.Join("; ", errors));

                string userId = GetUserId();
                ViewBag.AccountID = new SelectList(_context.Accounts.Where(a => a.UserID == userId), "AccountID", "Name", transaction.AccountID);
                return View(transaction);
            }

            _context.Add(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
