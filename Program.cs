using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YouFinanceIt.Data;
using YouFinanceIt.Models;
using YouFinanceIt.Services;
using YouFinanceIt.Filters; // Added for UserIdFilter

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString,
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        }));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Register your services
builder.Services.AddScoped<ITransactionService, TransactionService>();

// Register the custom UserIdFilter as a scoped service
builder.Services.AddScoped<UserIdFilter>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseMiddleware<YouFinanceIt.Middleware.ErrorHandlingMiddleware>();
app.UseStatusCodePagesWithReExecute("/Home/StatusCode", "?code={0}");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

// Seed test data before app starts (still commented out, but updated for ApplicationUser)
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

//    if (!db.Users.Any()) // This check might need adjustment if Identity tables are empty but not your custom ones
//    {
//        var user = new ApplicationUser // Use ApplicationUser
//        {
//            UserName = "testuser",
//            Email = "testuser@example.com",
//            CreatedDate = DateTime.UtcNow // Use UTC time
//        };

//        var result = await userManager.CreateAsync(user, "TestPassword123!"); // Create user with password

//        if (result.Succeeded)
//        {
//            var checking = new YouFinanceIt.Models.Account // Use correct Account model from Models namespace
//            {
//                UserID = user.Id, // Use string Id from ApplicationUser
//                AccountName = "Main Checking",
//                AccountType = "Checking",
//                Balance = 5432.10m,
//                CreatedDate = DateTime.UtcNow
//            };

//            var savings = new YouFinanceIt.Models.Account // Use correct Account model from Models namespace
//            {
//                UserID = user.Id, // Use string Id from ApplicationUser
//                AccountName = "Emergency Savings",
//                AccountType = "Savings",
//                Balance = 8500.00m,
//                CreatedDate = DateTime.UtcNow
//            };

//            db.Accounts.AddRange(checking, savings);
//            await db.SaveChangesAsync(); // Use async SaveChanges
//        }
//    }
//}


app.Run();
