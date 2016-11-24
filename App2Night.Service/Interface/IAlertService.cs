using System.Collections.Generic;
using System.Threading.Tasks;
using App2Night.Model.Model;

namespace App2Night.Service.Interface
{
    public interface IAlertService
    {
        void PartyPullFinished(Result<IEnumerable<Party>> requestResult);

        void UserCreationFinished(Result requestResult, string username);

        void LoginFinished(Result requestResult);
    }
}