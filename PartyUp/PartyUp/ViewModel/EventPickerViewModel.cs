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
        private Event _selectedEvent = null;
        public ObservableCollection<Event> Events => MvvmNanoIoC.Resolve<ICacheService>().GetEvents();

        public Event SelectedEvent
        {
            get { return _selectedEvent; }
            set
            {
                if (value != null)
                {
                    _selectedEvent = value;
                    NavigateTo<EventViewModel, Event>(value);
                    _selectedEvent = null;
                    NotifyPropertyChanged(nameof(SelectedEvent));
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
