using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Acr.UserDialogs;
using App2Night.Model.Enum;
using App2Night.Model.Model;
using App2Night.Service.Helper;
using App2Night.Service.Interface;
using FreshMvvm;
using PropertyChanged;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App2Night.PageModel.SubPages
{
    [ImplementPropertyChanged]
    public class MyPartyDetailViewModel : FreshBasePageModel
    {
        public Party Party { get; set; }

        private Pin _mapPin;

        public Pin MapPins
        {
            get
            {
                return new Pin
                {
                    Position = new Position(Party.Location.Latitude, Party.Location.Longitude),
                    Label = Party.Name
                };
            }
            private set
            {
                _mapPin = new Pin
                {
                    Position = new Position(Party.Location.Latitude, Party.Location.Longitude),
                    Label = Party.Name
                };
            }
        }

        private string _name;
        private string _description;
        private MusicGenre _musicGenre;
        private TimeSpan _time;
        private DateTime _date;
        private Location _location;
        string _streetName;
        string _zipcode;
        string _houseNumber;
        private string _cityName;
        private PartyType _partyType;

        [AlsoNotifyFor(nameof(AcceptButtonEnabled))]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [AlsoNotifyFor(nameof(ValidDescription))]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        [AlsoNotifyFor(nameof(ValidPartyType))]
        public PartyType PartyType
        {
            get { return _partyType; }
            set { _partyType = value; }
        }

        public MusicGenre MusicGenre
        {
            get { return _musicGenre; }
            set { _musicGenre = value; }
        }

        public TimeSpan Time
        {
            get { return _time; }
            set { _time = value; }
        }

        [AlsoNotifyFor(nameof(ValidDate))]
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public string StreetName
        {
            get { return _streetName; }
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

        [AlsoNotifyFor(nameof(ValidPrice))]
        public string Price { get; set; }

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

        [AlsoNotifyFor(nameof(AcceptButtonEnabled))]
        public bool ValidStreetname { get; private set; }

        [AlsoNotifyFor(nameof(AcceptButtonEnabled))]
        public bool ValidCityname { get; private set; }

        [AlsoNotifyFor(nameof(AcceptButtonEnabled))]
        public bool ValidHousenumber { get; private set; }

        [AlsoNotifyFor(nameof(AcceptButtonEnabled))]
        public bool ValidZipcode { get; private set; }

        public bool AcceptButtonEnabled
        {
            get
            {
                //TODO add again after backend fixes his endpoint.
                var enabled =
                    ValidCityname
                    && ValidDescription
                    && ValidHousenumber
                    && ValidPrice
                    && ValidName
                    && ValidZipcode
                    && ValidStreetname;
                    
                return enabled;
            }
        }

        [AlsoNotifyFor(nameof(AcceptButtonEnabled))]
        public bool ValidPrice => ValidatePrice();

        [AlsoNotifyFor(nameof(AcceptButtonEnabled))]
        public bool ValidDate => ValidateDate();

        [AlsoNotifyFor(nameof(AcceptButtonEnabled))]
        public bool ValidName => ValidateName();

        [AlsoNotifyFor(nameof(AcceptButtonEnabled))]
        public bool ValidPartyType => ValidatePartyType();

        [AlsoNotifyFor(nameof(AcceptButtonEnabled))]
        public bool ValidDescription => ValidateDescription();

        public bool ParticipantsVisible { get; set; }

        public Command UpdatePartyCommand => new Command(async () => await UpdateParty());
        public Command ClearFormCommand => new Command(ClearForm);


        private IDataService _dataService;

        public MyPartyDetailViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.SelectedPartyUpdated += SelectedPartyUpdated;
        }

        private void SelectedPartyUpdated(object sender, Party party)
        {
            Party = party;
            if (Party.Participants.Any())
            {
                RaisePropertyChanged(nameof(Party.Participants));
                ParticipantsVisible = true;
            }
            Name = Party.Name;
            Description = Party.Description;
            MusicGenre = Party.MusicGenre;
            Time = Party.Date.TimeOfDay;
            Date = Party.Date;
            StreetName = Party.Location.StreetName;
            HouseNumber = Party.Location.HouseNumber;
            CityName = Party.Location.CityName;
            Zipcode = Party.Location.Zipcode;
            Price = Party.Price.ToString();
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            Party = (Party)initData;
            Debug.WriteLine(Party.Id);
            LoadPartyDetails();
        }

        private void LoadPartyDetails()
        {
            //Load more detailed infos about the party
            Device.BeginInvokeOnMainThread(async () =>
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    await _dataService.GetParty(Party.Id);
                }
            });
        } 

        private void ClearForm()
        {
            Name = Party.Name;
            Description = Party.Description;
            MusicGenre = Party.MusicGenre;
            Time = Party.Date.TimeOfDay;
            Date = Party.Date;
            StreetName = Party.Location.StreetName;
            HouseNumber = Party.Location.HouseNumber;
            CityName = Party.Location.CityName;
            Zipcode = Party.Location.Zipcode;
        }

        public bool ValidateDate()
        {
            return Date > DateTime.Now && Date <= DateTime.Today.AddMonths(12);
        }

        public bool ValidatePartyType()
        {
            return PartyType.GetHashCode() != -1;
        }

        private bool ValidatePrice()
        {
            return !string.IsNullOrEmpty(Price);
        }

        bool ValidateName()
        {
            return !string.IsNullOrEmpty(Name) && Name.Length <= 32;
        }

        bool ValidateDescription()
        {
            return !string.IsNullOrEmpty(Description) && Description.Length <= 256;
        }

        private async Task UpdateParty()
        {
            using (UserDialogs.Instance.Loading("Update Party")) //RESOURCE 
            {
                var dateTime = new DateTime(Date.Year, Date.Month, Date.Day, Time.Hours, Time.Minutes, 0);
                Party.Name = Name;
                Party.Description = Description;
                Party.Date = Date;
                Party.PartyType = PartyType;
                Party.MusicGenre = MusicGenre;
                Party.Price = int.Parse(Price);
                Party.Location.CityName = CityName;
                Party.Location.Zipcode = Zipcode;
                Party.Location.HouseNumber = HouseNumber;
                Party.Location.StreetName = StreetName;
                var result = await
                    FreshIOC.Container.Resolve<IDataService>()
                        .UpdateParty(Party);

                //if (result.Success)
                //{
                //    //The user should come to the dashboard after popping the party detail page.
                //    await CoreMethods.SwitchSelectedMaster<DashboardPageModel>();

                //    await CoreMethods.PushPageModel<PartyDetailViewModel>(result);
                //}
                //else
                //{
                //    //TODO Handle failure
                //}
            }
        }

        
        public Command DeletePartyCommand => new Command(() =>
        {
            Debug.WriteLine(Party.Id);
            UserDialogs.Instance.Confirm(new ConfirmConfig().SetMessage("Do you really want to delete your party?").SetOkText("Delete").SetAction(b =>
            {
                if (b)
                {
                    var result = _dataService.DeleteParty(Party.Id);
                }
                
            }));
        });

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

            if (result.StatusCode == (int) HttpStatusCode.NotAcceptable || result.Success)
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