// BountyHuntersBlog.Web/MappingProfile.cs
using AutoMapper;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.ViewModels;
using BountyHuntersBlog.ViewModels.Missions;
using System.Linq;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
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

        CreateMap<MissionDto, BountyHuntersBlog.ViewModels.Missions.MissionListItemViewModel>();
        CreateMap<MissionDto, BountyHuntersBlog.ViewModels.Missions.MissionEditViewModel>().ReverseMap();
        CreateMap<MissionDto, BountyHuntersBlog.ViewModels.Missions.MissionViewModel>().ReverseMap();

        CreateMap<TagDto, TagViewModel>();
        CreateMap<CategoryDto, CategoryViewModel>();
        CreateMap<MissionDto, MissionViewModel>().ReverseMap();
        CreateMap<MissionWithStatsDto, MissionDto>().ReverseMap();

    }
}