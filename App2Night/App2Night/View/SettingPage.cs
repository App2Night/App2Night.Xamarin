using System;
using System.Globalization;
using App2Night.CustomView.Page;
using App2Night.Data.Language;
using App2Night.ViewModel; 
using Xamarin.Forms;

namespace App2Night.View
{
    public class SettingPage : ContentPageWithInfo<SettingViewModel> {

        Picker _languagePicker = new Picker();

        public SettingPage()
        { 
            
            BindToViewModel(_languagePicker, Picker.SelectedIndexProperty, vm => vm.SelectedLanguage);

            var stackLayout = new StackLayout
            {
                 Children =
                 {
                     new Label {Text = AppResources.PreferedLanguage},
                     _languagePicker
                 }
            };
            Content = stackLayout;
        }

        public override void OnViewModelSet()
        {
            base.OnViewModelSet();
            foreach (Tuple<string, CultureInfo> culture in ViewModel.Cultures)
            {
                _languagePicker.Items.Add(culture.Item1);
            }
            _languagePicker.SelectedIndex = 0;
        }
    }
}