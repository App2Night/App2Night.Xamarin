using System.Collections.Generic;
using System.Threading.Tasks;
using App2Night.Model.Model;

namespace App2Night.Service.Interface
{
    /// <summary>
    /// Service to create and manage user alerts (pop-ups, toasts) centraliced.
    /// </summary>
    public interface IAlertService
    {
        /// <summary>
        /// Informs the user about the result of a party refresh.
        /// </summary>
        /// <param name="requestResult">The party refresh request result.</param>
        void PartyPullFinished(Result<IEnumerable<Party>> requestResult);

        /// <summary>
        /// Informas the user about the result of a user creation.
        /// </summary>
        /// <param name="requestResult">The creation request result.</param>
        /// <param name="username">The new username.</param>
        void UserCreationFinished(Result requestResult, string username);

        /// <summary>
        /// Informs the user wether or not a login was succesfull.
        /// </summary>
        /// <param name="requestResult">The login request result</param>
        void LoginFinished(Result requestResult);
    }
}