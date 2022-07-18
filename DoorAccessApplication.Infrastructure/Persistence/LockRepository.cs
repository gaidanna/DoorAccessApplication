using DoorAccessApplication.Core.Interfaces;
using DoorAccessApplication.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DoorAccessApplication.Infrastructure.Persistence
{
    public class LockRepository : ILockRepository
    {

        private readonly DoorAccessDbContext _dbContext;
        public LockRepository(DoorAccessDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Lock> AddAsync(Lock createLock)
        {
            await _dbContext.Locks.AddAsync(createLock);
            await _dbContext.SaveChangesAsync();

            return createLock;
        }

        public async Task<Lock> UpdateAsync(Lock lockTool)
        {
            _dbContext.Locks.Update(lockTool);
            await _dbContext.SaveChangesAsync();

            return lockTool;
        }          

        public async Task DeleteAsync(Lock deleteLock)
        {
            _dbContext.Locks.Remove(deleteLock);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Lock>> GetAllAsync(string userId)
        {
            var locks = await _dbContext.Locks
                .Where(e => e.Users.Any(l => l.Id == userId))
                .Include(e => e.Users)
                .AsNoTracking()
                .ToListAsync();

            return locks;
        }

        public async Task<Lock> GetAsync(int lockId, string userId)
        {
            return await _dbContext.Locks
                .Include(e => e.Users)
                .FirstOrDefaultAsync(e => e.Id == lockId && e.Users.Any(l => l.Id == userId));
        }

        public async Task<bool> IsExistAsync(string uniqueIdentifier)
        {
            var result = await _dbContext.Locks
                .FirstOrDefaultAsync(e => e.UniqueIdentifier == uniqueIdentifier);
            
            return result != null;
        }
    }
}
