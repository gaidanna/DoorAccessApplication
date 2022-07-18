using DoorAccessApplication.Core.Interfaces;
using DoorAccessApplication.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DoorAccessApplication.Infrastructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly DoorAccessDbContext _dbContext;
        public UserRepository(DoorAccessDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> CreateAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }
        public async Task<User> GetAsync(string userId)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(e => e.Id == userId);
        }
        public async Task<User> GetByEmailAsync(string email)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(e => e.Email == email);
        }
    }
}
