namespace TechCraftsmen.User.Core.Exceptions
{
    public class NotAllowedException : Exception
    {
        public NotAllowedException(string? message) : base(message)
        {
        }
    }
}
