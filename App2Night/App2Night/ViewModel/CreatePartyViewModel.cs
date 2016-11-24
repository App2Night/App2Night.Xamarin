using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using App2Night.Model.Enum;
using App2Night.Model.Model;
using App2Night.Service;
using App2Night.Service.Helper;
using App2Night.Service.Interface;
using MvvmNano;
using Plugin.Media;
using PropertyChanged;
using Xamarin.Forms;

namespace App2Night.ViewModel
{
    [ImplementPropertyChanged]
    public class CreatePartyViewModel : MvvmNanoViewModel
    {
		string _name;
		string _description;
		MusicGenre _musicGenre;
		TimeSpan _time;
		DateTime _date;
		Location _location;
		string _streetName;
		string _locationName;
		string _zipcode;
		string _houseNumber;
        private string _cityName; 

        public string Name 
		{ 
			get { return _name;} 
			set 
			{
				_name = value;
				NotifyPropertyChanged(nameof(ValidName));
			} 
		} 
        

        public string Description 
		{ 
			get { return _description; } 
			set 
			{ 
				_description = value;
				NotifyPropertyChanged(nameof(ValidDescription));
			} 
		}

		public MusicGenre MusicGenre 
		{ 
			get { return _musicGenre; } 
			set
			{ 
				_musicGenre = value; 
			} 
		}

		public TimeSpan Time 
		{ 
			get { return _time; } 
			set { _time = value; }
		}

		public DateTime Date 
		{ 
			get { return _date; }
		    set
		    {
		        _date = value; 
		        NotifyPropertyChanged(nameof(ValidDate));
		    } 
		}

		public string StreetName
		{
			get { return _streetName;}
			set 
			{
				_streetName = value;
			    StartLocationValidation();
			}
		}

        public string CityName
        {
            get { return _cityName; }
            set
            {
                _cityName = value;
                StartLocationValidation();
            } 
        }

        public string LocationName
		{
			get { return _locationName; }
			set
			{
				_locationName = value;
                NotifyPropertyChanged(nameof(ValidLocationname));

            }
        }

		public string Zipcode
		{
			get { return _zipcode; }
			set
			{
				_zipcode = value;
                StartLocationValidation();
            }
		}

		public string HouseNumber
		{
			get { return _houseNumber; }
			set
			{
				_houseNumber = value;
                StartLocationValidation();
            }
		}

        public bool ValidStreetname { get; private set; } 

        public bool ValidCityname { get; private set; }

        public bool ValidHousenumber { get; private set; }

        public bool ValidZipcode { get; private set; }

        public bool ValidLocationname => ValidateLocationname(); 

        public bool ValidDate => ValidateDate(); 

        public bool ValidName => ValidateName(); 

		public bool ValidDescription => ValidateDescription();
        public MvvmNanoCommand CreatePartyCommand => new MvvmNanoCommand(async ()=> await CreateParty()); 

        private bool ValidateDate()
        {
            return Date >= DateTime.Today && Date <= DateTime.Today.AddMonths(12);
        }

        private bool ValidateLocationname()
        {
            return !string.IsNullOrEmpty(LocationName);
        }

        bool ValidateName()
        {
            return !string.IsNullOrEmpty(Name) && Name.Length > 3;
        }

        bool ValidateDescription()
		{
			return !string.IsNullOrEmpty(Description);
		}

        private async Task CreateParty()
        {
            var dateTime = new DateTime(Date.Year, Date.Month, Date.Day, Time.Hours, Time.Minutes, 0);
            var result = await
                    MvvmNanoIoC.Resolve<IDataService>()
                        .CreateParty(Name, dateTime, MusicGenre, "Germany", CityName, StreetName, HouseNumber, Zipcode,
                            PartyType.Bar, Description);
        }

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
            var task =Task.Run(async () =>
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

            var result =await MvvmNanoIoC.Resolve<IDataService>().ValidateLocation(locationData);

            if (result.Success)
            {
                var resLocation = result.Data;

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
        public MvvmNanoCommand LoadImageCommand => new MvvmNanoCommand(async () => await MediaPicker());

        Image _image = new Image();

        public Image Image
        {
            get { return _image; }
            set
            {
                _image = value;
                NotifyPropertyChanged(nameof(MediaPicker));
            }
        }

        private async Task MediaPicker()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                UserDialogs.Instance.Alert("No Camera", "OK");
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                UserDialogs.Instance.Alert("File Location", file.Path, "OK");

            Image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }
    }
}