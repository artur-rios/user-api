namespace TechCraftsmen.User.Core.Dto
{
    public class BaseResultDto(bool success = false)
    {
        public bool Success { get; set; } = success;
    }
}