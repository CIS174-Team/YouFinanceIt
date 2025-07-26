using Microsoft.AspNetCore.Mvc;
using YouFinanceIt.Services;
using System.Threading.Tasks;
using YouFinanceIt.Data;
using Microsoft.AspNetCore.Authorization;

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
        int userId = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
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
            transaction.UserID = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
            await _transactionService.AddAsync(transaction);
            return RedirectToAction(nameof(Index));
        }
        return View(transaction);
    }

}
