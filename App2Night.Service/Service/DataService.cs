using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using App2Night.Model.Enum;
using App2Night.Model.HttpModel;
using App2Night.Model.Model;
using App2Night.Service.Helper;
using App2Night.Service.Interface; 
using Plugin.Connectivity;
using Xamarin.Forms;

namespace App2Night.Service.Service
{
    public class DataService : IDataService
    {
        //Service references
        private readonly IClientService _clientService;
        private readonly IStorageService _storageService;

        public event EventHandler NearPartiesUpdated;
        public event EventHandler HistoryPartisUpdated;
        public event EventHandler SelectedPartiesUpdated;
        public event EventHandler<Party> SelectedPartyUpdated;
        public event EventHandler<User> UserUpdated;


        /// <summary>
        /// Provides the token from the storage. 
        /// </summary> 
        private Token Token
        {
            get { return _storageService.Storage.Token; }
            set { _storageService.SetToken(value); }
        } 

        /// <summary>
        /// Constructor
        /// </summary> 
        public DataService(IClientService clientService, IStorageService storageService)
        {
            //Get the dependencys
            _clientService = clientService;
            _storageService = storageService;
            RestorePartiesFromCache();

        }

        private void RestorePartiesFromCache()
        { 
            Device.BeginInvokeOnMainThread(() =>
            {
                var cachedLocalParties = _storageService.RestoreCachedParty(PartyListType.Local);
                SetNearParties(cachedLocalParties);

                var cachedPartyHistory = _storageService.RestoreCachedParty(PartyListType.History);
                SetHistoryParty(cachedPartyHistory); 

                var cachedSelectedParties = _storageService.RestoreCachedParty(PartyListType.ByUser);
                SetSelectedParties(cachedSelectedParties);
            }); 
        }

        public ObservableCollection<Party> InterestingPartys { get; } = new ObservableCollection<Party>();

        public ObservableCollection<Party> SelectedPartys { get; } = new ObservableCollection<Party>();

        public ObservableCollection<Party> PartyHistory { get; } = new ObservableCollection<Party>(); 

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

        async Task<bool> CheckIfTokenIsValid()
        {
            if (Token == null) return false;

            //Check if token is expired
            if (Token.ExpirationDate > DateTime.Now) return true;

            //Try to refreh token if token is expired
            var result = await RefreshToken();
            return result.Success;
        } 

        #region party creation, modification

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

        public async Task<Result<Party>> CreateParty(string name, DateTime date, MusicGenre genre, string country,
            string cityName, string street, string houseNr, string zipcode, PartyType type, string description, int price)
        {
            //Create an object from the given parameters for the party creation
            dynamic partyCreationObject = CreatePartyCreateObject(name, date, genre, country, cityName, street, houseNr,
                zipcode, type, description, price);

            //Send the create party request
            Result <Guid> result =
                await
                    _clientService.SendRequest<Guid>("api/party", RestType.Post, bodyParameter: partyCreationObject,
                        token: Token.AccessToken); 

            Result<Party> partyResult = new Result<Party>
            {
                IsCached = result.IsCached,
                Message = result.Message,
                RequestFailedToException = result.RequestFailedToException,
                StatusCode = result.StatusCode,
                Success = result.Success
            };

            if (result.Success)
            {
                //Accept to the party, as host you obviously appear.
                await ChangeCommitmentState(result.Data, PartyCommitmentState.Accepted);

                DebugHelper.PrintDebug(DebugType.Info, $"Guid of the created party is {result.Data}");
                //Get the created party if the creation was successfull. 

                partyResult = await GetParty(result.Data);
            } 

            //Return the created party
            return partyResult;
        }

        /// <summary>
        /// Creates a party object for the api/party post endpoint.
        /// </summary> 
        private dynamic CreatePartyCreateObject(string name, DateTime date, MusicGenre genre, string country,
            string cityName, string street, string houseNr, string zipcode, PartyType type, string description
            , int price)
        {
            dynamic partyCreationObject = new ExpandoObject();
            partyCreationObject.partyName = name;

            partyCreationObject.partyDate = date;

            partyCreationObject.musicGenre = (int) genre;

            partyCreationObject.countryName = country;

            partyCreationObject.cityName = cityName;

            partyCreationObject.streetName = street;

            partyCreationObject.houseNumber = houseNr;

            partyCreationObject.zipcode = zipcode;

            partyCreationObject.partyType = (int) type;

            partyCreationObject.description = description;

            partyCreationObject.price = price;

            return partyCreationObject;
        }

        public async Task<Result> DeleteParty(Guid partyId)
        {
            if(! await CheckIfTokenIsValid()) return new Result();
            var result = await _clientService.SendRequest("/api/Party", RestType.Delete, 
                            token: Token.AccessToken, urlQuery: "?id=" + partyId.ToString("D"));
            return result;
        }

