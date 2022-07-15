namespace DoorAccessApplication.Api.Models
{
    public class AddUserInLockRequest
    {
        public int LockId { get; set; }
        public string Email { get; set; }
    }
}
