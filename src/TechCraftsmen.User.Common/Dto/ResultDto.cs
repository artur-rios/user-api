namespace TechCraftsmen.User.Common.Dto
{
    public class ResultDto<T>(T? _data = default, string? _message = null, bool _success = false)
    {
        public T? Data { get; set; } = _data;
        public string? Message { get; set; } = _message;
        public bool Success { get; set; } = _success;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
