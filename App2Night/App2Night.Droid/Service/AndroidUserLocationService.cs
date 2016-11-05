using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Locations;
using Android.OS;
using App2Night.DependencyService;
using App2Night.Droid.Service;
using App2Night.Model.Model;
using Application = Android.App.Application;
using Location = Android.Locations.Location;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidUserLocationService))]

namespace App2Night.Droid.Service
{
    public class AndroidUserLocationService : Java.Lang.Object, IUserLocationService, ILocationListener
    {
        LocationManager _locationManager;
        string _locationProvider;
        private Coordinates _currentLocation;
        private Location _location;
        public event EventHandler<Coordinates> LocationChanged;
        public event EventHandler<LocationChangeEventArgs> LocationStatusChanged;


        public AndroidUserLocationService()
        {
            InitializeLocationManager();
        }


        /// <summary>
        /// Locates the user's position with the local location manager.
        /// </summary>
        /// <returns><code>Coordinates</code> of the User.</returns>
        public Coordinates GetUserCoordinates()
        {
            Location loc = _locationManager.GetLastKnownLocation(_locationProvider);
            if (loc != null)
                _currentLocation = new Coordinates
                {
                    Latitude = (float)loc.Latitude,
                    Longitude = (float)loc.Longitude
                };
            return _currentLocation;
        }

        /// <summary>
        /// Initializes the <code>LocationManager</code>, which is necessary to locate the user.
        /// </summary>
        private void InitializeLocationManager()
        {
            // initialize locationManager
            _locationManager = (LocationManager) Application.Context.GetSystemService(Context.LocationService);
            // set criteria
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine,
            };
            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);
            // set provider
            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                _locationProvider = string.Empty;
            }
            _locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
        }

        public void OnLocationChanged(Location location)
        {
            _location = location;
            LocationChanged?.Invoke(this, new Coordinates { Latitude = (float)_location.Latitude, Longitude = (float)_location.Longitude});
        }


        public void OnProviderDisabled(string provider)
        {
            LocationStatusChanged?.Invoke(this, new LocationChangeEventArgs(false, null));
        }

        public void OnProviderEnabled(string provider)
        {
            LocationStatusChanged?.Invoke(this, new LocationChangeEventArgs(true, _currentLocation));
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            if (LocationStatusChanged != null)
            {
                var hasFix = status == Availability.Available;
                LocationStatusChanged(this, new LocationChangeEventArgs(hasFix, _currentLocation));
            }
        }
    }
}