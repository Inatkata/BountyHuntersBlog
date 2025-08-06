using BountyHuntersBlog.Repositories;
using BountyHuntersBlog.Data;
using BountyHuntersBlog.Repositories;
using BountyHuntersBlog.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BountyHuntersDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthBountyHuntersDb")));

builder.Services.AddIdentity<IdentityHunter, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>();

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();