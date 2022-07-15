﻿namespace DoorAccessApplication.Core.Models
{
    public class Lock
    {
        public int Id { get; set; }
        public string UniqueIdentifier { get; set; }

        public bool IsLocked { get; set; }
        public List<User> Users { get; set; } = new List<User>();
        public List<LockHistoryEntry> HistoryEntries { get; set; } = new List<LockHistoryEntry>();
    }
}
