namespace TechCraftsmen.User.Tests.Utils.Generators
{
    public static class CustomRandomNumberGenerator
    {
        public static int RandomWeakIntOnRange(int start, int end, int? differentFrom = null)
        {
            Random rng = new();
            
            int random = rng.Next(start, end);

            if (differentFrom is null)
            {
                return random;
            }

            while (random == differentFrom)
            {
                random = rng.Next(start, end);
            }

            return random;
        }
    }
}
