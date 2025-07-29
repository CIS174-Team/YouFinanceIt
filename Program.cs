// Program.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YouFinanceIt.Data; // Your DbContext namespace
using YouFinanceIt.Models; // Your ApplicationUser namespace

var builder = WebApplication.CreateBuilder(args);

// Configure database connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Add DbContext for your application (including Identity tables)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString,
        sqlServerOptionsAction: sqlOptions =>
        {
            // Enable retry on failure for transient errors (common with Azure SQL Database)
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5, // Number of retries
                maxRetryDelay: TimeSpan.FromSeconds(30), // Max delay between retries
                errorNumbersToAdd: null); // SQL error numbers to consider transient (null uses defaults)
        }));

// Add Identity services
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

// Seed test data before app starts
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (!db.Users.Any())
    {
        var user = new User
        {
            Username = "testuser",
            Email = "testuser@example.com",
            PasswordHash = "hashed_password_here",
            CreatedDate = DateTime.Now
        };

        db.Users.Add(user);
        db.SaveChanges();

        var checking = new Account
        {
            UserID = user.UserID,
            AccountName = "Main Checking",
            AccountType = "Checking",
            Balance = 5432.10m,
            CreatedDate = DateTime.Now
        };

        var savings = new Account
        {
            UserID = user.UserID,
            AccountName = "Emergency Savings",
            AccountType = "Savings",
            Balance = 8500.00m,
            CreatedDate = DateTime.Now
        };

        db.Accounts.AddRange(checking, savings);
        db.SaveChanges();
    }
}


app.Run();
