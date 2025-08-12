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

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Comment, AdminCommentListItemVM>()
            .ForMember(d => d.MissionTitle, o => o.MapFrom(s => s.Mission.Title))
            .ForMember(d => d.UserDisplayName, o => o.MapFrom(s => s.User.DisplayName))
            .ForMember(d => d.LikesCount, o => o.MapFrom(s => s.Likes.Count));

        CreateMap<ApplicationUser, AdminUserListItemVM>()
            .ForMember(d => d.MissionsCount, o => o.MapFrom(s => s.Missions.Count))
            .ForMember(d => d.CommentsCount, o => o.MapFrom(s => s.Comments.Count))
            .ForMember(d => d.LikesCount, o => o.MapFrom(s => s.Likes.Count))
            // Roles -> ако идват от сервис като IEnumerable<string> в DTO, мапвай оттам.
            ;


        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Tag, TagDto>().ReverseMap();

        CreateMap<Comment, CommentDto>().ReverseMap();
        CreateMap<CommentDto, CommentViewModel>()
            .ForMember(d => d.UserName, m => m.MapFrom(s => s.UserDisplayName));

        CreateMap<Mission, MissionDto>()
            .ForMember(dest => dest.TagIds, opt => opt.MapFrom(src => src.MissionTags.Select(mt => mt.TagId)))
            .ReverseMap()
            .ForMember(dest => dest.MissionTags, opt => opt.Ignore());
        CreateMap<MissionWithStatsDto, MissionDetailsViewModel>()
            .ForMember(d => d.Tags, m => m.MapFrom(s => s.Tags));
        
        CreateMap<Mission, MissionDto>()
            .ForMember(d => d.TagIds, o => o.MapFrom(s => s.MissionTags.Select(mt => mt.TagId)));

        CreateMap<MissionDto, Mission>()
            .ForMember(d => d.MissionTags, o => o.Ignore()); 

        CreateMap<MissionDto, BountyHuntersBlog.ViewModels.Missions.MissionListItemViewModel>();
        CreateMap<MissionDto, BountyHuntersBlog.ViewModels.Missions.MissionEditViewModel>().ReverseMap();
        CreateMap<MissionDto, BountyHuntersBlog.ViewModels.Missions.MissionViewModel>().ReverseMap();

        CreateMap<Category, AdminCategoryListItemVM>()
            .ForMember(d => d.MissionsCount, o => o.MapFrom(s => s.Missions.Count));
        CreateMap<Category, AdminCategoryFormVM>().ReverseMap();

        CreateMap<Tag, AdminTagListItemVM>()
            .ForMember(d => d.MissionsCount, o => o.MapFrom(s => s.MissionTags.Count));
        CreateMap<Tag, AdminTagFormVM>().ReverseMap();

        CreateMap<Mission, AdminMissionListItemVM>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
            .ForMember(d => d.LikesCount, o => o.MapFrom(s => s.Likes.Count));

        CreateMap<Mission, AdminMissionFormVM>()
            .ForMember(d => d.TagIds, o => o.MapFrom(s => s.MissionTags != null ? s.MissionTags.Select(mt => mt.TagId) : new List<int>()))
            .ReverseMap()
            .ForMember(d => d.MissionTags, o => o.Ignore()); // ще го сетнем ръчно в сервиса

        CreateMap<Comment, AdminCommentListItemVM>()
            .ForMember(d => d.MissionTitle, o => o.MapFrom(s => s.Mission.Title))
            .ForMember(d => d.UserDisplayName, o => o.MapFrom(s => s.User.DisplayName))
            .ForMember(d => d.LikesCount, o => o.MapFrom(s => s.Likes.Count));


        CreateMap<TagDto, TagViewModel>();
        CreateMap<CategoryDto, CategoryViewModel>();
        CreateMap<MissionDto, MissionViewModel>().ReverseMap();
        CreateMap<MissionWithStatsDto, MissionDto>().ReverseMap();

    }
}