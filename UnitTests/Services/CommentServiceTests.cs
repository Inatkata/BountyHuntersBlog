using System.Security.Claims;
using Moq;
using Xunit;
using BountyHuntersBlog.Services.Implementations;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Data.Models;

public class CommentServiceTests
{
    private static ClaimsPrincipal User(string id = "u1", bool admin = false)
        => new(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, id),
            new Claim(ClaimTypes.Role, admin ? "Admin" : "User")
        }, "mock"));

    [Fact]
    public async Task AddAsync_Adds_Comment()
    {
        var mRepo = new Mock<IMissionRepository>();
        mRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Mission { Id = 1 });

        var cRepo = new Mock<ICommentRepository>();
        cRepo.Setup(r => r.AddAsync(It.IsAny<Comment>())).Returns(Task.CompletedTask);

        var svc = new CommentService(cRepo.Object, mRepo.Object);
        await svc.AddAsync(1, "hello", User("u1"));

        cRepo.Verify(r => r.AddAsync(It.Is<Comment>(c => c.MissionId == 1 && c.UserId == "u1" && c.Text == "hello")), Times.Once);
        cRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Blocks_NonOwner_NonAdmin()
    {
        var c = new Comment { Id = 5, UserId = "owner1" };
        var cRepo = new Mock<ICommentRepository>();
        cRepo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(c);

        var svc = new CommentService(cRepo.Object, Mock.Of<IMissionRepository>());

        await Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await svc.DeleteAsync(5, User("u2", admin: false)));
    }
}