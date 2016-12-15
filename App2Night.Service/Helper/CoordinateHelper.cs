using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using App2Night.Model.Model;
using App2Night.Service.Interface;
using FreshMvvm;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace App2Night.Service.Helper
{
    /// <summary>
    /// Helper class for coordinates.
    /// </summary>
    public static class CoordinateHelper
    {
        public static async Task<bool> HasGeolocationAccess()
        {
            var storageService = FreshIOC.Container.Resolve<IStorageService>();

            var storageEnabled = storageService.Storage.UseGps //Check if gps usage is enabled in the settigns.
                                 && CrossGeolocator.Current.IsGeolocationAvailable;  //Check if gps usage is available on the device. 
              

            if (!storageEnabled) return false;

            storageEnabled = await FreshIOC.Container.Resolve<IAlertService>().RequestLocationPermissions();

            //Since CrossGeolocator does not catch if uwp denied the access, ask uwp again!
            if (Device.OS == TargetPlatform.Windows && storageEnabled)
            {
                var locationAccess = DependencyService.Get<ILocationAccess>();
                storageEnabled = await locationAccess.HasAccess(); 
            }

            return storageEnabled;
        }

        public static async Task<Coordinates> GetCoordinates(bool fallBackToDefaultCoordinates = true)
        {
            var storageService = FreshIOC.Container.Resolve<IStorageService>(); 

            var storageEnabled =await HasGeolocationAccess();
            Coordinates coordinates = null;

            if (storageEnabled && CrossGeolocator.Current.IsGeolocationEnabled)
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
                var savedLocationData = storageService.Storage.ManualLocation;
                if (savedLocationData != null)
                    coordinates = new Coordinates((float) savedLocationData.Longitude, (float) savedLocationData.Latitude);
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