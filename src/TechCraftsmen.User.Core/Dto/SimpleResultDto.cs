namespace TechCraftsmen.User.Core.Dto
{
    public class SimpleResultDto : BaseResultDto
    {
        public IList<string> Errors { get; set; } = [];

        public void SetSuccessByErrors()
        {
            Success = !Errors.Any();
        }
    }
}
