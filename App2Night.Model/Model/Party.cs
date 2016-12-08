using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using App2Night.Model.Enum;
using App2Night.Model.Properties;
using Newtonsoft.Json;
using PropertyChanged;
using Xamarin.Forms;

namespace App2Night.Model.Model
{
    [ImplementPropertyChanged]
    public class Party : INotifyPropertyChanged
    {
        [JsonProperty(PropertyName = "PartyId")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "PartyName")]
        public string Name { get; set; } 
        [JsonProperty(PropertyName = "PartyDate")]
        public DateTime Date { get; set; }
        public DateTime CreationDateTime { get; set; }
        public MusicGenre MusicGenre { get; set; }
        public Location Location { get; set; }
        public PartyType PartyType { get; set; }
        public string Description { get; set; }
        public int Price { get; set; } 
        public Host Host { get; set; }
        public bool HostedByUser { get; set; }
        [JsonProperty(PropertyName = "CommittedUser")]
        public List<Participant> Participants { get; set; }

        [JsonProperty(PropertyName = "UserCommitmentState")]
        public PartyCommitmentState CommitmentState { get; set; }
        [JsonIgnore]
        public double DistanceToParty { get; set; } = -1;

        public string ImageSource { get; set; }

        [JsonIgnore]
        public Coordinates Coordinates => new Coordinates()
        {
            Latitude = Location!=null ?  Location.Latitude : 0,
            Longitude = Location != null ? Location.Longitude : 0
        };

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Party()
        {
            ImageSource = "dummy.png";
        }
    }
}
