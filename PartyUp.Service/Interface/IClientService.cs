using System.Collections;
using System.Collections.Generic;
using PartyUp.Model.Enum;
using PartyUp.Model.Model;

namespace PartyUp.Service.Interface
{
    public interface IClientService
    {
        User GetUser();
        Party GetParty();

        IEnumerable<Party> GetParties(MusicGenre musicGenre = MusicGenre.All, float distanceFromPosition = 30000,
            Coordinates coordinates = null);
    }
}