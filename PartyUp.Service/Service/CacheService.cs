using System.Collections.Generic;
using System.Collections.ObjectModel;
using PartyUp.Model;
using PartyUp.Service.Interface;

namespace PartyUp.Service.Service
{
    public class CacheService : ICacheService
    {
        public ObservableCollection<Event> GetEvents()
        {
            var result = new ObservableCollection<Event>();
            for (int i = 0; i < 9; i++)
            {
                result.Add(new Event
                {
                    Name = "Test event " + (i+1) + " !"
                });
            }
            return result;
        }
    }
}