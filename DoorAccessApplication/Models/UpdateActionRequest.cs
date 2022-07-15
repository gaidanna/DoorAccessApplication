namespace DoorAccessApplication.Api.Models
{
    public class UpdateActionRequest
    {
        public Action Action { get; set; }
        public int LockId { get; set; }
    }
}
