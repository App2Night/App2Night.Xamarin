using System.Threading.Tasks;

namespace App2Night.Service.Interface
{
    public interface ILocationAccess
    {
        Task<bool> HasAccess();
    }
}