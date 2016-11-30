namespace App2Night.Model.Model
{
    public class Coordinates
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public Coordinates() { }

        public Coordinates(float longitude, float latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }
    }
}