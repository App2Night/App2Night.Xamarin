using Xamarin.Forms;

namespace App2Night.CustomView.View
{
    public class CustomScrollView : ScrollView
    {
        public bool HorizontalBarEnabled { get; set; } = true;
        public bool VerticalBarEnabled { get; set; } = true; 
    }
}