namespace DoorAccessApplication.Api.Models
{
    public class LockWithHistoryResponse : LockResponse
    {
        public List<LockHistoryEntryResponse> HistoryEntries { get; set; }
    }
}
