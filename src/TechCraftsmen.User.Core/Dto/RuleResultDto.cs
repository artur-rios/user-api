namespace TechCraftsmen.User.Core.Dto
{
    public class RuleResultDto
    {
        public IList<string> Errors { get; set; } = [];
        public bool Success { get; set; } = false;
    }
}
