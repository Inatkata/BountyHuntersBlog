using AutoMapper;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Implementations;
using BountyHuntersBlog.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
[TestFixture]
public class MissionServiceTests
{
    private Mock<IMissionRepository> _missionsRepo = null!;
    private Mock<ICommentRepository> _commentsRepo = null!;
    private Mock<ILikeRepository> _likesRepo = null!;
    private Mock<ICategoryRepository> _categoriesRepo = null!;
    private Mock<ITagRepository> _tagsRepo = null!;
    private Mock<IMissionTagRepository> _missionTagsRepo = null!;
    private IMissionService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _missionsRepo = new Mock<IMissionRepository>();
        _commentsRepo = new Mock<ICommentRepository>();
        _likesRepo = new Mock<ILikeRepository>();
        _categoriesRepo = new Mock<ICategoryRepository>();
        _tagsRepo = new Mock<ITagRepository>();
        _missionTagsRepo = new Mock<IMissionTagRepository>();

        _service = new MissionService(
            missions: _missionsRepo.Object,
            comments: _commentsRepo.Object,
            likes: _likesRepo.Object,
            categories: _categoriesRepo.Object,
            tags: _tagsRepo.Object,
            missionTags: _missionTagsRepo.Object
        );
    }
[Test]
public async Task CreateAsync_Should_AddMission_And_Save()
{
    var dto = new MissionEditDto
    {
        Title = "Test Mission",
        Description = "Desc",
        CategoryId = 1,
        TagIds = new List<int> { 2, 3 }
    };

    // Arrange: при AddAsync -> нищо, при Save необходим е Id за тагове
    _missionsRepo.Setup(r => r.AddAsync(It.IsAny<Mission>())).Returns(Task.CompletedTask);
    _missionsRepo.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
        .ReturnsAsync(1);

    // Act
    await _service.CreateAsync(dto);

    // Assert mission
    _missionsRepo.Verify(r => r.AddAsync(It.Is<Mission>(m =>
        m.Title == "Test Mission" && m.CategoryId == 1)), Times.Once);
    _missionsRepo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

    // Assert tags
    _missionTagsRepo.Verify(r => r.AddAsync(It.Is<MissionTag>(mt => mt.TagId == 2)), Times.Once);
    _missionTagsRepo.Verify(r => r.AddAsync(It.Is<MissionTag>(mt => mt.TagId == 3)), Times.Once);
    _missionTagsRepo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
}


    [Test]
    public async Task UpdateAsync_Should_NotUpdate_When_NotFound()
    {
        _missionsRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                     .ReturnsAsync((Mission?)null);

        await _service.UpdateAsync(new MissionDto { Id = 99, Title = "X" });

        _missionsRepo.Verify(r => r.Update(It.IsAny<Mission>()), Times.Never);
        _missionsRepo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task UpdateAsync_Should_Update_And_Save_When_Found()
    {
        var entity = new Mission { Id = 5, Title = "Old", MissionTags = new List<MissionTag>() };
        _missionsRepo.Setup(r => r.WithAllRelations())
            .Returns(new List<Mission> { entity }.AsQueryable());
        _missionsRepo.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        await _service.UpdateAsync(new MissionDto { Id = 5, Title = "New", CategoryId = 1 });

        _missionsRepo.Verify(r => r.Update(It.Is<Mission>(m => m.Id == 5 && m.Title == "New")), Times.Once);
        _missionsRepo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }


    [Test]
    public async Task EditAsync_Should_Update_And_Save_When_Found()
    {
        var entity = new Mission { Id = 10, Title = "Old" };
        _missionsRepo.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(entity);

        await _service.EditAsync(new MissionEditDto { Id = 10, Title = "New" });

        _missionsRepo.Verify(r => r.Update(It.Is<Mission>(m => m.Title == "New")), Times.Once);
        _missionsRepo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task SoftDeleteAsync_Should_Flag_And_Save()
    {
        var entity = new Mission { Id = 7, IsDeleted = false };
        _missionsRepo.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(entity);

        await _service.SoftDeleteAsync(7);

        _missionsRepo.Verify(r => r.Update(It.Is<Mission>(m => m.IsDeleted)), Times.Once);
        _missionsRepo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
