using System;
using System.Collections.ObjectModel;
using System.Linq;
using App2Night.Model.Model;
using App2Night.PageModel.SubPages;
using App2Night.Service.Interface;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.PageModel
{
    public class DashboardPageModel : FreshBasePageModel
    {
        private readonly IDataService _dataService;
        public bool InterestingPartieAvailable { get; set; }
        public bool PartyHistoryAvailable { get; set; }
        public bool SelectedpartiesAvailable { get; set; } 


        public ObservableCollection<Party> InterestingPartiesForUser  => FreshIOC.Container.Resolve<IDataService>().InterestingPartys;
        public ObservableCollection<Party> PartyHistory => FreshIOC.Container.Resolve<IDataService>().PartyHistory;
        public ObservableCollection<Party> Selectedparties => FreshIOC.Container.Resolve<IDataService>().SelectedPartys;

        public Command MoveToMyPartiesCommand => new Command(async () => await CoreMethods.PushPageModel<MyPartysViewModel>());
        public Command MoveToHistoryCommand => new Command(async () => await CoreMethods.PushPageModel<HistoryViewModel>());
        public Command MoveToPartyPicker => new Command(async () => await CoreMethods.PushPageModel<PartyPickerViewModel>());

        public DashboardPageModel(IDataService dataService) : base()
        {
            _dataService = dataService;
            _dataService.PartiesUpdated += OnPartiesUpdated;
            User = _dataService.User;
        }

        private void OnNearPartiesUpdated(object sender, EventArgs eventArgs)
        {
            SetAvailabilitys();
        }

        public Model.Model.User User { get; set; }

        void SetAvailabilitys()
        {
            InterestingPartieAvailable = InterestingPartiesForUser.Any();
            PartyHistoryAvailable = PartyHistory.Any();
            SelectedpartiesAvailable = Selectedparties.Any(); 
        }
    }
}