using System.Collections.ObjectModel;
using PartyUp.Model.Model;

namespace PartyUp.Service.Interface
{
    /// <summary>
    /// Interface that provides the cached data.
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Returns a cached collection of all events filtered by last applied search criteria.
        /// </summary>
        /// <returns><see cref="ObservableCollection{Event}"/></returns>
        ObservableCollection<Event> GetEvents();

        /// <summary>
        /// Returns cached user data.
        /// </summary>
        /// <returns><see cref="User"/></returns>
        User GetUser();

    }
}