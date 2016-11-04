using App2Night.CustomPage;
using App2Night.ViewModel;
using MvvmNano.Forms;
using Xamarin.Forms;

namespace App2Night.View
{
    public class AboutPage : ContentPageWithInfo<AboutViewModel>
    {
        public AboutPage()
        {
            // TODO Fill Context
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