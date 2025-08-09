using BountyHuntersBlog.Data;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Data.Seeds;
using BountyHuntersBlog.Repositories;
using BountyHuntersBlog.Repositories.Extensions;
using BountyHuntersBlog.Repositories.Implementations;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Services;
using BountyHuntersBlog.Services.Extensions;
using BountyHuntersBlog.Services.Implementations;
using BountyHuntersBlog.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Регистрация на DbContext
builder.Services.AddDbContext<BountyHuntersDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.EnableRetryOnFailure()));

// 2. Настройка на Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<BountyHuntersDbContext>()
    .AddDefaultTokenProviders();

// 3. Регистрация на репозитории
builder.Services.AddRepositories(); // ако имаш общия метод в Repositories.Extensions

// или директно така, ако искаш ръчно:
builder.Services.AddScoped<IMissionRepository, MissionRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ILikeRepository, LikeRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IMissionTagRepository, MissionTagRepository>();

// 4. Регистрация на услуги
builder.Services.AddApplicationServices(); // ако имаш общия метод в Services.Extensions

// или ръчно:
builder.Services.AddScoped<IMissionService, MissionService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IMissionTagService, MissionTagService>();

// 5. AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// 6. MVC + Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// 7. Seed данни (ако имаш DbSeeder)
using (var scope = app.Services.CreateScope())
{
    await DbSeeder.SeedAsync(scope.ServiceProvider);
}

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithReExecute("/Home/StatusCode", "?code={0}");
}
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapRazorPages();

app.Run();
