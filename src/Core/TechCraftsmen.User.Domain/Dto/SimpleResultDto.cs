using System.Collections;

namespace TechCraftsmen.User.Core.Dto
{
    public class SimpleResultDto : BaseResultDto
    {
        public IList<string> Errors { get; } = [];

        public void SetSuccessByErrors()
        {
            Success = !Errors.Any();
        }
    }
}
