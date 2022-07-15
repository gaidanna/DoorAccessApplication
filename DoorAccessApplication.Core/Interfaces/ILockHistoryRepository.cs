using DoorAccessApplication.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorAccessApplication.Core.Interfaces
{
    public interface ILockHistoryRepository
    {
        Task<List<LockHistoryEntry>> GetHistoryAsync(int lockId, string userId);
    }
}
