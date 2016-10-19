using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using PartyUp.Model.Enum;
using PartyUp.Model.Model;
using PartyUp.Service.Interface;

namespace PartyUp.Service.Service
{
    public class CacheService : ICacheService, INotifyPropertyChanged
    {
        private readonly IClientService _clientService;

        public CacheService(IClientService clientService)
        {
            _clientService = clientService;
        }

        public ObservableCollection<Party> GetPartys()
        { 
            var result = new ObservableCollection<Party>();
            for (int i = 0; i < 9; i++)
            {
                result.Add(new Party
                {
                    Name = "Test event " + (i+1) + " !"
                });
            }
            return result;
        }

        public ObservableCollection<Party> Partys { get; } = new ObservableCollection<Party>();

        public async Task RefreshPartys()
        {
            //Get cached data!
            IEnumerable<Party> cached = null;
            var syncResult = await _clientService.SendRequest<IEnumerable<Party>>("Party", RestType.Get); 
            //If synced data is success override the old cache
            if (syncResult != null)
            {
                Partys.Clear();
                foreach (Party party in syncResult)
                {
                    Partys.Add(party);
                }
            }
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}