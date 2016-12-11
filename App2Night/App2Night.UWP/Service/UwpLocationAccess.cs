using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using App2Night.DependencyService;
using App2Night.Service.Interface;
using App2Night.UWP.Service;
using Xamarin.Forms;

[assembly: Dependency(typeof(UwpLocationAccess))]
namespace App2Night.UWP.Service
{
     
    public class UwpLocationAccess : ILocationAccess
    {
        public async Task<bool> HasAccess()
        {

            var geolocator = new Geolocator();
            if (geolocator.LocationStatus == PositionStatus.NotAvailable) return false;
            var result = await Geolocator.RequestAccessAsync();
            return result == GeolocationAccessStatus.Allowed;
        }
    }
}