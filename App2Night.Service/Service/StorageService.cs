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

        public async Task SaveStorage(Storage storage)
        { 
            var folder = await GetFolder();
            var file = await folder.CreateFileAsync(_fileName,
                CreationCollisionOption.ReplaceExisting);

            string data = JsonConvert.SerializeObject(storage);
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

        public async Task<Storage> OpenStorage()
        {
            var folder = await GetFolder();
            var fileExists = await folder.CheckExistsAsync(_fileName);
            if (fileExists == ExistenceCheckResult.NotFound)
                //File does not exist, return null
                return null;
            else
            //File exists return data
            {
                var file = await folder.GetFileAsync(_fileName);
                string encryptedString = await file.ReadAllTextAsync();
                var decryptedString = DecriptString(encryptedString);
                var storage = JsonConvert.DeserializeObject<Storage>(decryptedString);
                return storage;
            }
        }
    }
}