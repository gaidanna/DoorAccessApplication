using DoorAccessApplication.Core.Interfaces;
using DoorAccessApplication.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DoorAccessApplication.Infrastructure.Persistence
{
    public class LockHistoryRepository : ILockHistoryRepository
    {
        private readonly DoorAccessDbContext _dbContext;
        public LockHistoryRepository(DoorAccessDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<LockHistoryEntry>> GetHistoryAsync(int lockId, string userId)
        {
            return await _dbContext.LockHistoryEntries
                .Where( a => a.LockId == lockId && a.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
