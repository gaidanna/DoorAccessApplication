namespace DoorAccessApplication.Api.Models
{
    public class UpdateLockRequest
    {
        public int Id { get; set; }
        public Guid UniqueIdentifier { get; set; }
        public string Status { get; set; }
    }
}
