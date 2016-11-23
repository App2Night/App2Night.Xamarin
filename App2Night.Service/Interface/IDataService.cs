using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using App2Night.Model.Enum;
using App2Night.Model.HttpModel;
using App2Night.Model.Model;

namespace App2Night.Service.Interface
{
    /// <summary>
    /// Interface that provides the cached data.
    /// </summary>
    public interface IDataService
    {
        event EventHandler PartiesUpdated;

        event EventHandler UserUpdated;
        
        /// <summary>
        /// Sets the token.
        /// </summary>
        /// <param name="token">The new token.</param>
        /// <returns>Wether or not the token is valid.</returns>
        Task<bool> SetToken(Token token);

        Task<Result<Location>> ValidateLocation(Location location);

        /// <summary>
        /// Clears all data from the device storage and current objects, including the logged in user.
        /// </summary>
        Task WipeData();

        /// <summary>
        /// Returns the current <see cref="Model.Model.User"/>.
        /// </summary>
        User User { get; }

        /// <summary>
        /// Returns a cached collection of all <see cref="Party"/> filtered by last applied search criteria.
        /// </summary>
        /// <returns><see cref="ObservableCollection{Party}"/></returns>
        ObservableCollection<Party> InterestingPartys { get; }

        /// <summary>
        /// Returns a cached collection of all <see cref="Party"/> filtered by last applied search criteria.
        /// </summary>
        /// <returns><see cref="ObservableCollection{Party}"/></returns>
        ObservableCollection<Party> SelectedPartys { get; }

        /// <summary>
        /// Returns a cached collection of all <see cref="Party"/> filtered by last applied search criteria.
        /// </summary>
        /// <returns><see cref="ObservableCollection{Party}"/></returns>
        ObservableCollection<Party> PartyHistory { get; }

        /// <summary>
        /// Refresh the <see cref="InterestingPartys"/> collection.
        /// </summary> 
        Task<Result<IEnumerable<Party>>> RefreshPartys();

        Task<Result<Party>> GetParty(Guid id);

        /// <summary>
        /// Creates a new party.
        /// </summary>
        /// <returns></returns>
        Task<Result<Party>> CreateParty(string name, DateTime date, MusicGenre genre, string country, string cityName, string street, string houseNr, string zipcode, PartyType type, string description);

        Task<Result> DeleteParty();

        Task<Result> UpdateParty();

        Task<Result> UpdateUser();

        Task<Result> DeleteAccount();

        Task<Result> CreateUser(SignUp signUpModel);

        /// <summary>
        /// Requests a token in name of the user.
        /// </summary>
        /// <param name="username">User Username.</param>
        /// <param name="password">User Password</param>
        /// <returns></returns>
        Task<Result> RequestToken(string username, string password);

        Task<Result> RequestNewPasswort(string email);

        Task<Result> RefreshToken();
    }
}