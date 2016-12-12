using System;
using System.Diagnostics;
using App2Night.CustomView.View;
using App2Night.Model.Enum;
using App2Night.Model.Model;
using FreshMvvm;
using PropertyChanged;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App2Night.PageModel.SubPages
{
    [ImplementPropertyChanged]
    public class PartyDetailViewModel : FreshBasePageModel
    {
        public Party Party { get; set; }

        public string Name => Party.Name;
        public string Description => Party.Description;
        public MusicGenre MusicGenre => Party.MusicGenre;
        public DateTime Date => Party.Date;
        public PartyType PartyType => Party.PartyType;
        public DateTime CreationDateTime => Party.CreationDateTime;
        public Location Location => Party.Location;


        public bool IsMyParty => Party.HostedByUser;

        private Pin _mapPin;

        public Pin MapPins
        {
            get { return new Pin
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

        public bool ValidRate => ValidateRating();

        public bool ValidateRating()
        {
            if (Party.CommitmentState == PartyCommitmentState.Rejected)
            {
                return false;
            }

            if (Party.Date.Date == DateTime.Today.Date) return true;
            if (Party.Date.AddDays(1).Date == DateTime.Today.AddDays(1).Date) return true;
            return false;
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            Party = (Party) initData;
            Debug.WriteLine(Party.Id);
        }
    }
}