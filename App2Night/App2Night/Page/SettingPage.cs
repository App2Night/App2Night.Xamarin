using App2Night.CustomView.Page;
using App2Night.Data.Language;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.Page
{
    public class SettingPage : CustomContentPage 
    {
        public SettingPage()
        {
            //GPS
            //Slider
            var gpsRangeSlider = new Slider(5, 200, 1);
            gpsRangeSlider.SetBinding( Slider.ValueProperty, "SelectedRange");

            var gpsEnabledSwitch = new Switch
            {
                HorizontalOptions = LayoutOptions.End
            };
            gpsEnabledSwitch.SetBinding( Switch.IsToggledProperty, "GpsEnabled");

            var stackLayout = new StackLayout
            {
                Padding = new Thickness(5),
                Children =
                {
                    new Label {Text = AppResources.GpsSettingHeader},
                    gpsRangeSlider,
                    new Grid
                    {
                        Children =
                        {
                            gpsEnabledSwitch,
                            new Label {Text = AppResources.EnableGps, HorizontalOptions = LayoutOptions.Start}
                        }
                    },
                    new Label {Text = AppResources.GpsUsage}
                }
            };
            Content = stackLayout;
        }
    }
}