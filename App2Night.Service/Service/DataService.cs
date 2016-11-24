using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using App2Night.Model.Enum;
using App2Night.Model.HttpModel;
using App2Night.Model.Model;
using App2Night.Service.Helper;
using App2Night.Service.Interface;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Plugin.Geolocator;

namespace App2Night.Service.Service
{
    public class DataService : IDataService 
    {
        private Token Token
        {
            get { return _storageService.Storage.Token; }
            set { _storageService.Storage.Token = value; }
        }

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
            Token = token;
            //Cant check if the token is valid if user is offline, assume that the token is valid.
            //User will get logged out if the next request getsends
            //TODO add automatic token check when user gets online
            if (!CrossConnectivity.Current.IsConnected) return true;
            var tokenValid = await CheckIfTokenIsValid();

            //Assume that the token is valid.
            return tokenValid;
        }

        public async Task<Result<Location>> ValidateLocation(Location location)
        {
            var tokenValid = await CheckIfTokenIsValid();
            if (tokenValid)
            {
               return await _clientService.SendRequest<Location>("/api/Party/validate", RestType.Post,
                    bodyParameter: location, token: _storageService.Storage.Token.AccessToken);
            }
            else
            {
                return new Result<Location>
                {
                    Message = "Token not valid."
                };
            }
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

		public async Task<Result<Party>> CreateParty(string name, DateTime date, MusicGenre genre, string country, string cityName, string street, string houseNr, string zipcode, PartyType type, string description)
		{
		    dynamic partyCreationObject = new ExpandoObject(); 
		    partyCreationObject.partyName = name;

		    partyCreationObject.partyDate = date;

                 partyCreationObject.musicGenre=(int) genre;

                 partyCreationObject.countryName= country;

            partyCreationObject.cityName=cityName;

            partyCreationObject.streetName= street;

            partyCreationObject.houseNumber= houseNr;

            partyCreationObject.zipcode= zipcode;

            partyCreationObject.partyType=(int) type;

            partyCreationObject.description= description;
       
		    var result =
		        await
		            _clientService.SendRequest<Guid>("api/party", RestType.Post, bodyParameter: partyCreationObject,
		                token: Token.AccessToken);

		    if (!result.Success) return new Result<Party>
		    {
		        RequestFailedToException = result.RequestFailedToException,
                StatusCode =result.StatusCode
		    };

		    var party = await GetParty(result.Data);

		    return party;

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
            var creationResult = await _clientService.SendRequest("api/user", RestType.Post, bodyParameter: signUpModels, endpoint: Endpoint.User);
            
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
                Token = result.Data;
                Token.LastRefresh = DateTime.Now;
                //Save the token to the storage
                await _storageService.SaveStorage();
            }
            return result;
        }

        public async Task<Result> RefreshToken()
        {
            if (Token != null)
            {
                //Create object
                var tokenRefreshObject = new JObject();
                tokenRefreshObject.Add("client_id", "nativeApp");
                tokenRefreshObject.Add("client_secret", "secret");
                tokenRefreshObject.Add("token", Token.RefreshToken);
                tokenRefreshObject.Add("token_type_hint", "access_token");

                var result =
                    await
                        _clientService.SendRequest<Token>("connect/revocation", RestType.Post,
                            wwwFormData: tokenRefreshObject, enableHttps: false, endpoint: Endpoint.User, token: Token.AccessToken);

                if (result.Success)
                {
                    Token.LastRefresh = DateTime.Now;
                    await _storageService.SaveStorage();
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
            Result<IEnumerable<Party>> requestResult = new Result<IEnumerable<Party>>();

            try
            { 
                var location = await CrossGeolocator.Current.GetPositionAsync(3000);
                string lat = location.Latitude.ToString();
                lat = lat.Replace(",", ".");
                string lon = location.Longitude.ToString();
                lon = lon.Replace(",", ".");
                var radius = _storageService.Storage.FilterRadius;
                var uri = $"?lat={lat}&lon={lon}&radius={radius.ToString()}";
                requestResult =
                    await
                        _clientService.SendRequest<IEnumerable<Party>>("api/party", RestType.Get, urlQuery: uri,
                            token: Token?.AccessToken);
            }
            catch (TaskCanceledException e)
            {
                DebugHelper.PrintDebug(DebugType.Error, "Getting location in time failed.\n" + e); 
                requestResult.RequestFailedToException = true;
            }
            catch (Exception e)
            {
                DebugHelper.PrintDebug(DebugType.Error, "Fetching parties failed.\n" + e);
                requestResult.RequestFailedToException = true; 
            }

            //Check if the request was a success
            if (requestResult.Success)
            {
                //TODO cache data 
            }
            else
            {
                var cachedData = new List<Party>();
                string buf = "";
                    for (int j = 0; j < 31; j++)
                    {
                        buf += "A";
                    }
                for (int i = 0; i < 5; i++)
                {
                    
                        cachedData.Add(new Party
                        {
                            Name = buf + (i + 1),
                            Date = DateTime.Today.AddDays(i).AddMonths(i)
                        }); 
                    
                }
                //TODO Replace with real caching
                if (cachedData.Count > 0)
                {
                    requestResult.Data = cachedData;
                    requestResult.IsCached = true; 
                }
            }
            if (requestResult.Data != null)
            { 
                PopulateObservableCollection(InterestingPartys, requestResult.Data.OrderBy(o => o.Date).Take(5).Where(o => o.Date >= DateTime.Today));
                PopulateObservableCollection(SelectedPartys, requestResult.Data.OrderBy(o => o.Date).Take(5).Where(o => o.Date >= DateTime.Today)); 
                PopulateObservableCollection(PartyHistory, requestResult.Data.OrderBy(o => o.Date).Take(5).Where(o => o.Date < DateTime.Today));
            } 
            PartiesUpdated?.Invoke(this, EventArgs.Empty);
            return requestResult;
        }

        public async Task<Result<Party>> GetParty(Guid id)
        {
            var result =
                await
                    _clientService.SendRequest<Party>("api/party", RestType.Get, urlQuery: "?id=" + id,
                        token: Token.AccessToken);
            return result;
        }

        async Task<bool> CheckIfTokenIsValid()
        {
            if (Token == null) return false;
            if (Token.ExpirationDate > DateTime.Now) return true;
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