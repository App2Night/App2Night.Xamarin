using System;
using App2Night.Model.Enum;
using App2Night.Model.Model;
using FreshMvvm;
using PropertyChanged;
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

        public override void Init(object initData)
        {
            base.Init(initData);
            Party = (Party) initData;
        }
    }
}