namespace DoorAccessApplication.Core.Models
{
    public class Lock
    {
        public int Id { get; set; }
        List<User> Users { get; set; } = new List<User>();
        
    }
}
