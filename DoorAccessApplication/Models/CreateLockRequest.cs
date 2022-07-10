namespace DoorAccessApplication.Api.Models
{
    public class CreateLockRequest
    {
        public Guid UniqueIdentifier { get; set; }
        public string Type { get; set; }
    }
}
