using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using YouFinanceIt.Data;
using YouFinanceIt.Models;

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

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

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
            ViewBag.AccountID = new SelectList(_context.Accounts.Where(a => a.UserID == userId), "AccountID", "AccountName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Transaction transaction)
        {
            // Set UserID before validating model state
            transaction.UserID = GetUserId();

            if (!ModelState.IsValid)
            {
                string userId = GetUserId();
                ViewBag.AccountID = new SelectList(_context.Accounts.Where(a => a.UserID == userId), "AccountID", "AccountName", transaction.AccountID);
                return View(transaction);
            }

            _context.Add(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
