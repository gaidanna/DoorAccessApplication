﻿using DoorAccessApplication.Core.Interfaces;
using DoorAccessApplication.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorAccessApplication.Infrastructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly DoorAccessDbContext _dbContext;
        public UserRepository(DoorAccessDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> CreateAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }
        public async Task<User> GetAsync(string email)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstAsync(e => e.Email == email);
        }
    }
}