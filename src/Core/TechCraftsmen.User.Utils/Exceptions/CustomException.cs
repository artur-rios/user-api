namespace TechCraftsmen.User.Utils.Exceptions
{
    public class CustomException(string[] messages, string message = "Internal error") : Exception(message)
    {
        public string[] Messages { get; } = messages;
    }
}
