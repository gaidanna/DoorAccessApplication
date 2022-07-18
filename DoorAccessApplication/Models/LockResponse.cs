namespace DoorAccessApplication.Api.Models
{
    public class LockResponse
    {
        public int Id { get; set; }
        public string UniqueIdentifier { get; set; }

        public string Status { get; set; }
    }
}
