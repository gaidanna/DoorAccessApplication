using DoorAccessApplication.Core.Exceptions;
using DoorAccessApplication.Core.Interfaces;
using DoorAccessApplication.Core.Models;
using DoorAccessApplication.Core.ValueTypes;
using Microsoft.EntityFrameworkCore;

namespace DoorAccessApplication.Core.Services
{
    public class LockService : ILockService

    {
        private readonly ILockRepository _lockRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILockHistoryRepository _lockHistoryRepository;

        public LockService(ILockRepository lockRepository, IUserRepository userRepository, ILockHistoryRepository lockHistoryRepository)
        {
            _lockRepository = lockRepository;
            _userRepository = userRepository;
            _lockHistoryRepository = lockHistoryRepository;
        }

        public async Task<Lock> AddAsync(Lock createLock, string userId)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null)
                throw new Exception();

            createLock.Users.Add(user);
            try
            {
                return await _lockRepository.AddAsync(createLock);
            }
            catch (DbUpdateException)
            {
                throw new EntityAddForbiddenException
                    ("Lock cannot be added.");
            }
        }

        public async Task DeleteAsync(int lockId, string userId)
        {
            var deleteLock = await _lockRepository.GetAsync(lockId, userId);

            await _lockRepository.DeleteAsync(deleteLock);
        }

        public async Task<List<Lock>> GetAllAsync(string userId)
        {
            return await _lockRepository.GetAllAsync(userId);
        }

        public async Task<Lock> AddUserAsync(int lockId, string userId, string emailToAdd)
        {
            var user = await _userRepository.GetAsync(emailToAdd);
            var lockTool = await _lockRepository.GetAsync(lockId, userId);

            lockTool.Users.Add(user);

            return await _lockRepository.UpdateAsync(lockTool);
        }

        public async Task<Lock> RemoveUserAsync(int lockId, string userId, string emailToAdd)
        {
            var user = await _userRepository.GetAsync(emailToAdd);
            var lockTool = await _lockRepository.GetAsync(lockId, userId);

            lockTool.Users.Add(user);

            return await _lockRepository.UpdateAsync(lockTool);
        }


        public async Task<Lock> GetAsync(int lockId, string userId)
        {
            return await _lockRepository.GetAsync(lockId, userId);
        }

        public async Task<Lock> UpdateStatusAsync(int lockId, string userId)
        {
            var lockTool = await _lockRepository.GetAsync(lockId, userId);
            lockTool.IsLocked = lockTool.IsLocked != true;

            lockTool.HistoryEntries.Add(new LockHistoryEntry()
            {
                UserId = userId,
                LockId = lockId,
                DateTime = DateTime.Now,
                Status = lockTool.IsLocked ? StatusType.Close : StatusType.Open
            });

            return await _lockRepository.UpdateAsync(lockTool);
        }

        public async Task<List<LockHistoryEntry>> GetHistoryAsync(int lockId, string userId)
        {
            return await _lockHistoryRepository.GetHistoryAsync(lockId, userId);
        }
    }
}
