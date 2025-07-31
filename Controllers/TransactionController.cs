using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using YouFinanceIt.Data;
using YouFinanceIt.Models;
using System.Linq;

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
            ViewBag.AccountID = new SelectList(_context.Accounts.Where(a => a.UserID == userId), "AccountID", "AccountName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Transaction transaction)
        {
            transaction.UserID = GetUserId();

            if (string.IsNullOrEmpty(transaction.UserID))
            {
                ModelState.AddModelError("", "User must be logged in to create a transaction.");
            }

            if (!ModelState.IsValid)
            {
                var userId = transaction.UserID;
                ViewBag.AccountID = new SelectList(_context.Accounts.Where(a => a.UserID == userId), "AccountID", "AccountName", transaction.AccountID);
                return View(transaction);
            }
            var account = await _context.Accounts
    .FirstOrDefaultAsync(a => a.AccountID == transaction.AccountID && a.UserID == transaction.UserID);
            //Find related account
            if (account == null)
            {
                ModelState.AddModelError("", "Associated account not found.");
                return View(transaction);
            }

            // Adjust balance
            if (transaction.Type == "Income")
            {
                account.Balance += transaction.Amount;
            }
            else if (transaction.Type == "Expense")
            {
                account.Balance -= transaction.Amount;
            }
            else
            {
                ModelState.AddModelError("Type", "Invalid transaction type.");
                return View(transaction);
            }
            _context.Add(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
