namespace TechCraftsmen.User.WebApi.ValueObjects
{
    public class WebApiOutput<T>(T data, string[] messages, bool success = false)
    {
        public T Data { get; } = data;
        public string[] Messages { get; } = messages;
        public DateTime Timestamp { get; } = DateTime.UtcNow;
        public bool Success { get; set; } = success;
    }
}
