using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PartyUp.Model.Enum;
using PartyUp.Model.Model;
using PartyUp.Service.Interface;

namespace PartyUp.Service.Service
{
    public class ClientService : IClientService
    {
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
                Events = new ObservableCollection<Party>
                {
                    new Party
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

        public Party GetParty()
        {
            return new Party
            {
                MusicGenre = MusicGenre.Pop,
                Name = "DH goes Ballermann",
                MyEventCommitmentState = EventCommitmentState.Accepted,
                CreationDateTime = DateTime.Today,
                Date = DateTime.Today.AddDays(40)
            };
        }

        public IEnumerable<Party> GetParties(MusicGenre musicGenre = MusicGenre.All, float distanceFromPosition = 30000,
            Coordinates coordinates = null)
        {
            var result = new ObservableCollection<Party>();
            for (int i = 0; i < 9; i++)
            {
                result.Add(new Party
                {
                    Name = "Test event " + (i + 1) + " !"
                });
            }
            return result;
        }
    }
}