using DoorAccessApplication.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorAccessApplication.Core.Interfaces
{
    public interface ILockService
    {
        Task<Lock> GetAsync(int id);
        Task<List<Lock>> GetAllAsync();
        Task<Lock> CreateAsync(Lock createLock);
        Task<Lock> UpdateAsync(Lock updateLock);
        Task DeleteAsync(int id);
    }
}
