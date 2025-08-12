// BountyHuntersBlog.Web/MappingProfile.cs
using AutoMapper;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.ViewModels;
using BountyHuntersBlog.ViewModels.Missions;
using BountyHuntersBlog.ViewModels.Admin.Categories;
using BountyHuntersBlog.ViewModels.Admin.Tags;
using BountyHuntersBlog.ViewModels.Admin.Missions;
using BountyHuntersBlog.ViewModels.Admin.Comments;
using BountyHuntersBlog.ViewModels.Admin.Users;
using System.Linq;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ===== Comments =====
        CreateMap<Comment, CommentDto>().ReverseMap();

        CreateMap<CommentDto, CommentViewModel>()
            .ForMember(d => d.UserName, m => m.MapFrom(s => s.UserDisplayName));

        CreateMap<ApplicationUser, AdminUserListItemVM>()
            .ForMember(d => d.MissionsCount, o => o.MapFrom(s => s.Missions.Count))
            .ForMember(d => d.CommentsCount, o => o.MapFrom(s => s.Comments.Count))
            .ForMember(d => d.LikesCount, o => o.MapFrom(s => s.Likes.Count));

        // ===== Users (roles се пълнят в контролера) =====
        CreateMap<ApplicationUser, AdminUserListItemVM>()
            .ForMember(d => d.MissionsCount, o => o.MapFrom(s => s.Missions.Count))
            .ForMember(d => d.CommentsCount, o => o.MapFrom(s => s.Comments.Count))
            .ForMember(d => d.LikesCount, o => o.MapFrom(s => s.Likes.Count));

        // ===== Categories =====
        CreateMap<Category, CategoryDto>().ReverseMap();

        CreateMap<Category, AdminCategoryListItemVM>()
            .ForMember(d => d.MissionsCount, o => o.MapFrom(s => s.Missions.Count));

        CreateMap<Category, AdminCategoryFormVM>().ReverseMap();

        // ===== Tags =====
        CreateMap<Tag, TagDto>().ReverseMap();

        CreateMap<Tag, AdminTagListItemVM>()
            .ForMember(d => d.MissionsCount, o => o.MapFrom(s => s.MissionTags.Count));

        CreateMap<Tag, AdminTagFormVM>().ReverseMap();

        // ===== Missions =====
        // Entity <-> DTO (TagIds от/към връзката)
        CreateMap<Mission, MissionDto>()
            .ForMember(d => d.TagIds, o => o.MapFrom(s => s.MissionTags.Select(mt => mt.TagId)));

        CreateMap<MissionDto, Mission>()
            .ForMember(d => d.MissionTags, o => o.Ignore()); // set in service/repo

        // Admin list item (от Entity)
        CreateMap<Mission, AdminMissionListItemVM>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
            .ForMember(d => d.LikesCount, o => o.MapFrom(s => s.Likes.Count));

        // Admin form (DTO <-> VM с TagIds)
        CreateMap<MissionDto, AdminMissionFormVM>()
            .ForMember(d => d.TagIds, o => o.MapFrom(s => s.TagIds ?? Enumerable.Empty<int>()));
        CreateMap<AdminMissionFormVM, MissionDto>();

        // Public VMs
        CreateMap<MissionDto, MissionListItemViewModel>();   // assumes properties align (Id, Title, CategoryName, TagNames, IsCompleted)
        CreateMap<MissionDto, MissionEditViewModel>()
            .ForMember(d => d.TagIds, o => o.MapFrom(s => s.TagIds ?? Enumerable.Empty<int>()));
        CreateMap<MissionEditViewModel, MissionDto>();

        CreateMap<MissionDto, MissionViewModel>().ReverseMap();

        // Details: Rich DTO -> Details VM
        CreateMap<MissionWithStatsDto, MissionDetailsViewModel>()
            .ForMember(d => d.TagNames, m => m.MapFrom(s => s.TagIds))   // or project from s.Tags if that’s the source
            .ForMember(d => d.CategoryName, m => m.MapFrom(s => s.CategoryName))
            .ForMember(d => d.LikesCount, m => m.MapFrom(s => s.LikesCount))
            .ForMember(d => d.ImageUrl, m => m.MapFrom(s => s.ImageUrl));

        // (по избор) взаимна конверсия ако ти трябва
        CreateMap<MissionWithStatsDto, MissionDto>().ReverseMap();
    }
}
