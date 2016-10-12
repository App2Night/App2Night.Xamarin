using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PartyUp.Model;

namespace PartyUp.Service.Interface
{
    public interface ICacheService
    {
        ObservableCollection<Event> GetEvents();
    }
}