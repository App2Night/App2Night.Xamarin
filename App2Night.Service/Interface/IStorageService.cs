using System.Threading.Tasks;
using App2Night.Model.Model;

namespace App2Night.Service.Interface
{
    public interface IStorageService
    {
        Storage Storage { get; set; } 
        Task SaveStorage();
        Task OpenStorage();
    }
}