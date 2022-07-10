namespace DoorAccessApplication.Core.Exceptions
{
    public class EntityDeleteForbiddenException : Exception
    {
        public EntityDeleteForbiddenException(string message) : base(message) { }
    }
}
