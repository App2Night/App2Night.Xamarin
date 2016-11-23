using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using App2Night.Model.Enum;
using App2Night.Model.Model;
using App2Night.Service;
using App2Night.Service.Interface;
using MvvmNano;
using PropertyChanged;

namespace App2Night.ViewModel
{
    [ImplementPropertyChanged]
    public class CreatePartyViewModel : MvvmNanoViewModel
    {
		string _name;
		string _description;
		MusicGenre _musicGenre;
		DateTime _time;
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

		public DateTime Time 
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

        DateTime _lastLocationChange = new DateTime(); 

        private void StartLocationValidation()
        {  
            //Task.Run(async () =>
            //{
            //    try
            //    {
            //        var timestamp = DateTime.Now;
            //        _lastLocationChange = timestamp;
            //        //Wait for another user input that cancels this task
            //        await Task.Delay(300);
            //        //Check if this was the last user input
            //        if (timestamp == _lastLocationChange)
            //        {
            //            //Start location check
            //            await CheckLocation();
            //        } 
            //    }
            //    catch(Exception)
            //    {
            //        // ignored
            //    }
            //});  
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
            var result =await MvvmNanoIoC.Resolve<IClientService>()
                .SendRequest<Location>("/api/Party/validate", RestType.Post, bodyParameter: locationData);

            if (result.Success)
            {
                var resLocation = result.Data;
                ValidCityname = resLocation.CityName == CityName;
                ValidZipcode= resLocation.Zipcode == Zipcode;
                ValidStreetname = resLocation.StreetName == StreetName;
                ValidHousenumber = resLocation.HouseNumber == HouseNumber;
            }
        }
    }
}