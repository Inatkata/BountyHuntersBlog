using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using NUnit.Framework;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Implementations;

namespace BountyHuntersBlog.UnitTests.Services
{
    [TestFixture]
    public class CommentServiceTests
    {
        private Mock<ICommentRepository> _comments = null!;
        private CommentService _service = null!;
        private IMapper _mapper = null!;

        [SetUp]
        public void Setup()
        {
            _comments = new Mock<ICommentRepository>();
            _mapper = AutoMapperFixture.CreateMapper(new TestMappingProfile());

            // Ако твоят CommentService приема и други зависимости – добави ги тук.
            _service = new CommentService(_comments.Object, _mapper);
        }

        [Test]
        public async Task GetByIdAsync_Returns_Comment_When_Found()
        {
            var com = new Comment { Id = 10, Content = "Test content" };
            _comments.Setup(x => x.GetByIdAsync(10)).ReturnsAsync(com);

            var result = await _service.GetByIdAsync(10);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Content, Is.EqualTo("Test content"));
        }

        [Test]
        public async Task AddAsync_Adds_Comment_And_Saves()
        {
            _comments.Setup(x => x.AddAsync(It.IsAny<Comment>())).Returns(Task.CompletedTask);
            _comments.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            await _service.AddAsync(1, "u1", "Hello");

            _comments.Verify(x => x.AddAsync(It.Is<Comment>(c =>
                c.MissionId == 1 && c.UserId == "u1" && c.Content == "Hello")), Times.Once);

            _comments.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_Removes_Comment()
        {
            var com = new Comment { Id = 5, Content = "Delete me" };
            _comments.Setup(x => x.GetByIdAsync(5)).ReturnsAsync(com);
            _comments.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            await _service.DeleteAsync(5);

            _comments.Verify(x => x.Delete(com), Times.Once);
            _comments.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetAllByMissionIdAsync_Returns_Filtered()
        {
            var data = new List<Comment>
            {
                new Comment { Id = 1, MissionId = 100, Content = "A" },
                new Comment { Id = 2, MissionId = 200, Content = "B" },
                new Comment { Id = 3, MissionId = 100, Content = "C" },
            };

            _comments.Setup(x => x.All()).Returns(data.AsQueryable());

            var result = await _service.GetForMissionAsync(100);

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.All(c => c.MissionId == 100), Is.True);
        }
    }
}
