using AutoMapper;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Implementations;
using BountyHuntersBlog.Services.Interfaces;

[TestFixture]
public class CategoryServiceTests
{
    private Mock<ICategoryRepository> _categories = null!;
    private ICategoryService _service = null!;
    private IMapper _mapper = null!;

    [SetUp]
    public void SetUp()
    {
        _categories = new Mock<ICategoryRepository>();
        _mapper = AutoMapperFixture.CreateMapper(new TestMappingProfile());
        _service = new CategoryService(_categories.Object, _mapper);
    }


    [Test]
    public async Task CreateAsync_Should_Add_And_Save()
    {
        var dto = new CategoryDto { Name = "Bounties" };

        _categories.Setup(r => r.AddAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);
        _categories.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var id = await _service.CreateAsync(dto);

        _categories.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Once);
        _categories.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.That(id, Is.GreaterThan(0));
    }

    [Test]
    public async Task UpdateAsync_Should_ReturnFalse_When_NotFound()
    {
        _categories.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Category?)null);

        var ok = await _service.UpdateAsync(new CategoryDto { Id = 99, Name = "X" });

        Assert.That(ok, Is.False);
    }

    [Test]
    public async Task UpdateAsync_Should_Update_And_Save_When_Found()
    {
        var e = new Category { Id = 5, Name = "Old" };
        _categories.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(e);

        var ok = await _service.UpdateAsync(new CategoryDto { Id = 5, Name = "New" });

        Assert.That(ok, Is.True);
        _categories.Verify(r => r.Update(It.Is<Category>(c => c.Name == "New")), Times.Once);
        _categories.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task SoftDeleteAsync_Should_Flag_And_Save()
    {
        var e = new Category { Id = 7, IsDeleted = false };
        _categories.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(e);

        var ok = await _service.SoftDeleteAsync(7);

        Assert.That(ok, Is.True);
        _categories.Verify(r => r.Update(It.Is<Category>(c => c.IsDeleted)), Times.Once);
        _categories.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
