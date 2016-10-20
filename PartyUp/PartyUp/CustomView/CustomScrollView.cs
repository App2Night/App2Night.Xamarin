using Xamarin.Forms;

namespace PartyUp.CustomView
{
    public class CustomScrollView : ScrollView
    {
        public bool HorizontalBarEnabled { get; set; } = true;
        public bool VerticalBarEnabled { get; set; } = true;

    }
}