using DoorAccessApplication.Core.Models;

namespace DoorAccessApplication.Api.Models
{
    public class LockWithUsersResponse
    {
        public int Id { get; set; }
        public string UniqueIdentifier { get; set; }

        public bool IsLocked { get; set; }
        //change to userreponse
        public List<UserResponse> Users { get; set; }
    }
}
