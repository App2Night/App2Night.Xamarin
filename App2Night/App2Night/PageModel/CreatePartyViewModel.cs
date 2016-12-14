using System; 
using System.Net; 
using System.Threading.Tasks;
using Acr.UserDialogs;
using App2Night.Model.Enum;
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
    public class CreatePartyViewModel : FreshBasePageModel
    {
        Location _location;
		string _streetName;
        string _zipcode;
		string _houseNumber;
        private string _cityName;
        private int _price;


        [AlsoNotifyFor(nameof(AcceptButtonEnabled),nameof(ClearButtonEnabled))] 
        public string Name { get; set; }

        [AlsoNotifyFor(nameof(ValidDescription))]
        public string Description { get; set; }

        public string Price
        {
            get { return _price.ToString(); }
            set
            {
                var newPrice = int.Parse(value);
                if(newPrice >= 0)
                    _price = newPrice;
            }
        }

        public MusicGenre MusicGenre { get; set; }

        public TimeSpan Time { get; set; }

        [AlsoNotifyFor(nameof(ValidDate))]
        public DateTime Date { get; set; } = DateTime.Today;
         
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

        [AlsoNotifyFor(nameof(AcceptButtonEnabled), nameof(ClearButtonEnabled))] 
        public bool ValidStreetname { get; private set; }

        [AlsoNotifyFor(nameof(AcceptButtonEnabled), nameof(ClearButtonEnabled))]
        public bool ValidCityname { get; private set; }

        [AlsoNotifyFor(nameof(AcceptButtonEnabled), nameof(ClearButtonEnabled))]
        public bool ValidHousenumber { get; private set; }

        [AlsoNotifyFor(nameof(AcceptButtonEnabled), nameof(ClearButtonEnabled))] 
        public bool ValidZipcode { get; private set; }
         
        public bool AcceptButtonEnabled
        {
            get
            {  
                var enabled =
                       ValidCityname 
                       && ValidDescription
                       && ValidHousenumber 
                       && ValidName
                       && ValidZipcode
                       && ValidStreetname;
                return enabled;
            }
        }

        public bool ClearButtonEnabled
        {
            get
            {
                var enabled = !(string.IsNullOrEmpty(CityName)
                                && string.IsNullOrEmpty(Name)
                                && string.IsNullOrEmpty(Description)
                                && string.IsNullOrEmpty(Zipcode)
                                && string.IsNullOrEmpty(StreetName)
                                && string.IsNullOrEmpty(HouseNumber));
                return enabled;
            }
        } 

        [AlsoNotifyFor(nameof(AcceptButtonEnabled), nameof(ClearButtonEnabled))]
        public bool ValidDate => ValidateDate();

        [AlsoNotifyFor(nameof(AcceptButtonEnabled), nameof(ClearButtonEnabled))]
        public bool ValidName => ValidateName();

        [AlsoNotifyFor(nameof(AcceptButtonEnabled), nameof(ClearButtonEnabled))]
        public bool ValidDescription => ValidateDescription();

        public Command CreatePartyCommand => new Command(async () => await CreateParty());
        public Command ClearFormCommand => new Command(ClearForm); 

        private void ClearForm()
        {
            Zipcode = string.Empty;
            CityName = string.Empty;
            StreetName = string.Empty;
            HouseNumber = string.Empty; 
            Name = string.Empty;
            Description = string.Empty;
        }

        public bool ValidateDate()
        {
            return Date > DateTime.Today && Date <= DateTime.Today.AddMonths(12);
        }  

        bool ValidateName()
        {
            return !string.IsNullOrEmpty(Name) && Name.Length <= 32;
        }

        bool ValidateDescription()
		{
			return !string.IsNullOrEmpty(Description) && Description.Length <= 256;
		}

        private async Task CreateParty()
        {
            using (UserDialogs.Instance.Loading("Creating Party")) //RESOURCE 
            {
                var dateTime = new DateTime(Date.Year, Date.Month, Date.Day, Time.Hours, Time.Minutes, 0);
                var result = await
                        FreshIOC.Container.Resolve<IDataService>()
                            .CreateParty(Name, dateTime, MusicGenre, "Germany", CityName, StreetName, HouseNumber, Zipcode,
                                PartyType.Bar, Description, _price);

                if (result.Success)
                {
                    //The user should come to the dashboard after popping the party detail page.
                    await CoreMethods.SwitchSelectedMaster<DashboardPageModel>();

                    await CoreMethods.PushPageModel<MyPartyDetailViewModel>(result.Data);
                }
                else
                {
                    //TODO Handle failure
                }
            }   
        }

        #region location validation
         
        private DateTime _lastInputTime;

        private Task _validationTask;

        /// <summary>
        /// Starts a task that sets a timestamp to the current time and starts the validation when the time between the timestamp and the current time is greater then 500 ms.
        /// Calling this method while the task is still running will reset the timestamp to the current time.
        /// </summary>
        private void StartLocationValidation()
        {
            if (_validationTask!= null && !_validationTask.IsCompleted)
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

            var result =await FreshIOC.Container.Resolve<IDataService>().ValidateLocation(locationData);

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
    }
}