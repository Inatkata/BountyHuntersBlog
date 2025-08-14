
using AutoMapper;
using Moq;
using NUnit.Framework;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Implementations;
using BountyHuntersBlog.Repositories.Interfaces; // adjust
using BountyHuntersBlog.Services.Implementations;      // adjust
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Implementations;
using BountyHuntersBlog.Services.Interfaces; // adjust

[TestFixture]
public class CategoryServiceTests
{
    private Mock<IRepository<Category>> _repo = null!;
    private ICategoryService _service = null!;
    private IMapper _mapper = null!;

    [SetUp]
    public void SetUp()
    {
        _repo = new Mock<IRepository<Category>>();
        var profile = new MappingProfile(); // adjust ns
        _mapper = AutoMapperFixture.CreateMapper(profile);
        _service = new CategoryService(_repo.Object, _mapper); // adjust ctor
    }

    [Test]
    public async Task CreateAsync_Should_Add_And_Save()
    {
        var dto = new CategoryDto { Name = "Bounties" };

        var id = await _service.CreateAsync(dto);

        _repo.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Once);
        _repo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.That(id, Is.GreaterThan(0));
    }

    [Test]
    public async Task UpdateAsync_Should_ReturnFalse_When_NotFound()
    {
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Category?)null);

        var ok = await _service.UpdateAsync(new CategoryDto { Id = 99, Name = "X" });

        Assert.That(ok, Is.False);
    }
}