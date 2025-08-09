
using AutoMapper;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.ViewModels;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Data → DTO
        CreateMap<Mission, MissionDto>().ReverseMap();
        CreateMap<Comment, CommentDto>().ReverseMap();
        CreateMap<Like, LikeDto>().ReverseMap();
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Tag, TagDto>().ReverseMap();

        // DTO → ViewModel
        CreateMap<MissionDto, MissionViewModel>()
            .ForMember(vm => vm.AuthorName, opt => opt.MapFrom(d => d.AuthorId))
            .ForMember(vm => vm.CategoryName, opt => opt.MapFrom(d => d.CategoryId.ToString()))
            .ForMember(vm => vm.TagNames, opt => opt.MapFrom(d => d.TagIds.Select(id => id.ToString())))
            .ForMember(vm => vm.CommentsCount, opt => opt.Ignore())
            .ForMember(vm => vm.LikesCount, opt => opt.Ignore());
        CreateMap<CommentDto, CommentViewModel>()
            .ForMember(vm => vm.AuthorName, opt => opt.MapFrom(d => d.AuthorId));
        CreateMap<CategoryDto, CategoryViewModel>();
        CreateMap<TagDto, TagViewModel>();

    }
}