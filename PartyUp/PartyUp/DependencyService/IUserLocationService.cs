using System.Threading.Tasks; 
using PartyUp.Model.Enum;

namespace PartyUp.DependencyService
{
    public interface IUserLocationService
    {
        Coordinates GetUserCoordinates();
    }
}