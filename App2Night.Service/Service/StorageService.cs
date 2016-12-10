using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using App2Night.Model.Enum;
using App2Night.Model.Model;
using App2Night.Service.Helper;
using App2Night.Service.Interface;
using FreshMvvm;
using Newtonsoft.Json;
using PCLStorage; 
using SQLite.Net;
using Xamarin.Forms;

namespace App2Night.Service.Service
{
    public class StorageService : IStorageService
    {
        private readonly IDatabaseService _databaseService;

        //Path and file names
        private string _folderName = "SStorage";
        private string _fileName = "SStore.txt";
        private Storage _storage;
        private SQLiteConnection _databaseConnection;

        public event EventHandler<bool> IsLoginChanged;

        public bool IsLogIn { get; set; }

        public Storage Storage
        {
            get
            {
                //Create a new storage if non is set yet.
                if (_storage == null)
                {
                    _storage = new Storage(); 
                }
                return _storage;
            }
            set
            {
                _storage = value;
            }
        }

        public StorageService()
        { 
            _databaseService = FreshIOC.Container.Resolve<IDatabaseService>("IDatabaseService");
            _databaseConnection = _databaseService.GetConnection();

            //Make sure that all tables exist  
            _databaseConnection.CreateTable<Location>();
            _databaseConnection.CreateTable<Party>();
            _databaseConnection.CreateTable<Host>(); 
        } 

        public async Task SaveStorage()
        {
            try
            {
                var folder = await GetFolder();

                //Creates a file to save the storage in to.
                //Open the file if it already exists.
                var file = await folder.CreateFileAsync(_fileName,
                    CreationCollisionOption.ReplaceExisting);

                //Serialize the data
                string data = JsonConvert.SerializeObject(Storage);
                data = EncryptString(data);


                //Save the encrypted data.
                await file.WriteAllTextAsync(data);
            }
            catch (Exception ex)
            {
                DebugHelper.PrintDebug(DebugType.Error, "Saving storage failed" + ex);
            }
        }

        /// <summary>
        /// Gets the folder that contains the storage from the file system of the device.
        /// </summary>
        /// <returns>Folder that contains the storage.</returns>
        async Task<IFolder> GetFolder()
        {
            //Get the root folder of the application on the device file system.
            IFolder rootFolder = FileSystem.Current.LocalStorage;

            //Get the folder that contains the storage inside of the root folder.
            return await rootFolder.CreateFolderAsync(_folderName,
                CreationCollisionOption.OpenIfExists);
        }

        /// <summary>
        /// Encrypts a string.
        /// </summary>
        /// <param name="data">String that contains secret data.</param>
        /// <returns>The encrypted string.</returns>
        string EncryptString(string data)
        {
           //TODO Add encryption
            return data;
        }

        /// <summary>
        /// Decripts a string.
        /// </summary>
        /// <param name="encryptedData">Encrypted string.</param>
        /// <returns>The decrypted string.</returns>
        string DecriptString(string encryptedData)
        {
            //TODO Add decryption
            return encryptedData;
        }

        public async Task OpenStorage()
        {
            //Create a fallback storage if opening the storage fails.
            var storage = new Storage();

            //Signal if the storage comes from the file system or is freshly created.
            var cached = false;

            try
            {
                var folder = await GetFolder();

                //Check if the file exists in the folder.
                var fileExists = await folder.CheckExistsAsync(_fileName);

                if (fileExists == ExistenceCheckResult.FileExists)
                    //File exists, get data
                {
                    var file = await folder.GetFileAsync(_fileName);
                    string encryptedString = await file.ReadAllTextAsync();
                    var decryptedString = DecriptString(encryptedString);
                    storage = JsonConvert.DeserializeObject<Storage>(decryptedString);
                    cached = true;
                    if (storage.Token != null)
                    {
                        LogInChanged(true);
                    }
                }
            }

            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                Storage = storage;
                if (!cached) await SaveStorage();
            }
        }

        public async Task DeleteStorage()
        {
            Storage = new Storage();
            await SaveStorage();
            LogInChanged(false);
        }

        public async Task SetToken(Token token)
        {
            Storage.Token = token;
            await SaveStorage();
            LogInChanged(true);
           
        } 

        public async Task ClearCache()
        {
            _databaseConnection.DeleteAll<Party>();
        }

        private void LogInChanged(bool isLogIn)
        {
            IsLogIn = isLogIn;
            IsLoginChanged?.Invoke(null, isLogIn);
        }

        public Task CacheParty(IEnumerable<Party> parties, PartyListType partyListType)
        {

            try
            {
                //Apply the party list type
                foreach (Party party in parties)
                {
                    party.PartyListType = partyListType;
                    party.IsCached = true;
                }

                var connection = _databaseConnection; 

                //Delete the old cache for this party type.
               
                connection.Table<Party>()
                    .Delete(p => p.PartyListType == partyListType);

                //Cache
                foreach (Party party in parties)
                {
                    //Map objects
                    var locationId =  connection.Insert(party.Location);
                    var hostId = connection.Insert(party.Host);
                    party.HostId = hostId;
                    party.LocationId = locationId; 
                    
                    connection.Insert(party); 
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e); 
            }
            return Task.Delay(100);
        }

        public IList<Party> RestoreCachedParty(PartyListType listType)
        { 
            var connection = _databaseConnection;
            var parties = connection.Table<Party>().Where(p => p.PartyListType == listType).ToList();

            //Map objects
            foreach (Party party in parties)
            {
                party.Location = connection.Get<Location>(o => o.Id == party.LocationId);
                party.Host = connection.Get<Host>(o => o.Id == party.HostId);
            }

            return parties;
        }
    }
}