using System;
using PartyUp.Model.Enum;

namespace PartyUp.Model.Model
{
    public class Party
    {
        public string Name { get; set; }
        public EventCommitmentState MyEventCommitmentState { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreationDateTime { get; set; }
        public MusicGenre MusicGenre { get; set; } 
    }
}
