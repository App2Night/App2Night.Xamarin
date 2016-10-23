using MvvmNano.Forms;
using PartyUp.ViewModel;
using Xamarin.Forms;

namespace PartyUp.View
{
    public class SettingPage : MvvmNanoContentPage<SettingViewModel>
    {
        public SettingPage()
        {
            // TODO Set Settingsoption
            var stackLayout = new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        Text = "Privacy",
                        FontSize = 20
                    },
                    new BoxView
                    {
                        HeightRequest = 1,
                        BackgroundColor = Color.Black
                    },
                    new Grid
                    {
                        ColumnDefinitions = new ColumnDefinitionCollection
                        {
                            new ColumnDefinition {Width = new GridLength(50, GridUnitType.Star)},
                            new ColumnDefinition {Width = new GridLength(50, GridUnitType.Star)}
                        },
                        Children =
                        {
                            {
                                new Label
                                {
                                    Text = "Setting",
                                    HorizontalOptions = LayoutOptions.Start
                                },
                                0, 0
                            },
                            {new Switch
                            {
                                HorizontalOptions = LayoutOptions.End
                            }, 1, 0}
                        }
                    }
                }
            };
            Content = new ContentView
            {
                Content = stackLayout,
                BackgroundColor = Color.White
            };
        }
    }
}