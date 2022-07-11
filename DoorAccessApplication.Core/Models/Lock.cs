namespace DoorAccessApplication.Core.Models
{
    public class Lock
    {
        public int Id { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }
}
