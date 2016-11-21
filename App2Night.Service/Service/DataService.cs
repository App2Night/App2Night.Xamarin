using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using App2Night.Model.Enum;
using App2Night.Model.HttpModel;
using App2Night.Model.Model;
using App2Night.Service.Interface;
using Newtonsoft.Json.Linq;

namespace App2Night.Service.Service
{
    public class DataService : IDataService 
    {
        private Token _token;
        private readonly IClientService _clientService;
        private readonly IStorageService _storageService;

        public DataService(IClientService clientService, IStorageService storageService)
        {
            _clientService = clientService;
            _storageService = storageService;
        }

        public ObservableCollection<Party> InterestingPartys { get; } = new ObservableCollection<Party>();
        public ObservableCollection<Party> SelectedPartys { get; } = new ObservableCollection<Party>();
        public ObservableCollection<Party> PartyHistory { get; } = new ObservableCollection<Party>();

        public event EventHandler PartiesUpdated;
		public event EventHandler UserUpdated;

        public async Task<bool> SetToken(Token token)
        {
            _token = token;
            var tokenValid = await CheckIfTokenIsValid();

            //Assume that the token is valid.
            return tokenValid;
        }

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
            var tokenRequestObject = new JObject();
            tokenRequestObject.Add("client_id", "nativeApp");
            tokenRequestObject.Add("client_secret", "secret"); 
            tokenRequestObject.Add("grant_type","password");
            tokenRequestObject.Add("username", username);
            tokenRequestObject.Add("password", password);
            tokenRequestObject.Add("scope", "App2NightAPI offline_access");
            tokenRequestObject.Add("offline_access", "true");
            var result =
                    await
                        _clientService.SendRequest<Token>("/connect/token", RestType.Post,
                            wwwFormData: tokenRequestObject, endpoint:Endpoint.User, enableHttps: false);

            if (result.Success)
            {
                _token = result.Data;
                _token.LastRefresh = DateTime.Now;
                //Save the token to the storage
                await _storageService.SaveStorage(new Storage
                {
                    Token = _token
                });
            }
            return result;
        }

        public async Task<Result> RefreshToken()
        {
            if (_token != null)
            {
                //Create object
                var tokenRefreshObject = new JObject();
                tokenRefreshObject.Add("client_id", "nativeApp");
                tokenRefreshObject.Add("client_secret", "secret");
                tokenRefreshObject.Add("token", _token.RefreshToken);
                tokenRefreshObject.Add("token_type_hint", "refresh_token");

                var result =
                    await
                        _clientService.SendRequest<Token>("connect/revocation", RestType.Post,
                            wwwFormData: tokenRefreshObject, enableHttps: false, endpoint: Endpoint.User, token: _token.AccessToken);

                if (result.Success)
                {
                    _token.LastRefresh = DateTime.Now; 
                } 
                return result;
            }
            return new Result
            {
                Success = false
            };
        }

        public Task<Result> RequestNewPasswort(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<IEnumerable<Party>>>  RefreshPartys()
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

        async Task<bool> CheckIfTokenIsValid()
        {
            if (_token == null) return false;
            if (_token.ExpirationDate > DateTime.Now) return true;
            //Try to refreh token
            var result = await RefreshToken();
            return result.Success;
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