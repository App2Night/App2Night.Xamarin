using System;
using System.Collections.ObjectModel;
using System.Linq;
using App2Night.Model.Model;
using App2Night.Service.Interface;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.PageModel
{
    public class PartyPickerViewModel : FreshBasePageModel
    {
        private readonly IDataService _dataService;
        private Party _selectedParty = null;
        public ObservableCollection<Party> Parties => FreshIOC.Container.Resolve<IDataService>().InterestingPartys;

        public bool NearPartyAvailable { get; private set; }

        public PartyPickerViewModel(IDataService dataService)
        {
            _dataService = dataService;
            FreshIOC.Container.Resolve<IDataService>().NearPartiesUpdated += OnNearPartiesUpdated;
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
