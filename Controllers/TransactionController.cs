using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YouFinanceIt.Models;
using YouFinanceIt.Services;

namespace YouFinanceIt.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<IActionResult> Index()
        {
            var username = User.Identity.Name;
            var transactions = await _transactionService.GetUserTransactionsAsync(username);
            return View(transactions);
        }

        public async Task<IActionResult> Details(int id)
        {
            var username = User.Identity.Name;
            var transaction = await _transactionService.GetTransactionByIdAsync(id, username);
            if (transaction == null) return NotFound();
            return View(transaction);
        }

        public IActionResult Create()
        {
            // TODO: Load ViewBag.Accounts and ViewBag.Categories here from services for dropdowns
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                // TODO: Reload ViewBag.Accounts and ViewBag.Categories here
                return View(transaction);
            }

            var username = User.Identity.Name;
            await _transactionService.AddTransactionAsync(transaction, username);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var username = User.Identity.Name;
            var transaction = await _transactionService.GetTransactionByIdAsync(id, username);
            if (transaction == null) return NotFound();

            // TODO: Load ViewBag.Accounts and ViewBag.Categories here
            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Transaction transaction)
        {
            if (id != transaction.TransactionID) return BadRequest();

            if (!ModelState.IsValid)
            {
                // TODO: Reload ViewBag.Accounts and ViewBag.Categories here
                return View(transaction);
            }

            var username = User.Identity.Name;
            await _transactionService.UpdateTransactionAsync(transaction, username);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var username = User.Identity.Name;
            var transaction = await _transactionService.GetTransactionByIdAsync(id, username);
            if (transaction == null) return NotFound();
            return View(transaction);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var username = User.Identity.Name;
            await _transactionService.DeleteTransactionAsync(id, username);
            return RedirectToAction(nameof(Index));
        }
    }
}
