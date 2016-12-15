using App2Night.Model.Language;
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
                Content = new Label {Text = AppResources.MapsNotInstalled,
                    Margin = new Thickness(10)
                };  
            } 
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var locationAvailable = await CoordinateHelper.HasGeolocationAccess();
                    var locationPermissionsAvailable = await 
                        FreshIOC.Container.Resolve<IAlertService>().RequestLocationPermissions();
                   

                    if (locationPermissionsAvailable )
                    {
                        map.IsShowingUser = locationAvailable; 
                    }

                    if (locationAvailable)
                    {
                        Content = map;
                    }
                    else
                        Content = new Label { Text = AppResources.LocationNotEnabled,
                        Margin = new Thickness(10)};  
                });
            } 
        }

        public Map Map => _map;
    }
}