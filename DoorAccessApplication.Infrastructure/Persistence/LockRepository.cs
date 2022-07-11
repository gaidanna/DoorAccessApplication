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
        public async Task<Lock> CreateAsync(Lock lockTool)
        {
            await _dbContext.Locks.AddAsync(lockTool);
            await _dbContext.SaveChangesAsync();

            return lockTool;
        }

        public async Task DeleteAsync(Lock lockTool)
        {
            _dbContext.Locks.Remove(lockTool);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Lock> UpdateAsync(Lock lockTool)
        {
            _dbContext.Locks.Update(lockTool);
            await _dbContext.SaveChangesAsync();

            return lockTool;
        }

        public async Task<Lock> GetAsync(int id)
        {
            return await _dbContext.Locks
                //.Include(e => e.ChargeStations)
                //.ThenInclude(a => a.Connectors)
                .AsNoTracking()
                .FirstAsync(e => e.Id == id);
        }

        public async Task<List<Lock>> GetAllAsync()
        {
            return await _dbContext.Locks
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
