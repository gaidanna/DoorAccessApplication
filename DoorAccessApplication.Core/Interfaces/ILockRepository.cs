using DoorAccessApplication.Core.Models;

namespace DoorAccessApplication.Core.Interfaces
{
    public interface ILockRepository
    {
        Task<Lock> AddAsync(Lock createLock);
        Task<Lock> UpdateAsync(Lock updateLock);
        Task DeleteAsync(Lock deleteLock);
        Task<List<Lock>> GetAllAsync(string userId);
        Task<Lock> GetAsync(int lockId, string userId);
        Task<bool> IsExistAsync(string uniqueIdentifier);
    }
}
