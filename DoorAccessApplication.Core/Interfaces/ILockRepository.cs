using DoorAccessApplication.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorAccessApplication.Core.Interfaces
{
    public interface ILockRepository
    {
        Task<Lock> CreateAsync(Lock lockTool);
        Task<Lock> UpdateAsync(Lock lockTool);
        Task DeleteAsync(Lock lockTool);
        Task<Lock> GetAsync(int id);
        Task<List<Lock>> GetAllAsync();
    }
}
