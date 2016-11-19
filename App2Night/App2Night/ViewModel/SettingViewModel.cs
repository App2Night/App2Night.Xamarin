using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using App2Night.Data.Language;
using App2Night.DependencyService;
using MvvmNano;

namespace App2Night.ViewModel
{
    public class SettingViewModel : MvvmNanoViewModel
    {
        private int _selectedLanguage;

        public IList<Tuple<string, CultureInfo>> Cultures = new List<Tuple<string, CultureInfo>>
        {
            new Tuple<string, CultureInfo>("Deutsch", new CultureInfo("de")),
            new Tuple<string, CultureInfo>("English", new CultureInfo("en")) 
        };  

        public SettingViewModel()
        {
            try
            {
                var culture = Cultures.FirstOrDefault(o => o.Item2.Name == AppResources.Culture.Name);
                if(culture != null)
                    SelectedLanguage = Cultures.IndexOf(culture);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public int SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                if (_selectedLanguage != value && value >= 0)
                {
                    _selectedLanguage = value;
                    SetCulture(Cultures[_selectedLanguage].Item2);
                }
            }
        }

        private void SetCulture(CultureInfo culture)
        { 
            if (culture != null)
            {
                var oldCulture = AppResources.Culture;
                Debug.WriteLine("Old culture: " + oldCulture.Name + " new culture " + culture.Name);
                AppResources.Culture = culture; 
                Xamarin.Forms.DependencyService.Get<ICultureService>().SetLocale(culture); 
            }
        }
    }
}