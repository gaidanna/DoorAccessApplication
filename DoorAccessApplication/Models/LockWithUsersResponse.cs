namespace DoorAccessApplication.Api.Models
{
    public class LockWithUsersResponse : LockResponse
    {
        public List<UserResponse> Users { get; set; }
    }
}
