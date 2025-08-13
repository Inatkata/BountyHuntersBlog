// BountyHuntersBlog.Web/MappingProfile.cs
using AutoMapper;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.ViewModels.Admin.Categories;
using BountyHuntersBlog.ViewModels.Admin.Comments;
using BountyHuntersBlog.ViewModels.Admin.Missions;
using BountyHuntersBlog.ViewModels.Admin.Tags;
using BountyHuntersBlog.ViewModels.Admin.Users;
using BountyHuntersBlog.ViewModels.Category;
using BountyHuntersBlog.ViewModels.Comments;
using BountyHuntersBlog.ViewModels.Missions;
using BountyHuntersBlog.ViewModels.Tag;
using System.Linq;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ===== Users =====
        CreateMap<ApplicationUser, AdminUserListItemVM>()
            .ForMember(d => d.MissionsCount, o => o.MapFrom(s => s.Missions.Count))
            .ForMember(d => d.CommentsCount, o => o.MapFrom(s => s.Comments.Count))
            .ForMember(d => d.LikesCount, o => o.MapFrom(s => s.Likes.Count));

        // ===== Categories =====
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<CategoryDto, CategoryViewModel>();
        CreateMap<CategoryDto, AdminCategoryListItemVM>();
        CreateMap<CategoryDto, AdminCategoryFormVM>().ReverseMap();

        // ===== Tags =====
        CreateMap<Tag, TagDto>()
            .ForMember(d => d.MissionsCount, o => o.MapFrom(s => s.MissionTags.Count));
        CreateMap<TagDto, Tag>();
        CreateMap<TagDto, TagViewModel>();
        CreateMap<TagDto, AdminTagListItemVM>();
        CreateMap<TagDto, AdminTagFormVM>().ReverseMap();

        // ===== Comments =====
        CreateMap<Comment, CommentDto>()
            .ForMember(d => d.UserDisplayName, o => o.MapFrom(s => s.User != null
                ? (string.IsNullOrWhiteSpace(s.User.DisplayName) ? s.User.UserName : s.User.DisplayName)
                : "User"));
        CreateMap<CommentDto, Comment>();
        CreateMap<CommentDto, CommentViewModel>()
            .ForMember(d => d.UserName, m => m.MapFrom(s => s.UserDisplayName));

        CreateMap<Comment, AdminCommentListItemVM>()
            .ForMember(d => d.MissionTitle, o => o.MapFrom(s => s.Mission.Title))
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.User != null
                ? (string.IsNullOrWhiteSpace(s.User.DisplayName) ? s.User.UserName : s.User.DisplayName)
                : "User"))
            .ForMember(d => d.LikesCount, o => o.MapFrom(s => s.Likes.Count));

        // ===== Missions: Entity <-> DTO =====
        CreateMap<Mission, MissionDto>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category != null ? s.Category.Name : ""))
            .ForMember(d => d.TagIds, o => o.MapFrom(s => s.MissionTags.Select(mt => mt.TagId)))
            .ForMember(d => d.TagNames, o => o.MapFrom(s => s.MissionTags.Select(mt => mt.Tag.Name)));

        CreateMap<MissionDto, Mission>()
            .ForMember(d => d.MissionTags, o => o.Ignore()); // таговете се управляват в сервиса
                                                             // (optionally map simple fields directly by convention)

        // ===== Admin forms <-> DTO =====
        CreateMap<MissionDto, AdminMissionFormVM>()
            .ForMember(d => d.TagIds, o => o.MapFrom(s => s.TagIds ?? Enumerable.Empty<int>()));
        CreateMap<AdminMissionFormVM, MissionDto>();

        // ===== Public VMs =====
        CreateMap<MissionDto, MissionListItemVm>()
            .ForMember(d => d.TagNames, o => o.MapFrom(s => s.TagNames ?? Enumerable.Empty<string>()));

        CreateMap<MissionDto, MissionEditViewModel>()
            .ForMember(d => d.TagIds, o => o.MapFrom(s => s.TagIds ?? Enumerable.Empty<int>()));
        CreateMap<MissionEditViewModel, MissionDto>();

        // ===== Details =====
        CreateMap<MissionDetailsDto, MissionDetailsViewModel>()
            .ForMember(d => d.TagNames, m => m.MapFrom(s => s.TagNames ?? Enumerable.Empty<string>()));
        // Ако вместо това ползваш MissionWithStatsDto в проекта ти, можеш да запазиш и този мап:
        // CreateMap<MissionWithStatsDto, MissionDetailsViewModel>()
        //     .ForMember(d => d.TagNames,    m => m.MapFrom(s => s.TagNames ?? Enumerable.Empty<string>()))
        //     .ForMember(d => d.CategoryName,m => m.MapFrom(s => s.CategoryName))
        //     .ForMember(d => d.LikesCount,  m => m.MapFrom(s => s.LikesCount))
        //     .ForMember(d => d.ImageUrl,    m => m.MapFrom(s => s.ImageUrl));
    }
}
