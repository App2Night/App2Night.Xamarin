using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using App2Night.Model.Enum;
using App2Night.Model.Properties;
using Newtonsoft.Json;
using PropertyChanged;
using SQLite.Net.Attributes;
using Xamarin.Forms;

namespace App2Night.Model.Model
{
    [ImplementPropertyChanged]
    public class Party : INotifyPropertyChanged
    {
        //Table id
        [PrimaryKey, AutoIncrement]
        public int IdForTable { get; set; }

        [JsonProperty(PropertyName = "PartyId")] 
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "PartyName")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "PartyDate")]
        public DateTime Date { get; set; }

        public DateTime CreationDateTime { get; set; }

        public MusicGenre MusicGenre { get; set; } 

        public int LocationId { get; set; }

        [Ignore]
        public Location Location { get; set; }

        public PartyType PartyType { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public int HostId { get; set; }

        [Ignore]
        public Host Host { get; set; }

        public bool HostedByUser { get; set; }

        [JsonProperty(PropertyName = "CommittedUser")]
        [Ignore]
        public List<Participant> Participants { get; set; }

        public PartyListType PartyListType { get; set; }

        [JsonProperty(PropertyName = "UserCommitmentState")]
        public PartyCommitmentState CommitmentState { get; set; }

        [JsonIgnore]
        public double DistanceToParty { get; set; } = -1;

        public string ImageSource { get; set; }

        public bool IsCached { get; set; }

        public bool CommitmentStatePending { get; set; }

        [JsonIgnore]
        public Coordinates Coordinates => new Coordinates()
        {
            Latitude = Location!=null ?  Location.Latitude : 0,
            Longitude = Location != null ? Location.Longitude : 0
        };

        [JsonIgnore]
        public string AdressFormatted
        {
            get
            {
                var text = string.Empty;

                if (Location != null)
                {
                    text += $"{Location.CityName}({Location.Zipcode}),\n";
                    text += Location.StreetName + " " + Location.HouseNumber;
                }

                return text;
            }
        }

        public int GeneralUpVoting { get; set; }
        public int GeneralDownVoting { get; set; }
        [JsonIgnore]
        public double GeneralAvg => CalculateAvg(GeneralUpVoting, GeneralDownVoting);
        public int PriceUpVoting { get; set; }
        public int PriceDownVoting { get; set; }
        [JsonIgnore]
        public double PriceAvg => CalculateAvg(PriceUpVoting, PriceDownVoting);
        public int LocationUpVoting { get; set; }
        public int LocationDownVoting { get; set; }
        [JsonIgnore]
        public double LocationAvg => CalculateAvg(LocationUpVoting, LocationDownVoting);
        public int MoodUpVoting { get; set; }
        public int MoodDownVoting { get; set; }
        [JsonIgnore]
        public double MoodAvg => CalculateAvg(MoodUpVoting, MoodDownVoting);
        /// <summary>
        /// Calculates average of two digits. If last digit if 0, returns 1.
        /// </summary>
        /// <param name="up"></param>
        /// <param name="down"></param>
        /// <returns></returns>
        private double CalculateAvg(int up, int down)
        {
            return (down != 0) ? Math.Round(((double)up / down), 2) : 1;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Party()
        {
            //Backend did not provide pictures but since pictures are important for the app experience we added dummy pictures.
            ImageSource = "dummy.png";
        }
    }
}
