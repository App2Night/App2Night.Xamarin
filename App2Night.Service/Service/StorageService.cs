using System;
using System.Diagnostics;
using System.Threading.Tasks;
using App2Night.Model.Model;
using App2Night.Service.Interface;
using Newtonsoft.Json;
using PCLStorage;

namespace App2Night.Service.Service
{
    public class StorageService : IStorageService
    {
        //Path and file names
        private string _folderName = "SStorage";
        private string _fileName = "SStore.txt";
        private Storage _storage;

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

        public async Task SaveStorage()
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

            var folder = await GetFolder();
            
            //Check if the file exists in the folder.
            var fileExists = await folder.CheckExistsAsync(_fileName);

            if (fileExists == ExistenceCheckResult.FileExists) 
            //File exists, get data
            { 
                try
                {
                    var file = await folder.GetFileAsync(_fileName);
                    string encryptedString = await file.ReadAllTextAsync();
                    var decryptedString = DecriptString(encryptedString);
                    storage = JsonConvert.DeserializeObject<Storage>(decryptedString);
                    cached = true;
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
        }

        public async Task DeleteStorage()
        {
            Storage = new Storage();
            await SaveStorage();
        }
    }
}