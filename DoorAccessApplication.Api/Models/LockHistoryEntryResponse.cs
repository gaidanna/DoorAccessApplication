namespace DoorAccessApplication.Api.Models
{
    public class LockHistoryEntryResponse
    {
        public int Id { get; set; }
        public int LockId { get; set; }
        public string UserId { get; set; }
        public DateTime DateTime { get; set; }
        public string Status { get; set; }
    }
}
