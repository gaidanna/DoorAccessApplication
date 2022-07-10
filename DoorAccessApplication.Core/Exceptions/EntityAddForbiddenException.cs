namespace DoorAccessApplication.Core.Exceptions
{
    public class EntityAddForbiddenException : Exception
    {
        public EntityAddForbiddenException(string message) : base(message) { }
    }
}
