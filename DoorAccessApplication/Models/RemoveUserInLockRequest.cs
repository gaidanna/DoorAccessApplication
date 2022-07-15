namespace DoorAccessApplication.Api.Models
{
    public class RemoveUserInLockRequest
    {
        public int LockId { get; set; }
        public string Email { get; set; }
    }
}
