using Microsoft.Extensions.DependencyInjection;
using BountyHuntersBlog.Services.Interfaces;
using BountyHuntersBlog.Services.Implementations;

namespace BountyHuntersBlog.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ILikeService, LikeService>();
            services.AddScoped<IMissionTagService, MissionTagService>();
            services.AddScoped<ITagService, TagService>();

            return services;
        }
    }
}