using BrowserGameBackend.Models;
using System.Numerics;

namespace BrowserGameBackend.Tools.GameTools
{
    /// <summary>
    ///Used to find things in a range, mostly will be used for bots.
    ///<br>Returns Star System ID, for Array pos it has to be minus 1</br>
    /// </summary>
    public static class GalaxyMovementTool
    {
        public const int galaxySize = GeneratorTool.galaxySize;
        public const int galaxyWidth = GeneratorTool.galaxyWidth;
        public const int capitalRadius = galaxyWidth / 3;

        public static int[] GetRandomLocationFromRadius(ref int[][] locations)
        {
            Random random = new();
            int[]? location = null;
            while(location == null)
            {
                int pos = random.Next(8);
                location = locations[pos];
            }
            return location;
        }
        public static int[][] GetStarSystemsInRadius(int X, int Y, int radius)
        {
            if(X < 0 || Y < 0 || X > galaxyWidth || Y > galaxyWidth)
            {
                throw new ArgumentException("Coordinate was out of grid bounds");
            }
            int[][] allLocations = new int[8][];
            allLocations[0] = GoLeft(X, Y, radius)!;
            allLocations[1] = GoRight(X, Y, radius)!;
            allLocations[2] = GoUp(X, Y, radius)!;
            allLocations[3] = GoDown(X, Y, radius)!;
            allLocations[4] = GoLeftDown(X, Y, radius)!;
            allLocations[5] = GoRightDown(X, Y, radius)!;
            allLocations[6] = GoLeftUp(X, Y, radius)!;
            allLocations[7] = GoRightUp(X, Y, radius)!;

            return allLocations;
        }
        public static int[]? GoLeft(int X, int Y, int distance)
        {

            if (X - distance > 0)
            {
                return new int[] { X - distance, Y };
            }
            else return null!;
        } 
        public static int[]? GoRight(int X, int Y, int distance)
        {
            if (X + distance < galaxyWidth)
            {
                return new int[] { X + distance, Y };
            }
            else return null!;
        }
        public static int[]? GoUp(int X, int Y, int distance)
        {
            if (Y + distance < galaxyWidth)
            {
                return new int[] { X , Y + distance };
            }
            else return null!;
        }
        public static int[]? GoDown(int X, int Y, int distance)
        {
            if (Y - distance >= 0)
            {
                return new int[] { X , Y - distance };
            }
            else return null!;
        }
        //Diagonals
        public static int[]? GoLeftUp (int X, int Y, int distance)
        {
            int[]? LocForX = GoLeft(X, Y, distance);
            int[]? LocForY = GoUp(X, Y, distance);
            if (LocForX != null && LocForY != null)
            {
                return new int[] { LocForX[0], LocForY[1] };
            }
            else return null!;
        }
        
        public static int[]? GoLeftDown (int X, int Y, int distance)
        {
            int[]? LocForX = GoLeft(X, Y, distance);
            int[]? LocForY = GoDown(X, Y, distance);
            if (LocForX != null && LocForY != null)
            {
                return new int[] { LocForX[0], LocForY[1] };
            }
            else return null!;
        }

        public static int[]? GoRightUp (int X, int Y, int distance)
        {
            int[]? LocForX = GoRight(X, Y, distance);
            int[]? LocForY = GoUp(X, Y, distance);
            if (LocForX != null && LocForY != null)
            {
                return new int[] { LocForX[0], LocForY[1] };
            }
            else return null!;
        }

        public static int[]? GoRightDown (int X, int Y, int distance)
        {
            int[]? LocForX = GoRight(X, Y, distance);
            int[]? LocForY = GoDown(X, Y, distance);
            if (LocForX != null && LocForY != null)
            {
                return new int[] { LocForX[0], LocForY[1] };
            }
            else return null!;
        }
    }
}
