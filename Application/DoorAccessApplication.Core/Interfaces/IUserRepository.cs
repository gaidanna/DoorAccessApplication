using DoorAccessApplication.Core.Models;

namespace DoorAccessApplication.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
        Task<User> GetAsync(string email);
        Task<User> GetByEmailAsync(string email);
    }
}
