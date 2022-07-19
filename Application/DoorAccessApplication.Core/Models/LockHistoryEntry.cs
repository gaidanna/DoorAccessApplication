using DoorAccessApplication.Core.ValueTypes;

namespace DoorAccessApplication.Core.Models
{
    public class LockHistoryEntry
    {
        public int Id { get; set; }
        public int LockId { get; set; }
        public string UserId { get; set; }
        public DateTime DateTime { get; set; }
        public StatusType Status { get; set; }

    }
}
