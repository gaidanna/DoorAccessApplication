using DoorAccessApplication.Core.Interfaces;
using DoorAccessApplication.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return await _dbContext.Locks
                .Include(e => e.Users.Where(e => e.Id == userId))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Lock> GetAsync(int lockId, string userId)
        {
            return await _dbContext.Locks
                .Include(e => e.Users.Where(e => e.Id == userId))
                .AsNoTracking()
                .FirstAsync(e => e.Id == lockId);
        }
    }
}
