using DoorAccessApplication.Core.Models;
using Microsoft.EntityFrameworkCore;

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
    }
}
