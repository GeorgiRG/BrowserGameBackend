using BrowserGameBackend.Tools.GameTools;
using BrowserGameBackend.Types;

namespace BGBackend.xUnitTests.ToolsTests.GameToolsTests
{
    public class GalaxyMovementToolTests
    {
        [InlineData(5, 5, 1, 10)]
        [InlineData(5, 5, 2, 10)]
        [Theory]
        public void AssertGetsRadius(int X, int Y, int distance, int galaxyWidth)
        {
            List<Coordinates> loc = GalaxyMovementTool.GetStarSystemsInRadius(X, Y, distance, galaxyWidth);
            List<Coordinates> badLoc = GalaxyMovementTool.GetStarSystemsInRadius(X, Y, distance + 98, galaxyWidth);

            Assert.True(loc != null);
            Assert.Equal(distance * 2 * 4 + 2, loc.Count); //perimeter of square with 2 corners
            for(int i = 0; i< badLoc.Count; i++)
            {
                Assert.True(badLoc[i] == null);

            }
        }

        [InlineData(5, 5, 1, 10)]
        [InlineData(5, 5, 2, 10)]
        [Theory]
        public void AssertGoesLeft(int X, int Y, int distance, int galaxyWidth)
        {
            Coordinates loc = GalaxyMovementTool.GoLeft(X, Y, distance, galaxyWidth);
            Coordinates badLoc = GalaxyMovementTool.GoLeft(X, Y, distance + 10, galaxyWidth);

            Assert.True(loc != null);
            Assert.Equal(loc.X, X - distance);
            Assert.True(badLoc == null);
        }

        [InlineData(5, 5, 1, 10)]
        [InlineData(5, 5, 2, 10)]
        [Theory]
        public void AssertGoesRight(int X, int Y, int distance, int galaxyWidth)
        {
            Coordinates loc = GalaxyMovementTool.GoRight(X, Y, distance, galaxyWidth);
            Coordinates badLoc = GalaxyMovementTool.GoRight(X, Y, distance + 99, galaxyWidth);

            Assert.True(loc != null);
            Assert.Equal(loc.X, X + distance);
            Assert.True(badLoc == null);
        }

        [InlineData(5, 5, 2, 10)]
        [InlineData(5, 5, 1, 10)]
        [Theory]
        public void AssertGoesUp(int X, int Y, int distance, int galaxyWidth)
        {
            Coordinates loc = GalaxyMovementTool.GoUp(X, Y, distance, galaxyWidth);
            Coordinates badLoc = GalaxyMovementTool.GoUp(X, Y, distance + 99, galaxyWidth);

            Assert.True(loc != null);
            Assert.Equal(loc.Y, Y + distance);
            Assert.True(badLoc == null);
        }

        [InlineData(5, 5, 1, 10)]
        [InlineData(5, 5, 2, 10)]
        [Theory]
        public void AssertGoesDown(int X, int Y, int distance, int galaxyWidth)
        {
            Coordinates loc = GalaxyMovementTool.GoDown(X, Y, distance, galaxyWidth);
            Coordinates badLoc = GalaxyMovementTool.GoDown(X, Y, distance + 10, galaxyWidth);

            Assert.True(loc != null);
            Assert.Equal(loc.Y, Y - distance);
            Assert.True(badLoc == null);
        }    
    }
}
