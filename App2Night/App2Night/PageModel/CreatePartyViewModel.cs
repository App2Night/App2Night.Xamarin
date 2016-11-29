using System;
using System.Threading;
using System.Threading.Tasks;
using App2Night.Model.Enum;
using App2Night.Model.Model;
using App2Night.Service.Helper;
using App2Night.Service.Interface;
using FreshMvvm;
using PropertyChanged;
using Xamarin.Forms;

namespace App2Night.PageModel
{
    [ImplementPropertyChanged]
    public class CreatePartyViewModel : FreshBasePageModel
    {
        Location _location;
		string _streetName;
        string _zipcode;
		string _houseNumber;
        private string _cityName;

        [AlsoNotifyFor(nameof(AcceptButtonEnabled),nameof(DeleteButtonEnabled))] 
        public string Name { get; set; }

        [AlsoNotifyFor(nameof(ValidDescription))]
        public string Description { get; set; }

        public MusicGenre MusicGenre { get; set; }

        public TimeSpan Time { get; set; }

        [AlsoNotifyFor(nameof(ValidDate))]
        public DateTime Date { get; set; }
         
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

        [AlsoNotifyFor(nameof(ValidLocationname))]
        public string LocationName { get; set; }

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

        [AlsoNotifyFor(nameof(AcceptButtonEnabled), nameof(DeleteButtonEnabled))] 
        public bool ValidStreetname { get; private set; }

        [AlsoNotifyFor(nameof(AcceptButtonEnabled), nameof(DeleteButtonEnabled))]
        public bool ValidCityname { get; private set; }

        [AlsoNotifyFor(nameof(AcceptButtonEnabled), nameof(DeleteButtonEnabled))]
        public bool ValidHousenumber { get; private set; }

        [AlsoNotifyFor(nameof(AcceptButtonEnabled), nameof(DeleteButtonEnabled))] 
        public bool ValidZipcode { get; private set; }

        public bool AcceptButtonEnabled
        {
            get
            {
                return ValidCityname
                       && ValidDate
                       && ValidDescription
                       && ValidHousenumber
                       && ValidLocationname
                       && ValidName
                       && ValidZipcode
                       && ValidStreetname;
            }
        }

        public bool DeleteButtonEnabled
        {
            get
            {
                return !(string.IsNullOrEmpty(CityName)
                         && string.IsNullOrEmpty(Name)
                         && string.IsNullOrEmpty(Description)
                         && string.IsNullOrEmpty(Zipcode)
                         && string.IsNullOrEmpty(StreetName)
                         && string.IsNullOrEmpty(HouseNumber)
                         && string.IsNullOrEmpty(LocationName));
            }
        }

        [AlsoNotifyFor(nameof(AcceptButtonEnabled), nameof(DeleteButtonEnabled))]
        public bool ValidLocationname => ValidateLocationname();

        [AlsoNotifyFor(nameof(AcceptButtonEnabled), nameof(DeleteButtonEnabled))]
        public bool ValidDate => ValidateDate();

        [AlsoNotifyFor(nameof(AcceptButtonEnabled), nameof(DeleteButtonEnabled))]
        public bool ValidName => ValidateName();

        [AlsoNotifyFor(nameof(AcceptButtonEnabled), nameof(DeleteButtonEnabled))]
        public bool ValidDescription => ValidateDescription();

        public Command CreatePartyCommand => new Command(async () => await CreateParty());
        public Command ClearFormCommand => new Command(ClearForm);

        private void ClearForm()
        {
            Zipcode = string.Empty;
            CityName = string.Empty;
            StreetName = string.Empty;
            HouseNumber = string.Empty;
            LocationName = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
        }

        public bool ValidateDate()
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
                    FreshIOC.Container.Resolve<IDataService>()
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

            var result =await FreshIOC.Container.Resolve<IDataService>().ValidateLocation(locationData);

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
    }
}