using BountyHuntersBlog.Data;
using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Repositories;
using BountyHuntersBlog.Services;
using BountyHuntersBlog.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BountyHuntersDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<Hunter, IdentityRole>()
    .AddEntityFrameworkStores<BountyHuntersDbContext>();
 


builder.Services.Configure<IdentityOptions>(options =>
{
    // Default password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

// Dependency injection for repositories
builder.Services.AddScoped<IMissionPostRepository, MissionPostRepository>();
builder.Services.AddScoped<IMissionCommentRepository, MissionCommentRepository>();
builder.Services.AddScoped<IMissionLikeRepository, MissionLikeRepository>();
builder.Services.AddScoped<IFactionRepository, FactionRepository>();
builder.Services.AddScoped<IImageRepository, CloudinaryImageRepository>();
builder.Services.AddScoped<IMissionService, MissionService>();
builder.Services.AddScoped<IFactionService, FactionService>();



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
    name: "areas",
    pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<Hunter>>();

    await DbSeeder.SeedRolesAndAdminAsync(roleManager, userManager);
}





app.Run();