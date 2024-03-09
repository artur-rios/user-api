namespace TechCraftsmen.User.Common.Dto
{
    public class RuleResultDto
    {
        public IList<string> Errors { get; set; } = [];
        public bool Success { get; set; } = false;
    }
}
