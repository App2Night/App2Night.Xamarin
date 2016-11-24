using System;
using App2Night.Model.Model;

namespace App2Night.Service.Helper
{
    /// <summary>
    /// Helper class for coordinates.
    /// </summary>
    public static class CoordinateHelper
    {
        /// <summary>
        /// Calculates the distance between to coordinates. 
        /// Includs the fact that the earth is actually not a pizza.
        /// 
        /// Note: The logic for this code is taken from various blogs.
        /// </summary>
        /// <param name="coordinates">First coordinate.</param>
        /// <param name="toCoordinates">Second coordinate.</param>
        /// <returns>The distance between the two coordinates.</returns>
        public static double DistanceTo(this Coordinates coordinates, Coordinates toCoordinates)
        {
            double circumference = 40000.0;  
            double distance = 0.0;

            //Calculate radians
            double latitude1Rad = DegreesToRadians(coordinates.Latitude);
            double longitude1Rad = DegreesToRadians(coordinates.Longitude);
            double latititude2Rad = DegreesToRadians(toCoordinates.Latitude);
            double longitude2Rad = DegreesToRadians(toCoordinates.Longitude);

            double logitudeDiff = Math.Abs(longitude1Rad - longitude2Rad);

            if (logitudeDiff > Math.PI)
            {
                logitudeDiff = 2.0 * Math.PI - logitudeDiff;
            }

            double angleCalculation =
                Math.Acos(
                  Math.Sin(latititude2Rad) * Math.Sin(latitude1Rad) +
                  Math.Cos(latititude2Rad) * Math.Cos(latitude1Rad) * Math.Cos(logitudeDiff));

            distance = circumference * angleCalculation / (2.0 * Math.PI);

            return distance;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
}