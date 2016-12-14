using SQLite.Net.Attributes;

namespace App2Night.Model.Model
{
    public class Location
    { 
        //Table id
        [PrimaryKey, AutoIncrement] 
        public int Id { get; set; }

        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; } 
        public string Zipcode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}