using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BountyHuntersBlog.Data.Models;
using BountyHuntersBlog.Repositories.Interfaces;
using BountyHuntersBlog.Services.DTOs;
using BountyHuntersBlog.Services.Interfaces;

namespace BountyHuntersBlog.Services.Implementations
{
    public class LikeService : ILikeService
    {
        private readonly ILikeRepository _repo;
        private readonly IMapper _mapper;

        public LikeService(ILikeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LikeDto>> GetLikesByMissionIdAsync(int missionId)
            => _mapper.Map<IEnumerable<LikeDto>>(await _repo.GetLikesByMissionIdAsync(missionId));

        public async Task<IEnumerable<LikeDto>> GetLikesByUserIdAsync(string userId)
            => _mapper.Map<IEnumerable<LikeDto>>(await _repo.GetLikesByUserIdAsync(userId));

        public async Task<int> CountLikesByMissionIdAsync(int missionId)
            => await _repo.CountLikesByMissionIdAsync(missionId);

        public async Task<int> CountLikesByUserIdAsync(string userId)
            => await _repo.CountLikesByUserIdAsync(userId);

        public async Task<bool> IsLikedByUserAsync(int missionId, string userId)
            => await _repo.ExistsAsync(missionId, userId);

    }
}
