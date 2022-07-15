using DoorAccessApplication.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorAccessApplication.Infrastructure
{
    public class DoorAccessDbContext : DbContext
    {
        public DoorAccessDbContext(DbContextOptions<DoorAccessDbContext> options)
        : base(options)
        {
        }

        public DbSet<Lock> Locks { get; set; }

        public DbSet<LockHistoryEntry> LockHistoryEntries { get; set; }

        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
