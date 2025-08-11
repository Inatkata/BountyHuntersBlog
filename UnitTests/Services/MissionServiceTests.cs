using System.Security.Claims;
using AutoMapper;
using Moq;
using Xunit;
using BountyHuntersBlog.Services.Implementations;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Services.DTOs;

public class MissionServiceTests
{
    private static ClaimsPrincipal User(string id = "u1", bool admin = false)
        => new(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, id),
            new Claim(ClaimTypes.Role, admin ? "Admin" : "User")
        }, "mock"));

    [Fact]
    public async Task SearchPagedAsync_Filters_And_Paginates()
    {
        // arrange
        var missions = new List<Mission>
        {
            new() { Id=1, Title="Alpha", Description="x", CategoryId=1, CreatedOn=DateTime.UtcNow },
            new() { Id=2, Title="Beta", Description="y", CategoryId=2, CreatedOn=DateTime.UtcNow.AddMinutes(-1) }
        };

        var mRepo = new Mock<IMissionRepository>();
        mRepo.Setup(r => r.AllAsync()).ReturnsAsync(missions);

        var likeRepo = new Mock<ILikeRepository>();
        var comRepo = new Mock<ICommentRepository>();
        var tagRepo = new Mock<ITagRepository>();

        var mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Mission, MissionDto>()
               .ForMember(d => d.TagIds, opt => opt.MapFrom(s => s.MissionTags.Select(mt => mt.TagId)));
        }).CreateMapper();

        var svc = new MissionService(mRepo.Object, likeRepo.Object, comRepo.Object, tagRepo.Object, mapper);

        // act
        var (items, total) = await svc.SearchPagedAsync("a", null, null, 1, 10);

        // assert
        Assert.Equal(2, total);
        Assert.Equal(2, items.Count);
        Assert.Contains(items, i => i.Title == "Alpha");
    }

    [Fact]
    public async Task UpdateAsync_Throws_When_NotUser_And_NotAdmin()
    {
        var entity = new Mission { Id = 5, Title = "T", Description = "D", CategoryId = 1, UserId = "User1", MissionTags = new List<MissionTag>() };
        var mRepo = new Mock<IMissionRepository>();
        mRepo.Setup(r => r.GetByIdWithIncludesAsync(5)).ReturnsAsync(entity);
        var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Mission, MissionDto>()).CreateMapper();

        var svc = new MissionService(mRepo.Object, Mock.Of<ILikeRepository>(), Mock.Of<ICommentRepository>(), Mock.Of<ITagRepository>(), mapper);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await svc.UpdateAsync(5, new MissionDto { Title = "N", Description = "X", CategoryId = 1, TagIds = new[] { 1, 2 } }, User("u2")));
    }
}
