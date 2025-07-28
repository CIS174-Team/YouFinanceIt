using Microsoft.AspNetCore.Mvc;
using YouFinanceIt.Services;
using System.Threading.Tasks;
using YouFinanceIt.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[Authorize]
public class TransactionController : Controller
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    // GET: /Transaction/
    public async Task<IActionResult> Index()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var transactions = await _transactionService.GetAllAsync(userId);
        return View(transactions);
    }

    // GET: /Transaction/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Transaction/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Transaction transaction)
    {
        if (ModelState.IsValid)
        {
            transaction.UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _transactionService.AddAsync(transaction);
            return RedirectToAction(nameof(Index));
        }
        return View(transaction);
    }
}
