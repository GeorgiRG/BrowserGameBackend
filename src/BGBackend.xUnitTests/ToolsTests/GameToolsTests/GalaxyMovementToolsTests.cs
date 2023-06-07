using BrowserGameBackend.Tools.GameTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGBackend.xUnitTests.ToolsTests.GameToolsTests
{
    public class GalaxyMovementToolTests
    {
        [InlineData(5, 5, 1)]
        [InlineData(5, 5, 2)]
        [Theory]
        public void AssertGetsRadius(int X, int Y, int distance)
        {
            int[][] loc = GalaxyMovementTool.GetStarSystemsInRadius(X, Y, distance);
            int[][] badLoc = GalaxyMovementTool.GetStarSystemsInRadius(X, Y, distance + 10);

            Assert.True(loc != null);
            Assert.Equal(8, loc.Length);
            Assert.Equal(loc[0][0], X - distance); //left
            Assert.Equal(loc[2][1], Y + distance); //up
            for(int i = 0; i< badLoc.Length; i++)
            {
                Assert.True(badLoc[i] == null);

            }
        }

        [InlineData(5, 5, 1)]
        [InlineData(5, 5, 2)]
        [Theory]
        public void AssertGoesLeft(int X, int Y, int distance)
        {
            int[] loc = GalaxyMovementTool.GoLeft(X, Y, distance);
            int[] badLoc = GalaxyMovementTool.GoLeft(X, Y, distance + 10);

            Assert.True(loc != null);
            Assert.Equal(loc[0], X - distance);
            Assert.True(badLoc == null);

        }

        [InlineData(5, 5, 1)]
        [InlineData(5, 5, 2)]
        [Theory]
        public void AssertGoesRight(int X, int Y, int distance)
        {
            int[] loc = GalaxyMovementTool.GoRight(X, Y, distance);
            int[] badLoc = GalaxyMovementTool.GoRight(X, Y, distance + 10);
            Assert.True(loc != null);
            Assert.Equal(loc[0], X + distance);
            Assert.True(badLoc == null);
        }

        [InlineData(5, 5, 2)]
        [InlineData(5, 5, 1)]
        [Theory]
        public void AssertGoesUp(int X, int Y, int distance)
        {
            int[] loc = GalaxyMovementTool.GoUp(X, Y, distance);
            int[] badLoc = GalaxyMovementTool.GoUp(X, Y, distance + 10);

            Assert.True(loc != null);
            Assert.Equal(loc[1], Y + distance);
            Assert.True(badLoc == null);

        }

        [InlineData(5, 5, 1)]
        [InlineData(5, 5, 2)]
        [Theory]
        public void AssertGoesDown(int X, int Y, int distance)
        {

            int[] loc = GalaxyMovementTool.GoDown(X, Y, distance);
            int[] badLoc = GalaxyMovementTool.GoDown(X, Y, distance + 10);

            Debug.WriteLine(loc[0] + " " + loc[1]);
            Assert.True(loc != null);
            Assert.Equal(loc[1], Y - distance);
            Assert.True(badLoc == null);

        }

        [InlineData(5, 5, 1)]
        [InlineData(5, 5, 2)]
        [Theory]
        public void AssertGoesLeftUp(int X, int Y, int distance)
        {
            int[] loc = GalaxyMovementTool.GoLeftUp(X, Y, distance);
            int[] badLoc = GalaxyMovementTool.GoLeftUp(X, Y, distance + 10);

            Assert.True(loc != null);
            Assert.Equal(loc[0], X - distance);
            Assert.Equal(loc[1], Y + distance);
            Assert.True(badLoc == null);

        }

        [InlineData(5, 5, 1)]
        [InlineData(5, 5, 2)]
        [Theory]
        public void AssertGoesLeftDown(int X, int Y, int distance)
        {
            int[] loc = GalaxyMovementTool.GoLeftDown(X, Y, distance);
            int[] badLoc = GalaxyMovementTool.GoLeftDown(X, Y, distance + 10);

            Assert.True(loc != null);
            Assert.Equal(loc[0], X - distance);
            Assert.Equal(loc[1], Y - distance);
            Assert.True(badLoc == null);

        }

        [InlineData(5, 5, 1)]
        [InlineData(5, 5, 2)]
        [Theory]
        public void AssertGoesRightUp(int X, int Y, int distance)
        {
            int[] loc = GalaxyMovementTool.GoRightUp(X, Y, distance);
            int[] badLoc = GalaxyMovementTool.GoRightUp(X, Y, distance + 10);

            Assert.True(loc != null);
            Assert.Equal(loc[0], X + distance);
            Assert.Equal(loc[1], Y + distance);
            Assert.True(badLoc == null);

        }

        [InlineData(5, 5, 1)]
        [InlineData(5, 5, 2)]
        [Theory]
        public void AssertGoesRightDown(int X, int Y, int distance)
        {
            int[] loc = GalaxyMovementTool.GoRightDown(X, Y, distance);
            int[] badLoc = GalaxyMovementTool.GoRightDown(X, Y, distance + 10);

            Assert.True(loc != null);
            Assert.Equal(loc[0], X + distance);
            Assert.Equal(loc[1], Y - distance);
            Assert.True(badLoc == null);

        }
    }
}
