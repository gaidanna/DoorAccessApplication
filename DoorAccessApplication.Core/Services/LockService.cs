using DoorAccessApplication.Core.Exceptions;
using DoorAccessApplication.Core.Interfaces;
using DoorAccessApplication.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DoorAccessApplication.Core.Services
{
    public class LockService : ILockService
    {
        private readonly ILockRepository _lockRepository;
        public LockService(ILockRepository lockRepository)
        {
            _lockRepository = lockRepository;
        }
        public async Task<Lock> CreateAsync(Lock createLock)
        {
            try
            {
                return await _lockRepository.CreateAsync(createLock);
            }
            catch (DbUpdateException)
            {
                throw new EntityAddForbiddenException
                    ($"Connector cannot be added.");
            }
        }

        public async Task DeleteAsync(int id)
        {
            var lockTool = await _lockRepository.GetAsync(id);
            await _lockRepository.DeleteAsync(lockTool);
        }

        public async Task<Lock> GetAsync(int id)
        {
            return await _lockRepository.GetAsync(id);
        }

        public async Task<List<Lock>> GetAllAsync()
        {
            return await _lockRepository.GetAllAsync();
        }

        public async Task<Lock> UpdateAsync(Lock updateLock)
        {
            try
            {
                return await _lockRepository.UpdateAsync(updateLock);
            }
            catch (DbUpdateException)
            {
                throw new EntityUpdateForbiddenException
                    ($"Group with id {updateLock.Id} cannot be updated.");
            }
        }
    }
}
