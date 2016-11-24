using System;
using System.Globalization;
using App2Night.CustomView.Page;
using App2Night.Data.Language;
using App2Night.ViewModel;
using Xamarin.Forms;

namespace App2Night.View
{
    public class SettingPage : ContentPageWithInfo<SettingViewModel>
    {
        public SettingPage()
        {
            //GPS
            //Slider
            var gpsRangeSlider = new Slider(5, 200, 1);
            BindToViewModel(gpsRangeSlider, Slider.ValueProperty, vm => vm.SelectedRange);

            var gpsEnabledSwitch = new Switch
            {
                HorizontalOptions = LayoutOptions.End
            };
            BindToViewModel(gpsEnabledSwitch, Switch.IsToggledProperty, vm => vm.GpsEnabled);

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