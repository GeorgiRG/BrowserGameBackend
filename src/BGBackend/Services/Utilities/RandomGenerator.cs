namespace BrowserGameBackend.Services.Utilities
{
    public interface IRandomGenerator
    {
        public int IntInRange(int min, int max);
        public bool ResultOfPercentageChance(float percentage);
    }
    public class RandomGenerator : IRandomGenerator
    {
        private readonly Random _random;

        public RandomGenerator()
        {
            _random = new Random();
        }

        public int IntInRange(int min, int max)
        {
            return _random.Next(min, max);
        }

        public bool ResultOfPercentageChance(float percentage)
        {
            int randomPercent = _random.Next(0, 100);
            return percentage > randomPercent;
        }
    }
}
