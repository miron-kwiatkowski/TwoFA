using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TwoFA;
using TwoFA.Data;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;



// Dodaj DbContext z SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dodaj Identity z wymogiem potwierdzenia konta email i tokenami (m.in. 2FA)
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Dodaj uwierzytelnianie Google
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        IConfigurationSection googleAuthNSection =
        config.GetSection("Authentication:Google");
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireTwoFactorOrGoogle", policy =>
        policy.Requirements.Add(new TwoFactorOrGoogleRequirement()));
});

builder.Services.AddSingleton<IAuthorizationHandler, TwoFactorOrGoogleHandler>();

// Dodaj kontrolery i widoki MVC oraz Razor Pages (dla Identity UI)
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Dodaj middleware uwierzytelniania i autoryzacji
app.UseAuthentication();
app.UseAuthorization();

// Mapuj domyślną trasę MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Mapuj Razor Pages (w tym Identity)
app.MapRazorPages();

var port = Environment.GetEnvironmentVariable("PORT") ?? "80";
app.Urls.Add($"http://*:{port}");


app.Run();
