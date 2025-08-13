using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Moq;
using AutoMapper;
using BountyHuntersBlog.Services.Implementations;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Services.DTOs;

namespace BountyHuntersBlog.UnitTests.Services
{
    [TestFixture]
    public class CategoryServiceTests
    {
        private Mock<ICategoryRepository> _categories = null!;
        private IMapper _mapper = null!;
        private CategoryService _service = null!;

        [SetUp]
        public void Setup()
        {
            _categories = new Mock<ICategoryRepository>();

            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Category, CategoryDto>().ReverseMap();
            });
            _mapper = cfg.CreateMapper();

            _service = new CategoryService(_categories.Object, _mapper);
        }

        [Test]
        public async Task AllAsync_Returns_All_Categories_Projected()
        {
            var data = new List<Category>
            {
                new Category { Id = 1, Name = "Ops" },
                new Category { Id = 2, Name = "Recon" }
            };

            _categories.Setup(x => x.AllReadonly()).Returns(data.AsQueryable());

            var result = await _service.AllAsync();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(c => c.Name == "Ops"), Is.True);
        }

        [Test]
        public async Task GetAsync_Returns_Category_When_Found()
        {
            var cat = new Category { Id = 5, Name = "TestCat" };
            _categories.Setup(x => x.GetByIdAsync(5)).ReturnsAsync(cat);

            var result = await _service.GetAsync(5);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo("TestCat"));
        }

        [Test]
        public async Task CreateAsync_Adds_And_Returns_Id()
        {
            Category? saved = null;
            _categories.Setup(x => x.AddAsync(It.IsAny<Category>()))
                       .Callback<Category>(c => { c.Id = 42; saved = c; })
                       .Returns(Task.CompletedTask);
            _categories.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var id = await _service.CreateAsync(new CategoryDto { Name = "NewCat" });

            Assert.That(id, Is.EqualTo(42));
            Assert.That(saved!.Name, Is.EqualTo("NewCat"));
        }

        [Test]
        public async Task UpdateAsync_Updates_Name_When_Found()
        {
            var e = new Category { Id = 3, Name = "Old" };
            _categories.Setup(x => x.GetByIdAsync(3)).ReturnsAsync(e);
            _categories.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var ok = await _service.UpdateAsync(new CategoryDto { Id = 3, Name = "New" });

            Assert.That(ok, Is.True);
            _categories.Verify(x => x.Update(It.Is<Category>(c => c.Name == "New")), Times.Once);
            _categories.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task SoftDeleteAsync_Sets_IsDeleted_When_Found()
        {
            var e = new Category { Id = 7, Name = "Del", IsDeleted = false };
            _categories.Setup(x => x.GetByIdAsync(7)).ReturnsAsync(e);
            _categories.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var ok = await _service.SoftDeleteAsync(7);

            Assert.That(ok, Is.True);
            Assert.That(e.IsDeleted, Is.True);
            _categories.Verify(x => x.Update(It.Is<Category>(c => c.Id == 7 && c.IsDeleted)), Times.Once);
            _categories.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
