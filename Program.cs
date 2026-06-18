using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Feline_Gallery_v1.Data;
using Feline_Gallery_v1.Models;
using Feline_Gallery_v1.Services;
using Feline_Gallery_v1.Authorization;
using Feline_Gallery_v1.Models.Interfaces;
using Feline_Gallery_v1.Models.Repsitories;
using Feline_Gallery_v1.Authorization.Handler;
using Feline_Gallery_v1.Authorization.Requirement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(
//        builder.Configuration.GetConnectionString("DefaultConnection") // ? NEW
//    )
//);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
   options.UseSqlite($"Data Source={Path.Combine(builder.Environment.ContentRootPath, "FelineGallery.db")}")
);

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;

    // User settings
    options.User.RequireUniqueEmail = true;

    // Sign-in settings
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configure Cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
});

// Add Authorization with Claims-Based Policies
builder.Services.AddAuthorization(options =>
{
    // Admin Policies
    options.AddPolicy(Policies.RequireAdminAccess, policy =>
        policy.RequireClaim(CustomClaimTypes.Permission, Permissions.AccessAdminDashboard));

    options.AddPolicy(Policies.RequireArtworkManagement, policy =>
        policy.RequireClaim(CustomClaimTypes.Permission,
            Permissions.ViewArtworks,
            Permissions.CreateArtworks,
            Permissions.EditArtworks,
            Permissions.DeleteArtworks));

    options.AddPolicy(Policies.RequireCategoryManagement, policy =>
        policy.RequireClaim(CustomClaimTypes.Permission,
            Permissions.ViewCategories,
            Permissions.CreateCategories,
            Permissions.EditCategories,
            Permissions.DeleteCategories));

    options.AddPolicy(Policies.RequireArtistManagement, policy =>
        policy.RequireClaim(CustomClaimTypes.Permission,
            Permissions.ViewArtists,
            Permissions.CreateArtists,
            Permissions.EditArtists,
            Permissions.DeleteArtists));

    options.AddPolicy(Policies.RequireExhibitionManagement, policy =>
        policy.RequireClaim(CustomClaimTypes.Permission,
            Permissions.ViewExhibitions,
            Permissions.CreateExhibitions,
            Permissions.EditExhibitions,
            Permissions.DeleteExhibitions));

    options.AddPolicy(Policies.RequireOrderManagement, policy =>
        policy.RequireClaim(CustomClaimTypes.Permission,
            Permissions.ViewOrders,
            Permissions.ManageOrders));

    // Customer Policies
    options.AddPolicy(Policies.RequireCustomerAccess, policy =>
        policy.RequireAuthenticatedUser());

    options.AddPolicy(Policies.RequireOrderView, policy =>
        policy.RequireClaim(CustomClaimTypes.Permission, Permissions.ViewOwnOrders));
});

// Register Custom Authorization Handler
builder.Services.AddSingleton<IAuthorizationHandler,PermissionAuthorizationRequirementHandler>();

// Add Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddScoped<IArtworkRepository, ArtworkRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Register Services
builder.Services.AddScoped<IClaimsService, ClaimsService>();

// Register Repositories
builder.Services.AddScoped<IArtworkRepository, ArtworkRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddScoped<IExhibitionRepository, ExhibitionRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();

// Seed Roles and Claims
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
      var db = services.GetRequiredService<ApplicationDbContext>();
      await db.Database.MigrateAsync();   // creates DB + runs migrations
      await SeedRolesAndClaimsAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding roles and claims.");
    }
}

// Configure the HTTP request pipeline
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

app.UseSession();

// Admin Area Route
app.MapAreaControllerRoute(
    name: "admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}"
);

// Default Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();

// Method to seed roles and claims
async Task SeedRolesAndClaimsAsync(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var claimsService = serviceProvider.GetRequiredService<IClaimsService>();

    // Create roles if they don't exist (backward compatibility)
    string[] roleNames = { "Admin", "Customer" };
    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Create default admin user with claims
    var adminEmail = "admin@felinegallery.com";
    var adminPassword = "Admin@123";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = "Admin",
            LastName = "User",
            EmailConfirmed = true,
            CreatedAt = DateTime.Now
        };

        var result = await userManager.CreateAsync(admin, adminPassword);
        if (result.Succeeded)
        {
            // Add to Admin role (backward compatibility)
            await userManager.AddToRoleAsync(admin, "Admin");

            // Assign admin claims (POLICY-BASED AUTHORIZATION)
            await claimsService.AssignAdminClaimsAsync(admin);
        }
    }
}
