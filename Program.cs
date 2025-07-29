using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YouFinanceIt.Data;
using YouFinanceIt.Models;
using YouFinanceIt.Services;
using YouFinanceIt.Filters;

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

// Register the custom filter
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

// Seed test data before app starts
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

//    if (!db.Users.Any())
//    {
//        var user = new User
//        {
//            Username = "testuser",
//            Email = "testuser@example.com",
//            PasswordHash = "hashed_password_here",
//            CreatedDate = DateTime.Now
//        };

//        db.Users.Add(user);
//        db.SaveChanges();

//        var checking = new Account
//        {
//            UserID = user.UserID,
//            AccountName = "Main Checking",
//            AccountType = "Checking",
//            Balance = 5432.10m,
//            CreatedDate = DateTime.Now
//        };

//        var savings = new Account
//        {
//            UserID = user.UserID,
//            AccountName = "Emergency Savings",
//            AccountType = "Savings",
//            Balance = 8500.00m,
//            CreatedDate = DateTime.Now
//        };

//        db.Accounts.AddRange(checking, savings);
//        db.SaveChanges();
//    }
//}


app.Run();
