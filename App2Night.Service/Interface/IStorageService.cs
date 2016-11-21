using System.Threading.Tasks;
using App2Night.Model.Model;

namespace App2Night.Service.Interface
{
    public interface IStorageService
    {
        Task SaveStorage(Storage storage);
        Task<Storage> ResumeStorage();
    }
}