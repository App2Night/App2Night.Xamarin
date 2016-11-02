using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App2Night.CustomView
{
    public class MapWrapper : ContentView
    {
        private readonly Map _map;

        public MapWrapper(Map map)
        {
            _map = map;
            HeightRequest = map.HeightRequest;
            if (Device.OS == TargetPlatform.Android && !App.MapAvailable)
            {
                BackgroundColor = Color.Gray.MultiplyAlpha(0.6);
                Content = new Label {Text = "Google maps is not installed on this device."};
            }
            else
            {
                Content = map;
            }
        }

        public Map Map => _map;
    }
}