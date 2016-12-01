using System;
using System.Threading.Tasks;
using App2Night.Model.Model;

namespace App2Night.Service.Interface
{
    /// <summary>
    /// Service that provides secrets, encryption and data storage.
    /// </summary>
    public interface IStorageService
    {
        event EventHandler<bool> IsLoginChanged;
        bool IsLogIn { get; }
        /// <summary>
        /// Returns the <see cref="Storage"/> object that contains all settings, user data and the last token.
        /// </summary>
        Storage Storage { get; set; } 

        /// <summary>
        /// Encrypts and saves the storage in to a file on the device.
        /// </summary>
        /// <returns></returns>
        Task SaveStorage();

        /// <summary>
        /// Restores the last saved storage from the device.
        /// </summary>
        /// <returns></returns>
        Task OpenStorage();

        /// <summary>
        /// Deletes the storage and overwrites the saved data on the device with a default storage.
        /// </summary>
        /// <returns></returns>
        Task DeleteStorage();

        /// <summary>
        /// Sets Token to current Storage and invoke EventHandler
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task SetToken(Token token);
        //TODO Add party caching
    }
}