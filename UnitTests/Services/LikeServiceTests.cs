using System.Security.Claims;
using Moq;
using Xunit;
using BountyHuntersBlog.Services.Implementations;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Data.Models;

public class LikeServiceTests
{
    private static ClaimsPrincipal User(string id = "u1")
        => new(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, id) }, "mock"));

    [Fact]
    public async Task LikeMissionAsync_Adds_When_NotLiked()
    {
        var mission = new Mission { Id = 3, Likes = new List<Like>() };
        var mRepo = new Mock<IMissionRepository>();
        mRepo.Setup(r => r.GetByIdWithIncludesAsync(3)).ReturnsAsync(mission);

        var lRepo = new Mock<ILikeRepository>();
        lRepo.Setup(r => r.AddAsync(It.IsAny<Like>())).Returns(Task.CompletedTask);

        var svc = new LikeService(lRepo.Object, mRepo.Object, Mock.Of<ICommentRepository>());
        await svc.LikeMissionAsync(3, User("u1"));

        lRepo.Verify(r => r.AddAsync(It.Is<Like>(l => l.MissionId == 3 && l.UserId == "u1")), Times.Once);
        lRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task LikeCommentAsync_Returns_MissionId()
    {
        var comment = new Comment { Id = 10, MissionId = 7, Likes = new List<Like>() };
        var cRepo = new Mock<ICommentRepository>();
        cRepo.Setup(r => r.GetByIdWithIncludesAsync(10)).ReturnsAsync(comment);

        var lRepo = new Mock<ILikeRepository>();
        var svc = new LikeService(lRepo.Object, Mock.Of<IMissionRepository>(), cRepo.Object);

        var missionId = await svc.LikeCommentAsync(10, User("u2"));
        Assert.Equal(7, missionId);
    }
}