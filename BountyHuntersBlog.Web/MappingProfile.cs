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

        CreateMap<MissionDto, MissionListItemViewModel>()
            .ForMember(d => d.CreatedOn, m => m.Ignore()); // няма в DTO; идва от Details или добави в DTO при нужда

        CreateMap<MissionWithStatsDto, MissionDetailsViewModel>()
            .ForMember(d => d.Tags, m => m.MapFrom(s => s.Tags));

        CreateMap<TagDto, TagViewModel>();
        CreateMap<CategoryDto, CategoryViewModel>();
    }
}