using AutoMapper;
using System.Linq;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Services.DTOs;

public sealed class TestMappingProfile : Profile
{
    public TestMappingProfile()
    {
        // ===== Missions =====
        CreateMap<MissionEditDto, Mission>()
            .ForMember(d => d.Category, o => o.Ignore())
            .ForMember(d => d.Comments, o => o.Ignore())
            .ForMember(d => d.Likes, o => o.Ignore())
            .ForMember(d => d.MissionTags, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.CreatedOn, o => o.Ignore());

        CreateMap<MissionDto, Mission>()
            .ForMember(d => d.Category, o => o.Ignore())
            .ForMember(d => d.Comments, o => o.Ignore())
            .ForMember(d => d.Likes, o => o.Ignore())
            .ForMember(d => d.MissionTags, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.CreatedOn, o => o.Ignore());

        CreateMap<Mission, MissionDto>()
            .ForMember(d => d.TagIds, o => o.MapFrom(s => s.MissionTags.Select(mt => mt.TagId)))
            .ForMember(d => d.TagNames, o => o.MapFrom(s => s.MissionTags
                                                          .Where(mt => mt.Tag != null)
                                                          .Select(mt => mt.Tag.Name)));

        // ===== Categories =====
        CreateMap<CategoryDto, Category>()
            .ForMember(d => d.CreatedOn, o => o.Ignore())
            .ForMember(d => d.ModifiedOn, o => o.Ignore())
            .ForMember(d => d.Missions, o => o.Ignore());

        CreateMap<Category, CategoryDto>();

        // ===== Comments =====
        CreateMap<Comment, CommentDto>();

        CreateMap<CommentDto, Comment>()
            .ForMember(d => d.Mission, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.Likes, o => o.Ignore());
    }
}
