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
        /// <summary>
        /// Gets triggered if parties get updated.
        /// </summary>
        event EventHandler NearPartiesUpdated;

        /// <summary>
        /// Gets triggered if the party history get updated.
        /// </summary>
        event EventHandler HistoryPartisUpdated;

        /// <summary>
        /// Gets triggered if the selected parties get updated.
        /// </summary>
        event EventHandler SelectedPartiesUpdated;

        /// <summary>
        /// Gets triggerd if the user is updated.
        /// </summary>
        event EventHandler UserUpdated;
        
        /// <summary>
        /// Sets the token.
        /// </summary>
        /// <param name="token">The new token.</param>
        /// <returns>Wether or not the token is valid.</returns>
        Task<bool> SetToken(Token token);

        /// <summary>
        /// Sends a validation request and returns a possible location that corresponds to the given location.
        /// </summary>
        /// <param name="location">Location to be lookd up.</param>
        /// <returns>The best correspond to the given location.</returns>
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
        Task<Result<IEnumerable<Party>>> RequestPartyWithFilter();

        /// <summary>
        /// Returns a single party to the given id.
        /// </summary>
        /// <param name="id"><see cref="Guid"/> of the party.</param>
        /// <returns>The requested party.</returns>
        Task<Result<Party>> GetParty(Guid id);

        /// <summary>
        /// Creates a new party.
        /// </summary>
        /// <returns>The created <see cref="Party"/></returns>
        Task<Result<Party>> CreateParty(string name, DateTime date, MusicGenre genre, string country, string cityName, string street, string houseNr, string zipcode, PartyType type, string description);

        /// <summary>
        /// Deletes a party.
        /// </summary>
        /// <param name="id"><see cref="Guid"/> of the party.</param>
        /// <returns>Request result.</returns>
        Task<Result> DeleteParty(Guid id);

        /// <summary>
        /// Updates an existing party.
        /// </summary>
        /// <returns></returns>
        Task<Result> UpdateParty();

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <returns></returns>
        Task<Result> UpdateUser();

        /// <summary>
        /// Deletes the account of the user.
        /// </summary>
        /// <param name="password">Account password to validate the deletion.</param> 
        Task<Result> DeleteAccount(string password);

        /// <summary>
        /// Signs up a new user.
        /// </summary>
        /// <param name="signUpModel"><see cref="SignUp"/>  object with all user informations.</param>
        /// <returns></returns>
        Task<Result> CreateUser(SignUp signUpModel);

        /// <summary>
        /// Requests a token in name of the user.
        /// </summary>
        /// <param name="username">User Username.</param>
        /// <param name="password">User Password</param>
        /// <returns></returns>
        Task<Result> RequestToken(string username, string password);

        /// <summary>
        /// Requests a new password. Should be used if the user forgot his password.
        /// </summary>
        /// <param name="email">The email that corresponds with the account.</param>
        /// <returns></returns>
        Task<Result> RequestNewPasswort(string email);

        /// <summary>
        /// Refreshes the access_token stored in the <see cref="IStorageService"/>.
        /// </summary>
        /// <returns></returns>
        Task<Result> RefreshToken();

        Task<Result> ChangeCommitmentState(Guid partyId, PartyCommitmentState commitmentState);
    }
}