/// Controllers/AccountController.cs
using Microsoft.AspNetCore.Mvc;
using YouFinanceIt.Models;
using YouFinanceIt.Models.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity; // Required for UserManager and SignInManager
using Microsoft.AspNetCore.Authentication; // Still needed for SignOutAsync
using Microsoft.AspNetCore.Authorization; // For [Authorize] attribute
using YouFinanceIt.Data; // Required for ApplicationDbContext

namespace YouFinanceIt.Controllers
{
    public class AccountController : Controller
    {
        // Inject UserManager and SignInManager provided by ASP.NET Core Identity
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context; // Inject DbContext for potential direct use (e.g., in future for user-specific data)

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context; // Initialize DbContext
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken] // Important for security
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create a new ApplicationUser instance
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, CreatedDate = DateTime.UtcNow };

                // Use UserManager to create the user with the provided password.
                // UserManager handles password hashing automatically.
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // If user creation is successful, sign the user in.
                    await _signInManager.SignInAsync(user, isPersistent: false); // isPersistent = RememberMe

                    TempData["SuccessMessage"] = "Registration successful! Welcome to YouFinanceIt.";
                    // Redirect to a protected area, e.g., the Dashboard
                    return RedirectToAction("Index", "Dashboard"); // Assuming DashboardController.Index exists
                }

                // If there are errors (e.g., password too weak, email already exists)
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
        [ValidateAntiForgeryToken] // Important for security
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Use SignInManager to attempt to sign in the user.
                // It handles password verification against the hashed password in the database.
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // Login successful, redirect to the dashboard.
                    return RedirectToAction("Index", "Dashboard");
                }

                // Handle different login failure scenarios
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Account locked out.");
                }
                else if (result.IsNotAllowed)
                {
                    ModelState.AddModelError(string.Empty, "Login not allowed (e.g., email not confirmed).");
                }
                else if (result.RequiresTwoFactor)
                {
                    // This would redirect to a 2FA page if you implement it.
                    ModelState.AddModelError(string.Empty, "Two-factor authentication required.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            // If model state is not valid or login failed, return the same view with errors
            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        // [Authorize] // Optional: Only authenticated users can log out
        public async Task<IActionResult> Logout()
        {
            // Sign out the current user using SignInManager
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        // GET: /Account/AccessDenied (Optional: for unauthorized access)
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
