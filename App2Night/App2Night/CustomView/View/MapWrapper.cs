using App2Night.Service.Helper;
using App2Night.Service.Interface;
using FreshMvvm;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App2Night.CustomView.View
{
    /// <summary>
    /// Warps Map in a <see cref="ContentView"/>.
    /// </summary>
    public class MapWrapper : ContentView
    {
        private readonly Map _map;

        public MapWrapper(Map map)
        {
            _map = map;
            HeightRequest = map.HeightRequest;
            // check if maps is avaible on device
            if (Device.OS == TargetPlatform.Android && !App.MapAvailable)
            {
                BackgroundColor = Color.Gray.MultiplyAlpha(0.6);
                Content = new Label {Text = "Google maps is not installed on this device.",
                    Margin = new Thickness(10)
                }; //RESOURCE
            } 
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var locationAvailable = await CoordinateHelper.HasGeolocationAccess();
                    var locationPermissionsAvailable = await 
                        FreshIOC.Container.Resolve<IAlertService>().RequestLocationPermissions();
                    map.IsShowingUser = locationAvailable;

                    if (locationPermissionsAvailable)
                    {
                        Content = map;
                    } 
                    else
                        Content = new Label { Text = "Location is not enabled for this app or available on your device.\n" +
                                                     "Activate the location for this device and restart the app to see a beautiful map here.",
                        Margin = new Thickness(10)}; //RESOURCE
                });
            } 
        }

        public Map Map => _map;
    }
}