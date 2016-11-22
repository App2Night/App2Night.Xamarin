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
        private string _folderName = "SStorage";
        private string _fileName = "SStore.txt";
        private Storage _storage;

        public Storage Storage
        {
            get { return _storage; }
            set { _storage = value; }
        }

        public async Task SaveStorage()
        { 
            var folder = await GetFolder();
            var file = await folder.CreateFileAsync(_fileName,
                CreationCollisionOption.ReplaceExisting);

            string data = JsonConvert.SerializeObject(Storage);
            data = EncryptString(data); 
            await file.WriteAllTextAsync(data);
        }

        async Task<IFolder> GetFolder()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            return await rootFolder.CreateFolderAsync(_folderName,
                CreationCollisionOption.OpenIfExists);
        }

        string EncryptString(string data)
        {
            //Add encryption
            return data;
        }

        string DecriptString(string encryptedData)
        {
            return encryptedData;
        } 

        public async Task OpenStorage()
        {
            var storage = new Storage();
            var cached = false;
            var folder = await GetFolder();
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
    }
}