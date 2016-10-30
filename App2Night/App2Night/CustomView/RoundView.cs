using Xamarin.Forms;

namespace App2Night.CustomView
{
    public class RoundView : ContentView
    {
        public bool FlatBottom { get; set; }= false;
        public bool Edge { get; set; } = true;
        public double EdgeSize { get; set; } = 10;
        public Color EdgeColor { get; set; } = Color.Accent;
    }
}