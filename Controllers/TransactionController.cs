using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YouFinanceIt.Data;
using YouFinanceIt.Models;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using System;

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

        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            string userId = GetUserId();
            var transactions = await _context.Transactions
                .Include(t => t.Account)
                .Where(t => t.UserID == userId)
                .ToListAsync();

            return View(transactions);
        }

        // GET: Transaction/Create
        public IActionResult Create()
        {
            string userId = GetUserId();
            ViewBag.AccountID = new SelectList(_context.Accounts.Where(a => a.UserID == userId), "AccountID", "AccountName");
            return View(new Transaction());  // Pass new model instance to avoid null errors
        }

        // POST: Transaction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Transaction transaction)
        {
            transaction.UserID = GetUserId(); // Set before validation

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                Console.WriteLine("Validation Errors: " + string.Join("; ", errors));

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
