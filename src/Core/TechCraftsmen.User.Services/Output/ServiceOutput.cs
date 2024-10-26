using TechCraftsmen.User.Services.Enums;

namespace TechCraftsmen.User.Services.Output
{
    public class ServiceOutput<T>(T? data, string[] messages, Results result = Results.Success)
    {
        public T? Data { get; } = data;
        public string[] Messages { get; } = messages;
        public Results Result { get; } = result;
    }
}
