namespace TechCraftsmen.User.Core.Dto
{
    public class DataResultDto<T>(T data, string message, bool success = false) : BaseResultDto(success)
    {
        public T Data { get; set; } = data;
        public string Message { get; set; } = message;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
