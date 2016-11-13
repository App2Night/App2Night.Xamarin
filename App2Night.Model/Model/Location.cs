namespace App2Night.Model.Model
{
    public class Location
    {
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string StreetName { get; set; }
        public int HouseNumber { get; set; }
        public string HouseNumberAdditional { get; set; }
        public int Zipcode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}