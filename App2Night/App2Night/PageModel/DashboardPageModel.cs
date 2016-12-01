using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        void SetAvailabilitys()
        {
            InterestingPartieAvailable = InterestingPartiesForUser.Any();
            PartyHistoryAvailable = PartyHistory.Any();
            SelectedpartiesAvailable = Selectedparties.Any(); 
        }
    }
}