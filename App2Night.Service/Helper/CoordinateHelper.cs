using System;
using System.Threading.Tasks;
using App2Night.Model.Model;
using App2Night.Service.Interface;
using FreshMvvm;
using Plugin.Geolocator;

namespace App2Night.Service.Helper
{
    /// <summary>
    /// Helper class for coordinates.
    /// </summary>
    public static class CoordinateHelper
    {
        public static async Task<Coordinates> GetCoordinates(bool fallBackToDefaultCoordinates = true)
        {
            var storageService = FreshIOC.Container.Resolve<IStorageService>();

            //TODO check if user is alowed to use gps
            var storageEnabled = storageService.Storage.UseGps //Check if gps usage is enabled in the settigns.
                && CrossGeolocator.Current.IsGeolocationAvailable  //Check if gps usage is available on the device.
                && CrossGeolocator.Current.IsGeolocationEnabled; //Check if gps usage is enabled on the device.

            Coordinates coordinates = null;

            if (storageEnabled)
            {
                try
                {
                    //Get coordinates from GPS
                    var location = await CrossGeolocator.Current.GetPositionAsync(10000);
                    coordinates = new Coordinates()
                    {
                        Latitude = location.Latitude,
                        Longitude = location.Longitude
                    };
                }
                catch (TaskCanceledException e)
                {
                    DebugHelper.PrintDebug(DebugType.Error, "Getting location in time failed.\n" + e);
                }
            }

            //Backup if gps is disabled or fetching from gps didnt return a valid result.
            if (storageEnabled || coordinates == null)
            {
                //TODO Resolve gps data from the storage
            }

            //Take default coordinates if everything else fails.
            if (coordinates == null && fallBackToDefaultCoordinates)
            {
                coordinates = new Coordinates(10.324663f, 51.273610f);
            }

            return coordinates;
        }


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