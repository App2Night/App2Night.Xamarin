using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using App2Night.Data.Language;
using App2Night.Model.Model;
using App2Night.PageModel.SubPages;
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
        private readonly IDataService _dataService;
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
            }
        }

        private Location _gpsManuelLocation;

        public SettingViewModel(IStorageService storageService)
        {
            _storageService = storageService;

            var lastLocation = _storageService.Storage.ManualLocation;

            if (lastLocation != null)
            {
                CityName = lastLocation.CityName;
                StreetName = lastLocation.StreetName;
                HouseNumber = lastLocation.HouseNumber;
                Zipcode = lastLocation.Zipcode;
            } 
        } 

        public Command ValidateClearCacheCommand => new Command(() =>
        {
            UserDialogs.Instance.Confirm(
                new ConfirmConfig().SetMessage(AppResources.ClearCacheValid)
                    .SetOkText(AppResources.Yes)
                    .SetCancelText(AppResources.No)
                    .SetAction(b =>
                    {
                        if (b)
                        {
                            _storageService.ClearCache();
                        }
                    }));
        });

        public Command MoveToReadAgbCommand => new Command(async () => await CoreMethods.PushPageModel<AgbViewModel>());

        #region location validation

        private DateTime _lastInputTime;

        private Task _validationTask;

        /// <summary>
        /// Starts a task that sets a timestamp to the current time and starts the validation when the time between the timestamp and the current time is greater then 500 ms.
        /// Calling this method while the task is still running will reset the timestamp to the current time.
        /// </summary>
        private void StartLocationValidation()
        {
            if (_validationTask != null && !_validationTask.IsCompleted)
            {
                _lastInputTime = DateTime.Now;
            }
            else
            {
                _validationTask = Task.Run(async () =>
                {
                    _lastInputTime = DateTime.Now;
                    try
                    {
                        while ((DateTime.Now - _lastInputTime).TotalMilliseconds < 500)
                        {
                            await Task.Delay(50);
                        }

                        //Start location check
                        await CheckLocation();
                    }
                    catch (Exception e)
                    {
                        DebugHelper.PrintDebug(DebugType.Error, "Starting location validation process failed\n" + e);
                    }
                });
            }
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

            if (result.StatusCode == (int)HttpStatusCode.NotAcceptable || result.Success)
            {
                var resLocation = result.Data;

                if (IsEqualOrContains(resLocation.CityName, CityName))
                    CityName = resLocation.CityName;

                if (IsEqualOrContains(resLocation.StreetName, StreetName))
                    StreetName = resLocation.StreetName;

                ValidateAllLocationInputs(resLocation);

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

                ValidateAllLocationInputs(resLocation);

                _storageService.Storage.ManualLocation = resLocation;
                await _storageService.SaveStorage();
            }
        }

        void ValidateAllLocationInputs(Location comparisonLocation)
        {
            ValidCityname = IsNameEqual(comparisonLocation.CityName, CityName);
            ValidZipcode = IsNameEqual(comparisonLocation.Zipcode, Zipcode);
            ValidStreetname = IsNameEqual(comparisonLocation.StreetName, StreetName);
            ValidHousenumber = IsNameEqual(comparisonLocation.HouseNumber, HouseNumber);
        }

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

        #endregion

        public SettingViewModel(IStorageService storageService, IDataService dataService)
        {
            _storageService = storageService;
            _dataService = dataService;
        }
    }
}