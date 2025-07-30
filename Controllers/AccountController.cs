// Controllers/AccountController.cs
using Microsoft.AspNetCore.Mvc;
using YouFinanceIt.Models; // For ApplicationUser, Account, ViewModels
using YouFinanceIt.Models.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity; // Required for UserManager and SignInManager
using Microsoft.AspNetCore.Authentication; // Still needed for SignOutAsync
using Microsoft.AspNetCore.Authorization; // For [Authorize] attribute
using YouFinanceIt.Data; // Required for ApplicationDbContext
using Microsoft.Extensions.Logging; // Added for logging errors

namespace YouFinanceIt.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountController> _logger; // Added logger

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            ILogger<AccountController> logger) // Inject logger
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger; // Initialize logger
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
                    try
                    {
                        // Optional: Create a default financial account for the new user
                        var defaultAccount = new Account // Account model is now in YouFinanceIt.Models
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
                    catch (Exception ex)
                    {
                        // Log the exception details
                        _logger.LogError(ex, "Error creating default account for new user {UserId} - {Email}", user.Id, user.Email);
                        ModelState.AddModelError(string.Empty, "An error occurred while setting up your account. Please try again or contact support.");
                        // If there's a database error, don't sign in, just return the view with an error message.
                        // You might want to delete the partially created user if the account creation is critical.
                        // For now, we'll just log and display an error.
                        // await _userManager.DeleteAsync(user); // Uncomment if you want to delete user on account creation failure
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            // If model state is not valid or registration failed, return the same view with validation errors
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
                    // Redirect to the new Dashboard Index page after successful login
                    return RedirectToAction("Index", "Dashboard");
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
