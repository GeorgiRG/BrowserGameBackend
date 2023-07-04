using BrowserGameBackend.Tools.GameTools;
using System.Diagnostics;

namespace BGBackend.xUnitTests.ToolsTests.GameToolsTests
{
    public class GeneratorTests
    {
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(15)]
        [InlineData(25)]
        [InlineData(35)]
        [InlineData(50)]
        [Theory]
        public void AssertResourcesAreReturned(int size)
        {
            int[] res = GalaxyGenerationTools.CalculateAvailableResources(size);
            Assert.Equal(4, res.Length);
            Assert.True(res.Sum() <= size * 1.5 + 4);
            Assert.True(res.Sum() >= 4);

        }

    }
}
