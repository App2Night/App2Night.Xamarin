using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using App2Night.Model.Enum;
using App2Night.Model.Model;
using App2Night.Service.Interface;
using FreshMvvm;
using PropertyChanged;
using Xamarin.Forms;

namespace App2Night.PageModel
{
    [ImplementPropertyChanged]
    public class PartyPickerViewModel : FreshBasePageModel
    {
        private readonly IDataService _dataService;
        private readonly IAlertService _alertService;

        public bool IsRefreshing { get; set; }

        private Party _selectedParty = null;

        public ObservableCollection<Party> Parties => FreshIOC.Container.Resolve<IDataService>().InterestingPartys; 

        public Command RefreshCommand => new Command(async ()=> await Refresh()); 

        public Command<object> NotePartyCommand => new Command<object>(async(o)=> await NoteParty((Party) o)); 
        
        public Command<object> AcceptPartyCommand => new Command<object>(async (o) => await AcceptParty((Party)o)); 

        public bool NearPartyAvailable { get; private set; }

        public PartyPickerViewModel(IDataService dataService, IAlertService alertService)
        {
            _dataService = dataService;
            _alertService = alertService;
            FreshIOC.Container.Resolve<IDataService>().NearPartiesUpdated += OnNearPartiesUpdated;
        }

        private async Task NoteParty(Party party)
        {
            await ChangeCommitmentState(party, PartyCommitmentState.Noted);
        }

        private async Task AcceptParty(Party party)
        {
            await ChangeCommitmentState(party, PartyCommitmentState.Accepted);
        }

        private async Task ChangeCommitmentState(Party party, PartyCommitmentState commitmentState)
        {
            using (UserDialogs.Instance.Loading(commitmentState + " party")) //RESOURCE 
            {
                var result = await _dataService.ChangeCommitmentState(party.Id, commitmentState);

                _alertService.CommitmentStateChangedAlert(commitmentState, result.Success);
            }
        }

        private async Task Refresh()
        {
            var result = await _dataService.RequestPartyWithFilter();
            _alertService.PartyPullFinished(result);
            IsRefreshing = false;
        }

        private void OnNearPartiesUpdated(object sender, EventArgs eventArgs)
        {
            NearPartyAvailable = _dataService.InterestingPartys.Any();
        } 

        public Party SelectedParty
        {
            get { return _selectedParty; }
            set
            {
                if (value != null)
                {
                    _selectedParty = value;
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await CoreMethods.PushPageModel<PartyViewModel>(_selectedParty);
                        _selectedParty = null;
                    }); 
                   
                }
            }
        }

    }

    public class User
    {
        public string Name { get; set; }
    }
}
