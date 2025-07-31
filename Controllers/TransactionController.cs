// Controllers/TransactionController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using YouFinanceIt.Data;
using YouFinanceIt.Services;
using YouFinanceIt.Models;
using TransactionModel = YouFinanceIt.Models.Transaction;

namespace YouFinanceIt.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly ApplicationDbContext _context;

        public TransactionController(ITransactionService transactionService, ApplicationDbContext context)
        {
            _transactionService = transactionService;
            _context = context;
        }

        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var transactions = await _transactionService.GetAllAsync(userId);
            return View(transactions);
        }

        // GET: Transaction/Create
        public IActionResult Create(int? accountId)
        {
            var userId = GetUserId();
            var transaction = new TransactionModel
            {
                AccountID = accountId ?? 0
            };

            ViewData["AccountID"] = new SelectList(_context.Accounts.Where(a => a.UserID == userId), "AccountID", "AccountName", transaction.AccountID);
            // ViewData["CategoryID"] = new SelectList(_context.Categories.Where(c => c.UserID == userId || c.UserID == null), "CategoryID", "CategoryName");
            return View(transaction);
        }

        // POST: Transaction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransactionModel transaction)
        {
            transaction.UserID = GetUserId();

            if (ModelState.IsValid)
            {
                transaction.CreatedDate = DateTime.UtcNow;
                await _transactionService.AddAsync(transaction);
                return RedirectToAction(nameof(Index));
            }

            var userId = GetUserId();
            ViewData["AccountID"] = new SelectList(_context.Accounts.Where(a => a.UserID == userId), "AccountID", "AccountName", transaction.AccountID);
            // ViewData["CategoryID"] = new SelectList(_context.Categories.Where(c => c.UserID == userId || c.UserID == null), "CategoryID", "CategoryName", transaction.CategoryID);
            return View(transaction);
        }

        // GET: Transaction/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var userId = GetUserId();
            var transaction = await _transactionService.GetByIdAsync(id, userId);
            if (transaction == null)
            {
                return NotFound();
            }

            ViewData["AccountID"] = new SelectList(_context.Accounts.Where(a => a.UserID == userId), "AccountID", "AccountName", transaction.AccountID);
            // ViewData["CategoryID"] = new SelectList(_context.Categories.Where(c => c.UserID == userId || c.UserID == null), "CategoryID", "CategoryName", transaction.CategoryID);
            return View(transaction);
        }

        // POST: Transaction/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TransactionModel transaction)
        {
            if (id != transaction.TransactionID)
            {
                return NotFound();
            }

            transaction.UserID = GetUserId();

            if (ModelState.IsValid)
            {
                await _transactionService.UpdateAsync(transaction);
                return RedirectToAction(nameof(Index));
            }

            var userId = GetUserId();
            ViewData["AccountID"] = new SelectList(_context.Accounts.Where(a => a.UserID == userId), "AccountID", "AccountName", transaction.AccountID);
            // ViewData["CategoryID"] = new SelectList(_context.Categories.Where(c => c.UserID == userId || c.UserID == null), "CategoryID", "CategoryName", transaction.CategoryID);
            return View(transaction);
        }

        // GET: Transaction/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();
            var transaction = await _transactionService.GetByIdAsync(id, userId);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = GetUserId();
            await _transactionService.DeleteAsync(id, userId);
            return RedirectToAction(nameof(Index));
        }
    }
}
