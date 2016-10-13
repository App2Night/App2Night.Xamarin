using System;
using System.Collections.ObjectModel;
using PartyUp.Model.Enum;
using PartyUp.Model.Model;
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

        public User GetUser()
        {
            return new User
            {
                Name = "Hardy",
                Addresse = new Location(),
                Gender = Gender.Unkown,
                Age = 21,
                Email = "hardy@party.de",
                LastGpsLocation = new Location(),
                Events = new ObservableCollection<Event>
                {
                    new Event
                    {
                        MusicGenre = MusicGenre.Pop,
                        Name = "DH goes Ballermann",
                        MyEventCommitmentState = EventCommitmentState.Accepted,
                        CreationDateTime = DateTime.Today,
                        Date = DateTime.Today.AddDays(40)
                    }
                }

            };
        }
    }
}