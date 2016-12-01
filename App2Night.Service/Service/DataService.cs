using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
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
        /// <summary>
        /// Provides the token from the storage. 
        /// </summary>
        private Token Token
        {
            get { return _storageService.Storage.Token; }
            set { _storageService.Storage.Token = value; }
        }

        //Service references
        private readonly IClientService _clientService;
        private readonly IStorageService _storageService;

        public DataService(IClientService clientService, IStorageService storageService)
        {
            //Get the dependencys
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
            /*
             * Check if the user is online.
             * User is online: Check if the token is valid.
             * User is offline: Assume that the token is valid.
             */
            if (!CrossConnectivity.Current.IsConnected) return true;
            
            var tokenValid = await CheckIfTokenIsValid();
             
            return tokenValid;
        }

        public async Task<Result<Location>> ValidateLocation(Location location)
        {
            var tokenValid = await CheckIfTokenIsValid();

            if (tokenValid)
            {
                //Validate the given location an return the location suggested by the server.
               return await _clientService.SendRequest<Location>("/api/Party/validate", RestType.Post,
                    bodyParameter: location, token: _storageService.Storage.Token.AccessToken);
            }
            //Signal that the used token is not valid.
            return new Result<Location>
            {
                Message = "Token not valid."
            };
        }

        public Task WipeData()
        {
            //TODO Wipe all data from storage
            //Could be moved to 
            throw new NotImplementedException();
        }

        //TODO return real data here 
        private User _user = new User
        {
            Name = "Hardy",
            Addresse = new Location(),
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
                //Fire UserUpdate event since a new user is set.
				UserUpdated?.Invoke(this, EventArgs.Empty);
			} 
		}

		public async Task<Result<Party>> CreateParty(string name, DateTime date, MusicGenre genre, string country, string cityName, string street, string houseNr, string zipcode, PartyType type, string description)
        { 
            //Create an object from the given parameters for the party creation
            dynamic partyCreationObject = CreatePartyCreateObject(name, date, genre, country, cityName, street, houseNr, zipcode, type, description);

            //Send the create party request
            var result =
                await
                    _clientService.SendRequest<Guid>("api/party", RestType.Post, bodyParameter: partyCreationObject,
                        token: Token.AccessToken);
            DebugHelper.PrintDebug(DebugType.Info, $"Guid of the created party is {result.Data}");

		    if (!result.Success) return result; 

            //Get the created party if the creation was successfull. 
            var party = await GetParty(result.Data);

            //Return the created party
            return party;

        }

        /// <summary>
        /// Creates a party object for the api/party post endpoint.
        /// </summary> 
        private dynamic CreatePartyCreateObject(string name, DateTime date, MusicGenre genre, string country, string cityName, string street, string houseNr, string zipcode, PartyType type, string description)
        {
            dynamic partyCreationObject = new ExpandoObject();
            partyCreationObject.partyName = name;

            partyCreationObject.partyDate = date;

            partyCreationObject.musicGenre = (int)genre;

            partyCreationObject.countryName = country;

            partyCreationObject.cityName = cityName;

            partyCreationObject.streetName = street;

            partyCreationObject.houseNumber = houseNr;

            partyCreationObject.zipcode = zipcode;

            partyCreationObject.partyType = (int)type;

            partyCreationObject.description = description;
            return partyCreationObject;
        }

        public Task<Result> DeleteParty(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateParty()
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateUser()
        {
            UserDialogs.Instance.Toast(new ToastConfig("This Feature is not available"));
            return Task.FromResult(new Result
            {
                Success = false,
                Message = "Not possible"
            });
        }

        public Task<Result> DeleteAccount(string password)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> CreateUser(SignUp signUpModels)
        {
            try
            {
//SendKEY the create user request
                var creationResult = await _clientService.SendRequest("api/user", RestType.Post, bodyParameter: signUpModels, endpoint: Endpoint.User);
            
                //Login user after a successfull creation
                if (creationResult.Success)
                {
                    var loginResult = await RequestToken(signUpModels.Username, signUpModels.Password);
                    //TODO Handle what should happen if login request fails
                } 
                return creationResult;
            }
            catch (Exception e)
            {
                DebugHelper.PrintDebug(DebugType.Error, e.ToString());
            }
            return new Result();
        }

        public async Task<Result>  RequestToken(string username, string password)
        {
            Dictionary<string, string> tokenRequestValues = CreateLoginDictionary(username, password);

            //Request the user login
            var result =
                    await
                        _clientService.SendRequest<Token>("/connect/token", RestType.Post,
                            wwwFormData: tokenRequestValues, endpoint: Endpoint.User, enableHttps: false);

            //Save the new token to the storage
            if (result.Success)
            {
                Token = result.Data;
                Token.LastRefresh = DateTime.Now;
                
                //Save the modified storage
                await _storageService.SaveStorage();
            }
            return result;
        }

        /// <summary>
        /// Creates a dictionary containing all informations for the connnect/endpoint endpoint
        /// </summary> 
        private Dictionary<string, string> CreateLoginDictionary(string username, string password)
        {
            return new Dictionary<string, string>
            {
                {"client_id", "nativeApp"},
                {"client_secret", "secret"},
                {"grant_type", "password"},
                {"username", username},
                {"password", password},
                {"scope", "App2NightAPI offline_access"},
                {"offline_access", "true"}
            };
        }

        public async Task<Result> RefreshToken()
        { 
            Dictionary<string, string> tokenRefreshObject = CreateRefreshDictionary();

            //Request token refresh 
            var result =
                await
                    _clientService.SendRequest<Token>("connect/revocation", RestType.Post,
                        wwwFormData: tokenRefreshObject, token: Token.AccessToken, endpoint: Endpoint.User, enableHttps: false);

            if (result.Success)
            {
                //Set the new refresh date
                Token.LastRefresh = DateTime.Now;
                await _storageService.SaveStorage();
            }
            return result;
        }

        /// <summary>
        /// Creates a dictiory containg all values for the /connect/revocation endpoint.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> CreateRefreshDictionary()
        {
            return new Dictionary<string, string>
            {
                {"client_id", "nativeApp"},
                {"client_secret", "secret"},
                {"token", Token.RefreshToken},
                {"token_type_hint", "access_token"}
            };
        }

        public Task<Result> RequestNewPasswort(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<IEnumerable<Party>>>  RequestPartyWithFilter()
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
            } 
            PartiesUpdated?.Invoke(this, EventArgs.Empty);
            return requestResult;
        }

        public async Task<Result<Party>> GetParty(Guid id)
        {
            if(!await CheckIfTokenIsValid()) return new Result<Party>(); //Empty result with success = false
            
            //Request the party with the given id
            var result =
                await
                    _clientService.SendRequest<Party>("api/party", RestType.Get, urlQuery: "/id=" + id.ToString("D"),
                        token: Token.AccessToken);
            return result;
        }

        async Task<bool> CheckIfTokenIsValid()
        { 
            if (Token == null) return false;

            //Check if token is expired
            if (Token.ExpirationDate > DateTime.Now) return true;

            //Try to refreh token if token is expired
            var result = await RefreshToken();
            return result.Success;
        }

        /// <summary>
        /// Clears and populates an <see cref="ObservableCollection{T}"/> without setting it.
        /// </summary> 
        /// <param name="collection">Collection to be filled with objects.</param>
        /// <param name="newObjects">The new items that should be put into the collection.</param>
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