using Microsoft.Extensions.DependencyInjection;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Repositories.Implementations;
using BountyHuntersBlog.Repositories.Base;

namespace BountyHuntersBlog.Repositories.Extensions
{
    public static class RepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Регистрация на Generic Repository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Регистрация на конкретните репозитории
            services.AddScoped<IMissionRepository, MissionRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IMissionTagRepository, MissionTagRepository>();

            return services;
        }
    }
}