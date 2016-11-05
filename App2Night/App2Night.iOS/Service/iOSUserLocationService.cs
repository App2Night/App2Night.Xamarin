using System;
using App2Night.DependencyService;
using App2Night.iOS.Service;
using App2Night.Model.Model;
using CoreLocation;

[assembly: Xamarin.Forms.Dependency(typeof(iOSUserLocationService))]
namespace App2Night.iOS.Service
{
    public class iOSUserLocationService : IUserLocationService
    {
        private CLLocationManager _locationManager;
        public event EventHandler<Coordinates> LocationChanged;
        public event EventHandler<LocationChangeEventArgs> LocationStatusChanged;

        public iOSUserLocationService()
        {
            InitializeLocationManager();
        }
        public Coordinates GetUserCoordinates()
        {
            var test = _locationManager.Location;
            return new Coordinates
            {
                Longitude = (float)test.Coordinate.Longitude,
                Latitude = (float) test.Coordinate.Latitude
            };
        }

        private void InitializeLocationManager()
        {
            _locationManager = new CLLocationManager();
            _locationManager.RequestWhenInUseAuthorization();
            _locationManager.DesiredAccuracy = 100;
            _locationManager.RequestLocation();
        }
    }
}