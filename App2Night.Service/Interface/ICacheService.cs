using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
        /// <returns><see cref="ObservableCollection{Party}"/></returns>
        ObservableCollection<Party> Partys { get; }

        Task RefreshPartys();

        /// <summary>
        /// Returns cached user data.
        /// </summary>
        /// <returns><see cref="User"/></returns>
        User GetUser();

    }
}