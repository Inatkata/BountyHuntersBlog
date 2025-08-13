using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Implementations;
using BountyHuntersBlog.UnitTests.TestHelpers;
using Moq;
using NUnit.Framework.Legacy;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BountyHuntersBlog.UnitTests.TestData;

namespace BountyHuntersBlog.UnitTests.Services
{
    [TestFixture]
    public class MissionServiceTests
    {
        private Mock<IMissionRepository> _missions = null!;
        private Mock<ICommentRepository> _comments = null!;
        private Mock<ILikeRepository> _likes = null!;
        private Mock<ICategoryRepository> _categories = null!;
        private Mock<ITagRepository> _tags = null!;
        private Mock<IMissionTagRepository> _missionTags = null!;
        private MissionService _service = null!;

        [SetUp]
        public void Setup()
        {
            _missions = new Mock<IMissionRepository>();
            _comments = new Mock<ICommentRepository>();
            _likes = new Mock<ILikeRepository>();
            _categories = new Mock<ICategoryRepository>();
            _tags = new Mock<ITagRepository>();
            _missionTags = new Mock<IMissionTagRepository>();

            _service = new MissionService(
                _missions.Object, _comments.Object, _likes.Object,
                _categories.Object, _tags.Object, _missionTags.Object);
        }

        [Test]
        public async Task SearchPagedAsync_Filters_By_Category()
        {
            var data = SampleData.Missions(); // ???? helper
            _missions
                .Setup(x => x.SearchQueryable(It.IsAny<string?>(), 20, null))
                .Returns(data.Where(m => m.CategoryId == 20).AsQ());

            var (items, total) = await _service.SearchPagedAsync(null, 20, null, 1, 10);

            Assert.That(total, Is.EqualTo(1));
            Assert.That(items.Single().Title, Is.EqualTo("Bravo"));
            Assert.That(items.Single().CategoryName, Is.EqualTo("Recon"));
            CollectionAssert.Contains(items.Single().TagNames.ToList(), "HighRisk");
        }

        [Test]
        public async Task GetDetailsAsync_Returns_Comments_And_LikesCount()
        {
            var data = SampleData.Missions();
            data[1].Likes = new List<Like>
            {
                new Like { MissionId = 2, UserId = "u1" },
                new Like { MissionId = 2, UserId = "u2" }
            };

            _missions.Setup(x => x.GetByIdWithIncludesAsync(2))
                     .ReturnsAsync(data[1]);

            var dto = await _service.GetDetailsAsync(2);

            Assert.That(dto, Is.Not.Null);
            Assert.That(dto!.Title, Is.EqualTo("Bravo"));
            Assert.That(dto.LikesCount, Is.EqualTo(2));
            Assert.That(dto.Comments.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task ToggleMissionLikeAsync_Adds_When_Not_Exists()
        {
            _missions.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Mission { Id = 1 });
            _likes.Setup(x => x.All()).Returns(new List<Like>().AsQ());

            var user = TestClaims.MakeUser("u-1");
            await _service.ToggleMissionLikeAsync(1, user);

            _likes.Verify(x => x.AddAsync(It.Is<Like>(l => l.MissionId == 1 && l.UserId == "u-1")), Times.Once);
            _likes.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task AddCommentAsync_Saves_Comment_For_User()
        {
            _missions.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Mission { Id = 1 });
            _comments.Setup(x => x.AddAsync(It.IsAny<Comment>())).Returns(Task.CompletedTask);
            _comments.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var user = TestClaims.MakeUser("u-42");
            await _service.AddCommentAsync(1, "hello world", user);

            _comments.Verify(x => x.AddAsync(It.Is<Comment>(c =>
                c.MissionId == 1 && c.UserId == "u-42" && c.Content == "hello world")), Times.Once);
        }

        [Test]
        public async Task EditAsync_Syncs_Tags_Adds_And_Removes()
        {
            var mission = new Mission
            {
                Id = 1,
                Title = "A",
                Description = "D",
                CategoryId = 10,
                IsCompleted = false,
                MissionTags = new List<MissionTag> { new() { MissionId = 1, TagId = 100 } }
            };

            _missions.Setup(x => x.WithAllRelations()).Returns(new List<Mission> { mission }.AsQ());
            _missionTags.Setup(x => x.AddAsync(It.IsAny<MissionTag>())).Returns(Task.CompletedTask);
            _missions.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var dto = new MissionEditDto
            {
                Id = 1,
                Title = "Alpha",
                Description = "Desc",
                CategoryId = 10,
                IsCompleted = true,
                TagIds = new[] { 200 }
            };

            await _service.EditAsync(dto);

            _missionTags.Verify(x => x.Delete(It.Is<MissionTag>(mt => mt.TagId == 100)), Times.Once);
            _missionTags.Verify(x => x.AddAsync(It.Is<MissionTag>(mt => mt.MissionId == 1 && mt.TagId == 200)), Times.Once);
            _missions.Verify(x => x.Update(It.Is<Mission>(m => m.Id == 1 && m.IsCompleted == true)), Times.Once);
            _missions.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
