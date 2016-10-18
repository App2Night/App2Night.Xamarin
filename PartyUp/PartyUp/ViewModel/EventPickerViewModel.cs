using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using MvvmNano;
using PartyUp.Model;
using PartyUp.Model.Model;
using PartyUp.Service.Interface;

namespace PartyUp.ViewModel
{
    public class EventPickerViewModel : MvvmNanoViewModel
    {
        private Party _selectedParty = null;
        public ObservableCollection<Party> Events => MvvmNanoIoC.Resolve<ICacheService>().GetEvents();

        public Party SelectedParty
        {
            get { return _selectedParty; }
            set
            {
                if (value != null)
                {
                    _selectedParty = value;
                    NavigateTo<EventViewModel, Party>(value);
                    _selectedParty = null;
                    NotifyPropertyChanged(nameof(SelectedParty));
                }
            }
        }

        public EventPickerViewModel()
        {
            
        }
    }

    public class User
    {
        public string Name { get; set; }
    }
}