        public async Task<Result> UpdateParty(Party party)
        { 
            if (!await  CheckIfTokenIsValid()) return new Result();
            dynamic o = CreatePartyCreateObject(party.Name, party.Date, party.MusicGenre, party.Location.CountryName,
                party.Location.CityName, party.Location.StreetName, party.Location.HouseNumber, party.Location.Zipcode,
                party.PartyType, party.Description, party.Price);
            var result =
                await
                    _clientService.SendRequest("/api/Party", RestType.Put, bodyParameter: o, token: Token.AccessToken, 
                        urlQuery: "?id=" + party.Id.ToString("D"));

            if (result.Success)
            {
                await GetParty(party.Id);
            }

            return result;
        }

        public async Task<Result> ChangeCommitmentState(Guid partyId, PartyCommitmentState commitmentState)
        {
            if (!await CheckIfTokenIsValid()) return new Result
            {
                NeedLogin = true
            };

            dynamic bodyObject = new ExpandoObject();
            bodyObject.eventCommitment = commitmentState;


            Result result =
                await
                    _clientService.SendRequest("/api/UserParty/commitmentState", RestType.Put,
                        urlQuery: "?id=" + partyId.ToString("D"), bodyParameter: bodyObject, token: Token.AccessToken);

            if (result.Success)
            {
                foreach (Party interestingParty in InterestingPartys)
                {
                    if (interestingParty.Id == partyId)
                    {
                        interestingParty.CommitmentState = commitmentState;
                        if (commitmentState == PartyCommitmentState.Noted)
                        {
                            SelectedPartys.Add(interestingParty);
                        }
                        break;
                    }
                }

                foreach (Party interestingParty in SelectedPartys)
                {
                    if (interestingParty.Id == partyId)
                    {
                        interestingParty.CommitmentState = commitmentState;
                        break;
                    }
                }
            }

            return result;
        }

        public async Task<Result> RateParty(Guid partyId, int general, int price, int location, int mood)
        {
            if (!await CheckIfTokenIsValid())
            {
                return new Result();
            }
            dynamic o = new ExpandoObject();
            o.generalRating = general;
            o.priceRating = price;
            o.locationRating = location;
            o.moodRating = mood;

            var result =  await _clientService.SendRequest("/api/userParty/partyRating", RestType.Put, bodyParameter: o,
                            token: Token.AccessToken, urlQuery:"?id=" + partyId.ToString("D"));

            if (result.Success)
            {
                await GetParty(partyId);
            }

            return result;
        }

        #endregion

        #region user handlign

        public User User
        {
            get { return _storageService.Storage?.User; }
            set
            {
                _storageService.Storage.User = value;
                _storageService.SaveStorage();
            }
        } 

        public async Task<Result> GetUser()
        {
            if (!await CheckIfTokenIsValid()) return new Result();
            var result = await _clientService.SendRequest<User>("/connect/userinfo", RestType.Get, 
                            token: Token.AccessToken, endpoint:Endpoint.User);
            User = result.Data;
			Device.BeginInvokeOnMainThread(() => UserUpdated?.Invoke(this, User));
            return result;
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
                var creationResult =
                    await
                        _clientService.SendRequest("api/user", RestType.Post, bodyParameter: signUpModels,
                            endpoint: Endpoint.User);

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

        public async Task<Result> RequestToken(string username, string password)
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
                await GetUser();
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
                {"scope", "App2NightAPI offline_access openid email profile"},
                {"offline_access", "true"}
            };
        }

        public Task<Result> RequestNewPasswort(string email)
        {
            //Not implemented by the backend yet, an idea for the next version.
            throw new NotImplementedException();
        }

