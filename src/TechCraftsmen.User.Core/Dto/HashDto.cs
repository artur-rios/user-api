namespace TechCraftsmen.User.Core.Dto
{
    public class HashDto
    {
        public byte[] Hash { get; init; } = [];
        public byte[] Salt { get; set; } = [];
    }
}
