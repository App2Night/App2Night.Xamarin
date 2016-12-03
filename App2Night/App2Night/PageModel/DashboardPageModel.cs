using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using App2Night.CustomView.Template;
using App2Night.Model.Enum;
using App2Night.Model.Model;
using App2Night.PageModel.SubPages;
using App2Night.Service.Interface;
using FreshMvvm;
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
        public Command<PartyCommitmentParameter> PartyCommitmentStateChangedCommand => new Command<PartyCommitmentParameter>(async  (parameter) => await CommitmentStateChanged(parameter));

        private async Task CommitmentStateChanged(PartyCommitmentParameter parameter)
        {
            await _dataService.ChangeCommitmentState(parameter.Party.Id, parameter.CommitmentState);
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

        public async Task OpenMore(Party party)
        {
            await CoreMethods.PushPageModel<PartyDetailViewModel>(party);
        }

        void SetAvailabilitys()
        {
            InterestingPartieAvailable = InterestingPartiesForUser.Any();
            PartyHistoryAvailable = PartyHistory.Any();
            SelectedpartiesAvailable = Selectedparties.Any(); 
        }
    }
}