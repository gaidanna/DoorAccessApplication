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

        public async Task<Lock> CreateAsync(Lock createLock, string userId)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new EntityAddForbiddenException("User does not exist.");
            }

            if(await _lockRepository.IsExistAsync(createLock.UniqueIdentifier))
            {
                throw new EntityAddForbiddenException("Lock already registered.");
            }

            createLock.Users.Add(user);
            var historyEntry = new LockHistoryEntry()
            {
                DateTime = DateTime.UtcNow,
                UserId = userId,
                Status = StatusType.add
            };

            createLock.HistoryEntries.Add(historyEntry);
            try
            {
                return await _lockRepository.CreateAsync(createLock);
            }
            catch (DbUpdateException)
            {
                throw new EntityAddForbiddenException
                    ("Lock cannot be added to database.");
            }
        }

        public async Task DeleteAsync(int lockId, string userId)
        {
            var deleteLock = await _lockRepository.GetAsync(lockId, userId);
            if (deleteLock == null)
            {
                throw new EntityDeleteForbiddenException("Lock does not exist.");
            }

            var historyEntry = new LockHistoryEntry()
            {
                DateTime = DateTime.UtcNow,
                UserId = userId,
                Status = StatusType.remove
            };

            deleteLock.HistoryEntries.Add(historyEntry);

            await _lockRepository.DeleteAsync(deleteLock);
        }

        public async Task<List<Lock>> GetAllAsync(string userId)
        {
            return await _lockRepository.GetAllAsync(userId);
        }

        public async Task<Lock> AddUserAsync(int lockId, string userId, string emailToAdd)
        {
            var user = await _userRepository.GetByEmailAsync(emailToAdd);
            if (user == null)
            {
                throw new EntityAddForbiddenException("User does not exist.");
            }
            var lockTool = await _lockRepository.GetAsync(lockId, userId);
            if (lockTool == null)
            {
                throw new EntityAddForbiddenException("Lock does not exist.");
            }

            lockTool.Users.Add(user);

            return await _lockRepository.UpdateAsync(lockTool);
        }

        public async Task<Lock> RemoveUserAsync(int lockId, string userId, string emailToAdd)
        {
            var user = await _userRepository.GetByEmailAsync(emailToAdd);
            if (user == null)
            {
                throw new EntityAddForbiddenException("User does not exist.");
            }
            var lockTool = await _lockRepository.GetAsync(lockId, userId);
            if (lockTool == null)
            {
                throw new EntityAddForbiddenException("Lock does not exist.");
            }

            lockTool.Users.Remove(user);

            return await _lockRepository.UpdateAsync(lockTool);
        }


        public async Task<Lock> GetAsync(int lockId, string userId)
        {
            return await _lockRepository.GetAsync(lockId, userId);
        }

        public async Task<Lock> UpdateStatusAsync(int lockId, string userId, string status)
        {
            var newStatus = GetStatus(status);
            
            var lockTool = await _lockRepository.GetAsync(lockId, userId);
            
            if (lockTool == null)
            {
                throw new EntityAddForbiddenException("Lock does not exist.");
            }

            if (newStatus == lockTool.Status)
            {
                throw new EntityUpdateForbiddenException("Cannot set the same status."); 
            }
            lockTool.Status = newStatus;

            lockTool.HistoryEntries.Add(new LockHistoryEntry()
            {
                UserId = userId,
                LockId = lockId,
                DateTime = DateTime.Now,
                Status = newStatus
            });

            return await _lockRepository.UpdateAsync(lockTool);
        }

        public async Task<List<LockHistoryEntry>> GetHistoryAsync(int lockId, string userId)
        {
            return await _lockHistoryRepository.GetHistoryAsync(lockId, userId);
        }

        private StatusType GetStatus(string status)
        {
            if (!Enum.TryParse(status.ToLower(), out StatusType myStatus))
            {
                throw new EntityUpdateForbiddenException("This status does not exist.");
            }

            if (myStatus.ToString() != "open" && myStatus.ToString() != "close")
            {
                throw new EntityUpdateForbiddenException("Cannot update to this status. Open/close status update is allowed.");
            }
            return myStatus;
        }
    }
}
