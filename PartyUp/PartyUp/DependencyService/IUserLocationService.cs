using System.Threading.Tasks;
using Android.Locations;
using PartyUp.Model.Enum;

namespace PartyUp.DependencyService
{
    public interface IUserLocationService
    {
        Coordinates GetUserCoordinates();
    }
}