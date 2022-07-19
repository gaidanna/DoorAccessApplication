using DoorAccessApplication.Core.Models;

namespace DoorAccessApplication.Core.Interfaces
{
    public interface ILockHistoryRepository
    {
        Task<List<LockHistoryEntry>> GetHistoryAsync(int lockId, string userId);
    }
}
