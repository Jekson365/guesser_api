using guesser_api.Dto;
using guesser_api.Interfaces;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;

namespace guesser_api.Repositories
{
    public class UserRepository : IUserInterface
    {
        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }
        public async Task<User> Create(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }
        public async Task<User?> GetCurrent(string ipAddress)
        {
            User user = await _db.Users.Where(u => u.Ip == ipAddress).FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<List<User>> GetUsers()
        {
            var users = await _db.Users.ToListAsync();

            return users;
        }

        public async Task<User> UpdateScore(UpdateScoreDto updateScoreDto)
        {
            User user = await _db.Users.Where(u => u.Name == updateScoreDto.UserName).FirstOrDefaultAsync();

            user.HighScore = updateScoreDto.HighScore;

            await _db.SaveChangesAsync();

            return user;

        }
    }
}