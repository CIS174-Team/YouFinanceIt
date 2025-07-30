// Controllers/AccountController.cs
using Microsoft.AspNetCore.Mvc;
using YouFinanceIt.Models; // Now includes Account, ApplicationUser, ViewModels
using YouFinanceIt.Models.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using YouFinanceIt.Data; // Required for ApplicationDbContext

namespace YouFinanceIt.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, CreatedDate = DateTime.UtcNow };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // REQUIREMENT: Do NOT sign in the user immediately after registration.
                    // Instead, redirect them to the login page.

                    // Optional: Create a default financial account for the new user
                    var defaultAccount = new Account // Changed from YouFinanceIt.Account.Account
                    {
                        UserID = user.Id, // Link to the newly created Identity user's ID (string)
                        AccountName = "Primary Checking",
                        AccountType = "Checking",
                        Balance = 0.00m,
                        CreatedDate = DateTime.UtcNow
                    };
                    _context.Accounts.Add(defaultAccount);
                    await _context.SaveChangesAsync(); // Save the new account to the database

                    TempData["SuccessMessage"] = "Registration successful! Please log in with your new account.";
                    // Redirect to the Login page after successful registration
                    return RedirectToAction("Login", "Account");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // REQUIREMENT: Login successful, redirect to the FinancialAccount Index page.
                    return RedirectToAction("Index", "FinancialAccount");
                }

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Account locked out. Please try again later.");
                }
                else if (result.IsNotAllowed)
                {
                    ModelState.AddModelError(string.Empty, "Login not allowed. Please confirm your email or contact support.");
                }
                else if (result.RequiresTwoFactor)
                {
                    ModelState.AddModelError(string.Empty, "Two-factor authentication required.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your email and password.");
                }
            }
            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        // GET: /Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
