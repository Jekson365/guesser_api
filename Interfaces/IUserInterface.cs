using guesser_api.Dto;
using server.Models;

namespace guesser_api.Interfaces
{
    public interface IUserInterface
    {
        public Task<User> Create(User user);
        public Task<User?> GetCurrent(string ipAddress);
        public Task<User> UpdateScore(UpdateScoreDto updateScoreDto);
    }
}