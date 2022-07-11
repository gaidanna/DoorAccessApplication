using DoorAccessApplication.Core.Exceptions;
using DoorAccessApplication.Core.Interfaces;
using DoorAccessApplication.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorAccessApplication.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> CreateAsync(User user)
        {
            try
            {
                return await _userRepository.CreateAsync(user);
            }
            catch (DbUpdateException)
            {
                throw new EntityAddForbiddenException
                    ($"User cannot be added.");
            }
        }
    }
}
