using System.Collections.ObjectModel;
using PartyUp.Model.Enum;

namespace PartyUp.Model.Model
{
    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public ObservableCollection<Party> Events { get; set; }
        public Location Addresse { get; set; }
        public Location LastGpsLocation { get; set; }
    }
}