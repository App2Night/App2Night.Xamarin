using System;
using System.Threading;
using System.Threading.Tasks;
using App2Night.Model.Model;
using App2Night.Service.Helper;
using App2Night.Service.Interface;
using FreshMvvm;
using PropertyChanged;
using Xamarin.Forms;

namespace App2Night.PageModel
{
    [ImplementPropertyChanged]
    public class SettingViewModel : FreshBasePageModel
    {
        private readonly IStorageService _storageService;
        private int _selectedRange;
        private string _cityName;
        public string CityName
        {
            get { return _cityName; }
            set
            {
                _cityName = value;
                StartLocationValidation();
                
            }
        }
        private string _houseNumber;
        public string HouseNumber
        {
            get { return _houseNumber; }
            set
            {
                _houseNumber = value;
                StartLocationValidation();
            }
        }
        private string _streetName;
        public string StreetName
        {
            get { return _streetName; }
            set
            {
                _streetName = value;
                StartLocationValidation();
            }
        }
        private string _zipcode;
        public string Zipcode
        {
            get { return _zipcode; }
            set
            {
                _zipcode = value;
                StartLocationValidation();
            }

        }

        public bool ValidStreetname { get; private set; }

        public bool ValidCityname { get; private set; }

        public bool ValidHousenumber { get; private set; }

        public bool ValidZipcode { get; private set; }


        public SettingViewModel(IStorageService storageService)
        {
            _storageService = storageService; 
        }

        public int SelectedRange
        {
            get { return _storageService.Storage.FilterRadius; }
            set
            {
                _storageService.Storage.FilterRadius = value;
                //TODO save range after x amount of time.
            }
        }

        public bool GpsEnabled
        {
            get { return _storageService.Storage.UseGps; }
            set
            {
                _storageService.Storage.UseGps = value;
                _storageService.SaveStorage();
                //TODO show a manuel position entry view if usegps = false 
            }
        }

        public Command ClearCacheCommand => new Command(async () => await _storageService.ClearCache());
        #region ValidatePosition
        DateTime _lastLocationChange = new DateTime();

        private CancellationTokenSource _lastCancellationTokenSource;
        private void StartLocationValidation()
        {
            if (_lastCancellationTokenSource != null)
            {
                _lastCancellationTokenSource.Cancel();
                _lastCancellationTokenSource.Dispose();
            }

            _lastCancellationTokenSource = new CancellationTokenSource();
            var task = Task.Run(async () =>
            {
                try
                {
                    var timestamp = DateTime.Now;
                    _lastLocationChange = timestamp;
                    //Wait for another user input that cancels this task
                    await Task.Delay(1000);
                    //Check if this was the last user input
                    if (timestamp == _lastLocationChange)
                    {
                        //Start location check
                        await CheckLocation();
                    }
                }
                catch (TaskCanceledException e)
                {
                    // ignored
                }
                catch (Exception e)
                {
                    DebugHelper.PrintDebug(DebugType.Error, "Starting location validation process failed\n" + e);
                }
            }, _lastCancellationTokenSource.Token);
        }

        private async Task CheckLocation()
        {
            var locationData = new Location
            {
                CityName = CityName,
                CountryName = "Germany",
                StreetName = StreetName,
                HouseNumber = HouseNumber,
                Zipcode = Zipcode
            };

            var result = await FreshIOC.Container.Resolve<IDataService>().ValidateLocation(locationData);

            if (result.Success)
            {
                var resLocation = result.Data;

                if (IsEqualOrContains(resLocation.CityName, CityName))
                    CityName = resLocation.CityName;

                if (IsEqualOrContains(resLocation.StreetName, StreetName))
                    StreetName = resLocation.StreetName;

                ValidCityname = IsNameEqual(resLocation.CityName, CityName);
                ValidZipcode = IsNameEqual(resLocation.Zipcode, Zipcode);
                ValidStreetname = IsNameEqual(resLocation.StreetName, StreetName);
                ValidHousenumber = IsNameEqual(resLocation.HouseNumber, HouseNumber);

                if (ValidCityname && IsEqualOrContains(resLocation.Zipcode, Zipcode))
                    Zipcode = resLocation.Zipcode;

                if (IsEqualOrContains(resLocation.StreetName, StreetName))
                {
                    StreetName = resLocation.StreetName;
                }

                if (ValidStreetname && IsEqualOrContains(resLocation.CityName, CityName) &&
                    IsEqualOrContains(resLocation.Zipcode, Zipcode))
                {
                    CityName = resLocation.CityName;
                    Zipcode = resLocation.Zipcode;

                    ValidCityname = true;
                    ValidZipcode = true;
                }
            }
        }
        #endregion
        bool IsEqualOrContains(string final, string notFinal)
        {
            if (string.IsNullOrEmpty(final)) return false;
            if (string.IsNullOrEmpty(notFinal)) return true;
            final = NormalizeString(final);
            notFinal = NormalizeString(notFinal);
            return string.IsNullOrEmpty(final) || final.Contains(notFinal);
        }

        bool IsNameEqual(string first, string second)
        {
            if (string.IsNullOrEmpty(first) || string.IsNullOrEmpty(second)) return false;

            first = NormalizeString(first);
            second = NormalizeString(second);
            return first == second;
        }

        string NormalizeString(string s)
        {
            return s.ToLower().Replace(" ", "");
        }
    }
}