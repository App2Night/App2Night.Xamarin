using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using App2Night.Model.Enum;
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


        public ObservableCollection<Party> InterestingPartiesForUser  => FreshIOC.Container.Resolve<IDataService>().InterestingPartys;
        public ObservableCollection<Party> PartyHistory => FreshIOC.Container.Resolve<IDataService>().PartyHistory;
        public ObservableCollection<Party> Selectedparties => FreshIOC.Container.Resolve<IDataService>().SelectedPartys;

        public IList<Pin> MapPins { get; private set; }


        public Command MoveToMyPartiesCommand => new Command(async () => await CoreMethods.PushPageModel<MyPartysViewModel>());
        public Command MoveToHistoryCommand => new Command(async () => await CoreMethods.PushPageModel<HistoryViewModel>());
        public Command MoveToPartyPicker => new Command(async () => await CoreMethods.PushPageModel<PartyPickerViewModel>());
        public Command<Party> PartyCommitmentStateChangedCommand => new Command<Party>(async  (party) => await CommitmentStateChanged(party));

        private async Task CommitmentStateChanged(Party party)
        {
            PartyCommitmentState nextState = PartyCommitmentState.Noted;

            //Find the next logical commitment state
            if (party.CommitmentState == PartyCommitmentState.Rejected)
                nextState = PartyCommitmentState.Noted;

            if (party.CommitmentState == PartyCommitmentState.Noted)
                nextState = PartyCommitmentState.Accepted;

            if (party.CommitmentState == PartyCommitmentState.Accepted)
                nextState = PartyCommitmentState.Rejected;

            await _dataService.ChangeCommitmentState(party.Id, nextState);
        }

        public DashboardPageModel(IDataService dataService) : base()
        {
            _dataService = dataService;
            _dataService.NearPartiesUpdated += OnNearPartiesUpdated;
        }

        private void OnNearPartiesUpdated(object sender, EventArgs eventArgs)
        {
            SetAvailabilitys();
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

        void SetAvailabilitys()
        {
            InterestingPartieAvailable = InterestingPartiesForUser.Any();
            PartyHistoryAvailable = PartyHistory.Any();
            SelectedpartiesAvailable = Selectedparties.Any(); 
        }
    }
}