        public async Task<Result> RefreshToken()
        {
            Dictionary<string, string> tokenRefreshObject = CreateRefreshDictionary();

            //Request token refresh 
            var result =
                await
                    _clientService.SendRequest<Token>("connect/revocation", RestType.Post,
                        wwwFormData: tokenRefreshObject, token: Token.AccessToken, endpoint: Endpoint.User,
                        enableHttps: false);

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

        #endregion  

        #region party requests

        public async Task<IEnumerable<Result>> BatchRefresh()
        {
            var allResults = new List<Result>();
			var allTasks = new Task[]
			{
				Task.Run(async () =>
				{
					allResults.Add(await RequestPartyWithFilter());
				}),
				Task.Run(async () =>
				{
					allResults.Add(await RefreshPartyHistory());
				}),
				Task.Run(async () =>
				{
					allResults.Add(await RefreshSelectedParties());
				})	
			};

			await Task.WhenAll(allTasks);

            return allResults;
        }

        public async Task<Result<IEnumerable<Party>>> RefreshPartyHistory()
        {
            if (!await CheckIfTokenIsValid())
                return new Result<IEnumerable<Party>> { NeedLogin = true };

            var result = await
                       _clientService.SendRequest<IEnumerable<Party>>("api/party/history", RestType.Get,
                           token: Token?.AccessToken);

            await HandleCaching(result, PartyListType.History);
            SetHistoryParty(result.Data);

            return result;
        }

        private void SetHistoryParty(IEnumerable<Party> parties)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                PopulateObservableCollection(PartyHistory, parties);
                HistoryPartisUpdated?.Invoke(this, EventArgs.Empty);
            });
        }

        public async Task<Result<IEnumerable<Party>>> RefreshSelectedParties()
        {
            if (!await CheckIfTokenIsValid())
                return new Result<IEnumerable<Party>> { NeedLogin = true };

            var result = await
                        _clientService.SendRequest<IEnumerable<Party>>("api/party/myParties", RestType.Get,
                            token: Token?.AccessToken);

            await HandleCaching(result, PartyListType.ByUser);
            SetSelectedParties(result.Data);

            return result;
        }

        private void SetSelectedParties(IEnumerable<Party> parties)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                PopulateObservableCollection(SelectedPartys, parties);

                SelectedPartiesUpdated?.Invoke(this, EventArgs.Empty);
            });
        }


        public async Task<Result<IEnumerable<Party>>> RequestPartyWithFilter()
        {
            Result<IEnumerable<Party>> requestResult = new Result<IEnumerable<Party>>();

            Coordinates coordinates = await CoordinateHelper.GetCoordinates();

            try
            {
                //Format coordinates 
                var uri = $"?lat={coordinates.Latitude}&lon={coordinates.Longitude}&radius={_storageService.Storage.FilterRadius}"
                    .Replace(",", ".");  //Backend does not like , in the request

                requestResult =
                    await
                        _clientService.SendRequest<IEnumerable<Party>>("api/party", RestType.Get, urlQuery: uri,
                            token: Token?.AccessToken);
            }
            catch (Exception e)
            {
                DebugHelper.PrintDebug(DebugType.Error, "Fetching parties failed.\n" + e);
                requestResult.RequestFailedToException = true;
            }

            await HandleCaching(requestResult, PartyListType.Local);
            //Check if the request was a success

            SetNearParties(requestResult.Data);

            return requestResult;
        }

        private void SetNearParties(IEnumerable<Party> parties)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                PopulateObservableCollection(InterestingPartys, parties);
                NearPartiesUpdated?.Invoke(this, EventArgs.Empty);
            });
        }

        async Task HandleCaching(Result<IEnumerable<Party>> rawResult, PartyListType listType)
        {
            if (rawResult.Success)
            {
                _storageService.CacheParty(rawResult.Data, listType);
            }
            else
            { 
                var cachedData = _storageService.RestoreCachedParty(listType);

                if (cachedData != null && cachedData.Count > 0)
                {
                    rawResult.Data = cachedData;
                    rawResult.IsCached = true;
                } 
            }
        } 

        public async Task<Result<Party>> GetParty(Guid id)
        {
            if (!await CheckIfTokenIsValid()) return new Result<Party>(); //Empty result with success = false

            //Request the party with the given partyId
            var result =
                await
                    _clientService.SendRequest<Party>("api/party", RestType.Get, urlQuery: "/id=" + id.ToString("D"),
                        token: Token.AccessToken);
             
            if (result.Success)
            {
                AddPartyToCollection(PartyHistory, result.Data);
                AddPartyToCollection(SelectedPartys, result.Data);
                AddPartyToCollection(InterestingPartys, result.Data);
                Device.BeginInvokeOnMainThread(()=>SelectedPartyUpdated?.Invoke(this, result.Data));
            } 
            return result;
        }

        /// <summary>
        /// Adds a party to a collection if the party was not part of the collection, adds missing attributes if it was already part of the collection.
        /// </summary> 
        void AddPartyToCollection<TCollection>(TCollection collection, Party party)
            where TCollection : ObservableCollection<Party>
        {
            var alreadyInCollection = false;
            foreach (Party item in collection)
            {
                if (item.Id == party.Id)
                {
                    item.Participants = party.Participants;
                    item.Location = party.Location;
                    item.Name = party.Name; 
                    item.Date = party.Date;

                    //Voting
                    item.GeneralUpVoting = party.GeneralUpVoting;
                    item.LocationUpVoting = party.LocationUpVoting;
                    item.MoodUpVoting = party.MoodUpVoting;
                    item.PriceUpVoting = party.PriceUpVoting;
                    item.GeneralDownVoting = party.GeneralDownVoting;
                    item.LocationDownVoting = party.LocationDownVoting;
                    item.MoodDownVoting = party.MoodDownVoting;
                    item.PriceDownVoting = party.PriceDownVoting;

                    alreadyInCollection = true;
                    break;
                } 
            }

            if(!alreadyInCollection)
                collection.Add(party);
        } 

        /// <summary>
        /// Clears and populates an <see cref="ObservableCollection{T}"/> without setting it.
        /// </summary> 
        /// <param name="collection">Collection to be filled with objects.</param>
        /// <param name="newObjects">The new items that should be put into the collection.</param>
        void PopulateObservableCollection<TObservable>(TObservable collection, IEnumerable<Party> newObjects)
            where TObservable : ObservableCollection<Party>
        {
            collection.Clear();
            if (newObjects != null)
            {
                foreach (Party party in newObjects)
                {
                    collection.Add(party);
                }
            }
        }

        public void ClearData()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                SelectedPartys.Clear();
                PartyHistory.Clear();
                SelectedPartiesUpdated?.Invoke(this, EventArgs.Empty);
                HistoryPartisUpdated?.Invoke(this, EventArgs.Empty);
            }); 
        }

        

        #endregion  
    }
}