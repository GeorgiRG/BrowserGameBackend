using BrowserGameBackend.Models;
using BrowserGameBackend.Types;
using System.Numerics;

namespace BrowserGameBackend.Tools.GameTools
{
    /// <summary>
    ///Used to find things in a range, mostly will be used for bots.
    ///<br>Returns Star System ID, for Array pos it has to be minus 1</br>
    /// </summary>
    public static class GalaxyMovementTool
    {
        public static List<Coordinates> GetStarSystemsInRadius(int centerX, int centerY, int radius, int galaxyWidth)
        {
            if (centerX < 0 || centerY < 0 || centerX > galaxyWidth || centerY > galaxyWidth || radius > galaxyWidth)
            {
                throw new ArgumentException("Coordinate was out of grid bounds");
            }
            //squareSide + 1 will give actual length of the side but corners will repeat
            int squareSide = radius * 2;
            Coordinates bottomLeftCorner = new (){ X = centerX - radius, Y = centerY - radius };
            Coordinates topRightCorner = new(){ X = centerX + radius, Y = centerY + radius };
            List<Coordinates> allLocations = new ();
            //Add locations starting from the 2 corners, 
            for(int i = 1; i <= squareSide; i++)
            {
                allLocations.Add(GoRight(bottomLeftCorner.X, bottomLeftCorner.Y, i, galaxyWidth)!);
                allLocations.Add(GoUp(bottomLeftCorner.X, bottomLeftCorner.Y, i, galaxyWidth)!);
                allLocations.Add(GoLeft(topRightCorner.X, topRightCorner.Y, i, galaxyWidth)!);
                allLocations.Add(GoDown(topRightCorner.X, topRightCorner.Y, i, galaxyWidth)!);
            }
            //adding corners as they were skipped, using movement to return null if out of bounds
            allLocations.Add(GoLeft(topRightCorner.X, topRightCorner.Y, 0, galaxyWidth)!);
            allLocations.Add(GoRight(bottomLeftCorner.X, bottomLeftCorner.Y, 0, galaxyWidth)!);
            return allLocations;
        }
        public static Coordinates? GoLeft(int X, int Y, int distance, int galaxyWidth)
        {
            if (Y < 0 || Y > galaxyWidth) return null;
            if (X - distance > 0 && X + distance < galaxyWidth)
            {
                return new Coordinates() { X = X - distance, Y = Y };
            }
            else return null!;
        } 
        public static Coordinates? GoRight(int X, int Y, int distance, int galaxyWidth)
        {
            if (Y < 0 || Y > galaxyWidth) return null;

            if (X - distance > 0 && X + distance < galaxyWidth)
            {
                return new Coordinates() { X = X + distance, Y = Y };
            }
            else return null!;
        }
        public static Coordinates? GoUp(int X, int Y, int distance, int galaxyWidth)
        {
            if (X < 0 || X > galaxyWidth) return null;

            if (Y - distance >= 0 && Y - distance < galaxyWidth)
            {
                return new Coordinates(){ X = X , Y = Y + distance };
            }
            else return null!;
        }
        public static Coordinates? GoDown(int X, int Y, int distance, int galaxyWidth)
        {
            if (X < 0 || X > galaxyWidth) return null;

            if (Y - distance >= 0 && Y - distance < galaxyWidth)
            {
                return new Coordinates(){ X = X , Y = Y - distance };
            }
            else return null!;
        }
             
    }
}
