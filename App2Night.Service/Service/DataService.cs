using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using App2Night.Model.Enum;
using App2Night.Model.HttpModel;
using App2Night.Model.Model;
using App2Night.Service.Interface;

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

        public ObservableCollection<Party> InterestingPartys { get; } = new ObservableCollection<Party>();
        public ObservableCollection<Party> SelectedPartys { get; } = new ObservableCollection<Party>();
        public ObservableCollection<Party> PartyHistory { get; } = new ObservableCollection<Party>();

        public event EventHandler PartiesUpdated;
		public event EventHandler UserUpdated;

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

		public User User 
		{ 
			get { return _user; } 
			set 
			{ 
				_user = value;
				UserUpdated?.Invoke(this, EventArgs.Empty);
			} 
		}

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

        public async Task<Result> CreateUser(SignUp signUpModels)
        {
            var creationResult = await _clientService.SendRequest("user", RestType.Post, bodyParameter: signUpModels, endpoint: Endpoint.User);
            
            //Login user if creation was successfull
            if (creationResult.Success)
            {
                var loginResult = await RequestToken(signUpModels.Username, signUpModels.Password);
                //TODO Handle what should happen if login request fails
            }

            return creationResult;
        }

        public async Task<Result>  RequestToken(string username, string password)
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
            }
            else
            {
                var cachedData = new List<Party>();
                for (int i = 0; i < 10; i++)
                {
                    cachedData.Add(new Party
                    {
                        Name = "Cached dummy Party bla bla bla bla bla bla party party party" + (i + 1),
                        Date = DateTime.Today.AddDays(i).AddMonths(i)
                    });
                }
                //TODO Replace with real caching
                if (cachedData.Count > 0)
                {
                    syncResult.Data = cachedData; 
                    syncResult.IsCached = true; 
                }
            }
            if (syncResult.Data != null)
            { 
                PopulateObservableCollection(InterestingPartys, syncResult.Data.OrderBy(o => o.Date).Take(5).Where(o => o.Date >= DateTime.Today));
                PopulateObservableCollection(SelectedPartys, syncResult.Data.OrderBy(o => o.Date).Take(5).Where(o => o.Date >= DateTime.Today)); 
                PopulateObservableCollection(PartyHistory, syncResult.Data.OrderBy(o => o.Date).Take(5).Where(o => o.Date < DateTime.Today));
            } 
            PartiesUpdated?.Invoke(this, EventArgs.Empty);
            return syncResult;
        }

        void PopulateObservableCollection<TObservable>(TObservable collection, IEnumerable<Party> newObjects) where TObservable : ObservableCollection<Party>
        {
            collection.Clear();
            foreach (Party party in newObjects)
            {
                collection.Add(party);
            }
        } 
    }
}