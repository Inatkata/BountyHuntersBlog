using BountyHuntersBlog.Models.Domain;
using BountyHuntersBlog.Models.Requests;
using BountyHuntersBlog.Repositories;
using BountyHuntersBlog.Services.Interfaces;


namespace BountyHuntersBlog.Services
{
    public class MissionService : IMissionService
    {
        private readonly IMissionPostRepository _missionRepo;

        public MissionService(IMissionPostRepository missionRepo)
        {
            _missionRepo = missionRepo;
        }

        public async Task<IEnumerable<MissionPost>> GetAllAsync()
            => await _missionRepo.GetAllAsync();

        public async Task<MissionPost?> GetByIdAsync(Guid id)
            => await _missionRepo.GetAsync(id);

        public async Task AddAsync(AddMissionPostRequest request, Guid UserId)
        {
            var mission = new MissionPost
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                UrlHandle = request.UrlHandle,
                Visible = request.Visible,
                MissionDate = request.MissionDate,
                AuthorId = UserId.ToString()
            };

            await _missionRepo.AddAsync(mission);

        }

        public async Task UpdateAsync(EditMissionPostRequest request)
        {
            var existing = await _missionRepo.GetAsync(request.Id);
            if (existing is null) return;

            existing.Title = request.Title;
            existing.ShortDescription = request.ShortDescription;
            existing.Content = request.Content;
            existing.FeaturedImageUrl = request.FeaturedImageUrl;
            existing.UrlHandle = request.UrlHandle;
            existing.MissionDate = request.MissionDate;
            existing.Visible = request.Visible;

            await _missionRepo.UpdateAsync(existing);

        }


        public async Task DeleteAsync(Guid id)
        {
            await _missionRepo.DeleteAsync(id);
        }
    }
}
