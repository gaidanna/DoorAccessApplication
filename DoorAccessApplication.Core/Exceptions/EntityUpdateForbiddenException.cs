namespace DoorAccessApplication.Core.Exceptions
{
    public class EntityUpdateForbiddenException : Exception
    {
        public EntityUpdateForbiddenException(string message) : base(message) { }
    }
}
