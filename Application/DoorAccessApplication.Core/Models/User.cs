namespace DoorAccessApplication.Core.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<Lock> Locks { get; set; } = new List<Lock>();
        public List<LockHistoryEntry> HistoryEntries { get; set; } = new List<LockHistoryEntry>();
    }
}
