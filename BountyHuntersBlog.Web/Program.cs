using BountyHuntersBlog.Data;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Extensions;
using BountyHuntersBlog.Services.Extensions;
using BountyHuntersBlog.Web.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ===== Database =====
builder.Services.AddDbContext<BountyHuntersDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.EnableRetryOnFailure()));

// ===== Identity =====
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<BountyHuntersDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = "/Account/Login";
    opt.AccessDeniedPath = "/error/403"; // unified error handling
});

// ===== MVC + Anti-forgery =====
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

// ===== Repositories & Services =====
builder.Services.AddRepositories();
builder.Services.AddApplicationServices();

// ===== AutoMapper =====
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

var app = builder.Build();

// ===== Error handling / HSTS =====
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error/500");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

// All status codes (404 etc.) -> ErrorController
app.UseStatusCodePagesWithReExecute("/error/{0}");

// ===== DB migrate + seed =====
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BountyHuntersDbContext>();
    await db.Database.MigrateAsync();

    await IdentitySeeder.SeedAsync(scope.ServiceProvider); // roles + admin
    await DataSeeder.SeedAsync(scope.ServiceProvider);     // categories/tags/missions
}

// ===== Pipeline =====
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// ===== Routes =====
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=AdminHome}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
