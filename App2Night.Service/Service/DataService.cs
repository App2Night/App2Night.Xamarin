using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using App2Night.Model.Model;
using App2Night.Service.Interface;
using PartyUp.Model.Enum; 
using PartyUp.Model.Model;
using PartyUp.Service.Interface;
using PartyUp.Service.Service;

namespace App2Night.Service.Service
{
    public class DataService : IDataService 
    {
        private Token _token;
        private readonly IClientService _clientService;

        public DataService(IClientService clientService)
        {
            _clientService = clientService;
        }

        public ObservableCollection<Party> Partys { get; } = new ObservableCollection<Party>();

        public Task WipeData()
        {
            //TODO Wipe all data from storage
            throw new NotImplementedException();
        }

        //TODO return real data here 
        private User _user = new User
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

        public User User => _user; 

        public Task<Result<Party>> CreateParty()
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteParty()
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateParty()
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateUser()
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteAccount()
        {
            throw new NotImplementedException();
        }

        public async Task<Result<Token>>  RequestToken(string username, string password)
        {
            var result = await _clientService.GetToken(username, password);
            if (result.Success)
            {
                _token = result.Data;
            }
            return result;
        }

        public Task<Result> RequestNewPasswort(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<Result>  RefreshPartys()
        {
            var syncResult =
                await _clientService.SendRequest<IEnumerable<Party>>("Party", RestType.Get, token: _token?.AccessToken);
            //Check if the request was a success
            if (syncResult.Success)
            {
                //TODO cache data

                //Populate Partys with new data.
                Partys.Clear();
                PopulatePartys(syncResult.Data);
            }
            else
            {
                var dummyList = new List<Party>();
                for (int i = 0; i < 10; i++)
                {
                    dummyList.Add(new Party
                    {
                        Name = "Test Party" + (i+1),
                        Date = DateTime.Today.AddDays(i).AddMonths(i) 
                    });
                }
                //TODO Replace with real caching
                if (dummyList.Count > 0)
                {
                    syncResult.Data = dummyList;
                    syncResult.IsCached = true;
                    PopulatePartys(syncResult.Data);
                }
            }
            return syncResult;
        }

        void PopulatePartys(IEnumerable<Party> parties)
        {
            foreach (Party party in parties)
            {
                Partys.Add(party);
            }
        }
    }
}