 
using System;
using App2Night.DependencyService;
using App2Night.Model.Model;
using App2Night.UWP.Service;

[assembly: Xamarin.Forms.Dependency(typeof(WindowsUserLocationService))] 
namespace App2Night.UWP.Service
{
    public class WindowsUserLocationService : IUserLocationService
    {
        public event EventHandler<Coordinates> LocationChanged;
        public event EventHandler<LocationChangeEventArgs> LocationStatusChanged;
        public Coordinates GetUserCoordinates()
        {
            return new Coordinates
            {
                Latitude = 48.595184F,
                Longitude = 8.859824F
            };
        }
    }
}