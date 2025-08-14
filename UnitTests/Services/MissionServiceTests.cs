using AutoMapper;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Services.DTOs;            // MissionDto, MissionEditDto
using BountyHuntersBlog.Services.Interfaces;      // IMissionService
using BountyHuntersBlog.Services.Implementations; // MissionService
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

[TestFixture]
public class MissionServiceTests
{
    private Mock<IRepository<Mission>> _missionsRepo = null!;
    private IMissionService _service = null!;
    private IMapper _mapper = null!;

    [SetUp]
    public void SetUp()
    {
        _missionsRepo = new Mock<IRepository<Mission>>();

        var profile = new MappingProfile(); // your AutoMapper profile
        _mapper = AutoMapperFixture.CreateMapper(profile);

        _service = new MissionService(_missionsRepo.Object, _mapper);
    }

    [Test]
    public async Task CreateAsync_Should_Add_And_Save()
    {
        var dto = new MissionEditDto
        {
            Title = "Test Mission",
            ImageUrl = "https://img",
            CategoryId = 1
        };

        await _service.CreateAsync(dto);

        _missionsRepo.Verify(r => r.AddAsync(It.IsAny<Mission>()), Times.Once);
        _missionsRepo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
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
        var entity = new Mission { Id = 5, Title = "Old" };
        _missionsRepo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(entity);

        await _service.UpdateAsync(new MissionDto { Id = 5, Title = "New" });

        _missionsRepo.Verify(r => r.Update(It.Is<Mission>(m => m.Title == "New")), Times.Once);
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
