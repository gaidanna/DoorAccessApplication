using DoorAccessApplication.Core.Models;

namespace DoorAccessApplication.Core.Interfaces
{
    public interface ILockService
    {
        Task<Lock> AddAsync(Lock createLock, string userId);
        Task DeleteAsync(int lockId, string userId);
        Task<List<Lock>> GetAllAsync(string userId);
        Task<Lock> GetAsync(int lockId, string userId);
        Task<Lock> AddUserAsync(int lockId, string userId, string emailToAdd);
        Task<Lock> RemoveUserAsync(int lockId, string userId, string emailToAdd);
        Task<Lock> UpdateStatusAsync(int lockId, string userId, string status);
        Task<List<LockHistoryEntry>> GetHistoryAsync(int lockId, string userId);
    }
}
