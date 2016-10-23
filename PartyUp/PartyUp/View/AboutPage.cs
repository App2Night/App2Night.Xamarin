using MvvmNano.Forms;
using PartyUp.ViewModel;
using Xamarin.Forms;

namespace PartyUp.View
{
    public class AboutPage : MvvmNanoContentPage<AboutViewModel>
    {
        public AboutPage()
        {
            Title = "Information";
            // Set Context of the page
            var contextLabel = new Label
            {
                Text = "Context goes Here!"
            };
            var grid = new Grid()
            {
                RowDefinitions = new RowDefinitionCollection()
                {
                    new RowDefinition() {Height = new GridLength(1, GridUnitType.Auto)},
                },
                Children =
                {
                    {contextLabel, 0, 0}
                },
            };
            // set content
            Content = new ContentView()
            {
                Content = grid,
                BackgroundColor = Color.White
            };
        }
    }
}