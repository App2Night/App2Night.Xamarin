using System;
using System.Collections.ObjectModel;
using App2Night.Model.Model;
using App2Night.Service.Interface;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.PageModel
{
    public class PartyPickerViewModel : FreshBasePageModel
    {
        private Party _selectedParty = null;
        public ObservableCollection<Party> Parties => FreshIOC.Container.Resolve<IDataService>().InterestingPartys;


        public PartyPickerViewModel()
        {
            FreshIOC.Container.Resolve<IDataService>().PartiesUpdated += OnPartiesUpdated;
        }

        private void OnPartiesUpdated(object sender, EventArgs eventArgs)
        {
             
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
