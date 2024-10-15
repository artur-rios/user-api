namespace TechCraftsmen.User.Core.ValueObjects
{
    public class HashOutput
    {
        public byte[] Hash { get; init; } = [];
        public byte[] Salt { get; set; } = [];
    }
}
