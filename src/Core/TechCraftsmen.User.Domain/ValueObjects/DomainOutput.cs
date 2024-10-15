namespace TechCraftsmen.User.Core.ValueObjects
{
    public class DomainOutput(IList<string>? errors = null)
    {
        public IList<string> Errors { get; set; } = errors ?? [];
        public bool Success { get => Errors.Any(); }
    }
}
