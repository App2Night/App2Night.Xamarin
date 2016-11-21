using System;
using App2Night.Model.Enum;
using App2Night.Model.Model;
using MvvmNano;

namespace App2Night.ViewModel
{
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
			set { _date = value; } 
		}

		public string StreetName
		{
			get { return _streetName;}
			set 
			{
				_streetName = value;
			}
		}

		public string LocationName
		{
			get { return _locationName; }
			set
			{
				_locationName = value;
			}
		}

		public string Zipcode
		{
			get { return _zipcode; }
			set
			{
				_zipcode = value;
			}
		}

		public string HouseNumber
		{
			get { return _houseNumber; }
			set
			{
				_houseNumber = value;
			}
		}

		public bool ValidName => ValidateName();

		bool ValidateName()
		{
			return !string.IsNullOrEmpty(Name) && Name.Length > 3;
		}

		public bool ValidDescription => ValidateDescription();

		bool ValidateDescription()
		{
			return !string.IsNullOrEmpty(Description);
		}
    }
}