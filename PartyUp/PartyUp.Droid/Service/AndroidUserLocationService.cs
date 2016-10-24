using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Locations;
using PartyUp.DependencyService;
using PartyUp.Droid.Service;
using PartyUp.Model.Enum;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidUserLocationService))]
namespace PartyUp.Droid.Service
{
    public class AndroidUserLocationService : IUserLocationService
    {
        LocationManager _locationManager;
        string _locationProvider;
        public Coordinates GetUserCoordinates()
        {
            InitializeLocationManager();
            Location loc = _locationManager.GetLastKnownLocation(_locationProvider);
            return new Coordinates
            {
                Latitude = (float) loc.Latitude,
                Longitude = (float) loc.Longitude
            };
        }
        private void InitializeLocationManager()
        {
            _locationManager = (LocationManager)Application.Context.GetSystemService(Context.LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                _locationProvider = string.Empty;
            }
        }
    }
}