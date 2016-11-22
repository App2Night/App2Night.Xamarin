namespace App2Night.Model.Model
{
    public class Storage
    {
        public Token Token { get; set; }

        public int FilterRadius { get; set; } = 20;

        public bool UseGps { get; set; } = true;

    }
}