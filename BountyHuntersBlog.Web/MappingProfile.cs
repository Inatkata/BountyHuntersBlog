using AutoMapper;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.ViewModels;
using BountyHuntersBlog.ViewModels.Missions;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Entities <-> DTOs
        CreateMap<Mission, MissionDto>().ReverseMap();
        CreateMap<Comment, CommentDto>().ReverseMap();
        CreateMap<Like, LikeDto>().ReverseMap();

        CreateMap<Category, CategoryDto>()
            .ForMember(d => d.MissionsCount, o => o.MapFrom(s => s.Missions.Count))
            .ReverseMap()
            .ForMember(s => s.Missions, o => o.Ignore());

        CreateMap<Tag, TagDto>()
            .ForMember(d => d.MissionsCount, o => o.MapFrom(s => s.MissionTags.Count))
            .ReverseMap()
            .ForMember(s => s.MissionTags, o => o.Ignore());

        // DTO -> ViewModel
        CreateMap<TagDto, TagViewModel>().ReverseMap();

        CreateMap<MissionDto, MissionViewModel>()
            .ForMember(vm => vm.UserName, o => o.Ignore())   // ще го попълниш по userId
            .ForMember(vm => vm.CategoryName, o => o.Ignore())   // ще го попълниш по CategoryId
            .ForMember(vm => vm.TagNames, o => o.Ignore())   // ще ги попълниш по TagIds
            .ForMember(vm => vm.CommentsCount, o => o.Ignore())
            .ForMember(vm => vm.LikesCount, o => o.Ignore());

        CreateMap<MissionDto, MissionListItemViewModel>()
            .ForMember(vm => vm.CategoryName, o => o.Ignore())
            .ForMember(vm => vm.TagNames, o => o.Ignore())
            .ForMember(vm => vm.CommentsCount, o => o.Ignore())
            .ForMember(vm => vm.LikesCount, o => o.Ignore());
        
    }
}