using DoorAccessApplication.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorAccessApplication.Infrastructure
{
    public class LockDbContext : DbContext
    {
        public LockDbContext(DbContextOptions<LockDbContext> options)
        : base(options)
        {
        }

        public DbSet<Lock> Locks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
