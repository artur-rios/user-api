namespace TechCraftsmen.User.Core.Exceptions
{
    public class EntityNotChangedException : Exception
    {
        public EntityNotChangedException(string? message) : base(message)
        {
        }
    }
}
