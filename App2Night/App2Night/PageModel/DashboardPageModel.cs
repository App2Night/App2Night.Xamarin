using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using App2Night.Model.Enum;
using App2Night.Model.Language;
using App2Night.Model.Model;
using App2Night.PageModel.SubPages;
using App2Night.Service.Interface;
using FreshMvvm;
using Plugin.Share;
using PropertyChanged;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App2Night.PageModel
{
    [ImplementPropertyChanged]
    public class DashboardPageModel : FreshBasePageModel
    {
        private readonly IDataService _dataService;
        public bool InterestingPartieAvailable { get; set; }
        public bool PartyHistoryAvailable { get; set; }
        public bool SelectedpartiesAvailable { get; set; }   

        public IList<Party> InterestingPartiesForUser { get; }
        public IList<Party> PartyHistory { get; }
        public IList<Party> Selectedparties { get; }

        public IList<Pin> MapPins { get; private set; }


        public Command MoveToMyPartiesCommand => new Command(async () => await CoreMethods.PushPageModel<MyPartysViewModel>());
        public Command MoveToHistoryCommand => new Command(async () => await CoreMethods.PushPageModel<HistoryViewModel>());
        public Command MoveToPartyPicker => new Command(async () => await CoreMethods.PushPageModel<PartyPickerViewModel>());
        public Command<Party> PartyCommitmentStateChangedCommand => new Command<Party>(async  (party) => await CommitmentStateChanged(party));

        public Command ReloadCommand => new Command(async ()=> await Reload());

        private async Task Reload()
        {
            using (UserDialogs.Instance.Loading(AppResources.ReloadData))
            {
                var result = await _dataService.BatchRefresh(); 
            }
        }

        private async Task CommitmentStateChanged(Party party)
        {

            var message = string.Empty;
            PartyCommitmentState nextState = PartyCommitmentState.Noted;

            //Find the next logical commitment state
            if (party.CommitmentState == PartyCommitmentState.Rejected)
            {
                nextState = PartyCommitmentState.Noted;
                message = AppResources.NotingParty;
            }


            if (party.CommitmentState == PartyCommitmentState.Noted)
            {
                nextState = PartyCommitmentState.Accepted;
                message = AppResources.AcceptingParty;
            }

            if (party.CommitmentState == PartyCommitmentState.Accepted)
            {
                nextState = PartyCommitmentState.Rejected;
                message = AppResources.RejectingParty;
            }

            using (UserDialogs.Instance.Loading(message))
            {
                party.CommitmentStatePending = true;
                await _dataService.ChangeCommitmentState(party.Id, nextState);
                party.CommitmentStatePending = false; 
            }
        }

        public DashboardPageModel(IDataService dataService) : base()
        {
            _dataService = dataService;

            //Populate list views.

            InterestingPartiesForUser = _dataService.InterestingPartys;

            Selectedparties = _dataService.SelectedPartys;

            PartyHistory = _dataService.PartyHistory; 

            //Subscribe to party change events.

            _dataService.NearPartiesUpdated += OnNearPartiesUpdated;

            _dataService.SelectedPartiesUpdated += OnSelectedPartiesUpdated;

            _dataService.HistoryPartisUpdated += OnHistoryPartiesUpdated;
        }

        private void OnHistoryPartiesUpdated(object sender, EventArgs eventArgs)
        {
            PartyHistoryAvailable = _dataService.PartyHistory.Any();
            RaisePropertyChanged(nameof(PartyHistory));
        }

        private void OnSelectedPartiesUpdated(object sender, EventArgs eventArgs)
        {
            SelectedpartiesAvailable = _dataService.SelectedPartys.Any();
            RaisePropertyChanged(nameof(Selectedparties));

        }

        private void OnNearPartiesUpdated(object sender, EventArgs eventArgs)
        {
            InterestingPartieAvailable = _dataService.InterestingPartys.Any();
            RaisePropertyChanged(nameof(InterestingPartiesForUser));

            CreatePinsForNearParties();
        }

        private void CreatePinsForNearParties()
        {
            var list = new List<Pin>();
            foreach (Party nearParty in _dataService.InterestingPartys)
            {
                list.Add(new Pin
                {
                    Label = nearParty.Name,
                    Position = new Position(nearParty.Location.Latitude, nearParty.Location.Longitude)
                });
            }
            MapPins = list;
        }

        public async Task ShareParty(Party party)
        {
            await
                CrossShare.Current.Share($"Hey, wanna join the {party.Name} party?",
                    $"{party.Name} is on {party.Date.ToString("d")}");
        }


        public void OpenMore(Party party)
        {
            if (party.HostedByUser)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var page = FreshPageModelResolver.ResolvePageModel<MyPartyDetailViewModel>(party);
                    await App._masterDetailNav.PushPage(page, null);
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var page = FreshPageModelResolver.ResolvePageModel<PartyDetailViewModel>(party);
                    await App._masterDetailNav.PushPage(page, null);
                });
            }
        } 
    }
}