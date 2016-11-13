using System;
using App2Night.Model.Model;
using Newtonsoft.Json;
using PartyUp.Model.Enum;

namespace PartyUp.Model.Model
{
    public class Party
    {
        [JsonProperty(PropertyName = "PartyName")]
        public string Name { get; set; }
        public EventCommitmentState MyEventCommitmentState { get; set; }
        [JsonProperty(PropertyName = "PartyDate")]
        public DateTime Date { get; set; }
        public DateTime CreationDateTime { get; set; }
        public MusicGenre MusicGenre { get; set; }
        public Location Location { get; set; }
        public PartyType PartyType { get; set; }
        public string Description { get; set; }
    }
}
