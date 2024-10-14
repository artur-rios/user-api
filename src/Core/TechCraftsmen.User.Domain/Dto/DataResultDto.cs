namespace TechCraftsmen.User.Core.Dto
{
    public class DataResultDto<T>(T data, string[] messages, bool success = false) : BaseResultDto(success)
    {
        public T Data { get; } = data;
        public string[] Messages { get; } = messages;
        public DateTime Timestamp { get; } = DateTime.UtcNow;
    }
}
