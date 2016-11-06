using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using App2Night.Model.Model;
using App2Night.Service.Interface;
using MvvmNano;

namespace App2Night.ViewModel
{
    public class PartyPickerViewModel : MvvmNanoViewModel
    {
        private Party _selectedParty = null;
        public ObservableCollection<Party> Parties => MvvmNanoIoC.Resolve<IDataService>().InterestingPartys;


        public PartyPickerViewModel()
        {
            MvvmNanoIoC.Resolve<IDataService>().PartiesUpdated += OnPartiesUpdated;
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
                    NavigateTo<PartyViewModel, Party>(value);
                    _selectedParty = null;
                    NotifyPropertyChanged(nameof(SelectedParty));
                }
            }
        }

    }

    public class User
    {
        public string Name { get; set; }
    }
}
