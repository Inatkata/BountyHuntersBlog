using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Models.Requests;
using BountyHuntersBlog.Repositories;
using BountyHuntersBlog.Services;
using Moq;
using Xunit;

namespace BountyHuntersBlog.Tests.Services
{
    public class MissionServiceTests
    {
        [Fact]
        public async Task AddAsync_ShouldCallRepository_WithCorrectData()
        {
            // Arrange
            var mockRepo = new Mock<IMissionPostRepository>();
            var service = new MissionService(mockRepo.Object);
            var request = new AddMissionPostRequest
            {
                Title = "Test Mission",
                ShortDescription = "Short",
                Content = "Content",
                FeaturedImageUrl = "img.jpg",
                UrlHandle = "test-mission",
                Visible = true,
                MissionDate = DateTime.Now
            };
            var authorId = Guid.NewGuid();

            // Act
            await service.AddAsync(request, authorId);

            // Assert
            mockRepo.Verify(repo => repo.AddAsync(It.Is<MissionPost>(
                m => m.Title == request.Title &&
                     m.AuthorId == authorId.ToString() &&
                     m.UrlHandle == request.UrlHandle
            ), It.IsAny<List<Guid>>()), Times.Once);

        }
    }
}