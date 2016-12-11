using System.Collections.ObjectModel;
using System.ComponentModel;
using App2Night.Model.Enum;

namespace App2Night.Model.Model
{
    public class User 
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public ObservableCollection<Party> Events { get; set; }
        public Location Addresse { get; set; }
        public Location LastGpsLocation { get; set; } 
	}
}