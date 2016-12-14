using System;
using System.Diagnostics;
using System.Linq;
using Acr.UserDialogs;
using App2Night.CustomView.View;
using App2Night.Model.Enum;
using App2Night.Model.Model;
using App2Night.Service.Interface;
using FreshMvvm;
using PropertyChanged;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App2Night.PageModel.SubPages
{
    [ImplementPropertyChanged]
    public class PartyDetailViewModel : FreshBasePageModel
    {
        private readonly IDataService _dataService;

        [AlsoNotifyFor(nameof(ValidRate))]
        public Party Party { get; set; }

        public string Name => Party.Name;
        public string Description => Party.Description;
        public MusicGenre MusicGenre => Party.MusicGenre;
        public DateTime Date => Party.Date;
        public PartyType PartyType => Party.PartyType;
        public DateTime CreationDateTime => Party.CreationDateTime;
        public Location Location => Party.Location;

        public bool ParticipantsVisible { get; set; }

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

        public PartyDetailViewModel(IDataService dataService)
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

        public bool ValidRate => ValidateRating();

        public bool ValidateRating()
        {
            if (Party.CommitmentState == PartyCommitmentState.Rejected) //We can only rate when we noted or accepted a party
            {
                return false;
            }

            if (Party.Date.Date == DateTime.Today.Date) return true; //We can rate on the same day
            if (Party.Date.AddDays(1).Date == DateTime.Today.Date) return true; //We can rate the next day
            return false;
        } 
    }
}