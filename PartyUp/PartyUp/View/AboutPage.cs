using MvvmNano.Forms;
using PartyUp.ViewModel;
using Xamarin.Forms;

namespace PartyUp.View
{
    public class AboutPage : MvvmNanoContentPage<AboutViewModel>
    {
        public AboutPage()
        {
            // Set title of the page
            var titleLabel = new Label
            {
                Text = "Information about this App",
                FontSize = 20
                
            };
            // Set Context of the page
            var contextLabel = new Label
            {
                Text = "Context goes Here!"
            };
            var grid = new Grid()
            {
                RowDefinitions = new RowDefinitionCollection()
                {
                    new RowDefinition() {Height = new GridLength(50, GridUnitType.Absolute)},
                    new RowDefinition() {Height = new GridLength(50, GridUnitType.Absolute)},
                    new RowDefinition() {Height = new GridLength(1, GridUnitType.Auto)}
                },
                Children =
                {
                   {titleLabel,0,0},
                   {contextLabel,0,1}
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