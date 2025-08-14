using AutoMapper;
using BountyHuntersBlog.Services.DTOs;
using DM = BountyHuntersBlog.Data.Models;  // alias to avoid Constants.* conflicts

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ===== Entities <-> DTOs (clean) =====
        CreateMap<DM.Category, CategoryDto>().ReverseMap();

        CreateMap<DM.Tag, TagDto>()
            .ForMember(d => d.MissionsCount, o => o.MapFrom(s => s.MissionTags.Count));
        CreateMap<TagDto, DM.Tag>();

        CreateMap<DM.Comment, CommentDto>()
            .ForMember(d => d.UserDisplayName, o => o.MapFrom(s =>
                s.User != null
                    ? (string.IsNullOrWhiteSpace(s.User.DisplayName) ? s.User.UserName : s.User.DisplayName)
                    : "User"));
        CreateMap<CommentDto, DM.Comment>();

        CreateMap<DM.Mission, MissionDto>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category != null ? s.Category.Name : ""))
            .ForMember(d => d.TagIds, o => o.MapFrom(s => s.MissionTags.Select(mt => mt.TagId)))
            .ForMember(d => d.TagNames, o => o.MapFrom(s => s.MissionTags.Select(mt => mt.Tag.Name)));
        CreateMap<MissionDto, DM.Mission>()
            .ForMember(d => d.MissionTags, o => o.Ignore());
    }
}