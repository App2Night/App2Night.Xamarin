using App2Night.CustomView.Page;
using App2Night.ViewModel;
using MvvmNano.Forms;
using Xamarin.Forms;

namespace App2Night.View
{
    public class SettingPage : ContentPageWithInfo<SettingViewModel> { 
        BoxView _topBoxView = new BoxView
        {
            HeightRequest = 1
        };
        public Color SeperatorColor
        {
            get { return (Color)GetValue(SeperatorColorProperty); }
            set { SetValue(SeperatorColorProperty, value); }
        }
        public static BindableProperty SeperatorColorProperty = BindableProperty.Create(nameof(SeperatorColor),
            typeof(Color), typeof(SettingPage), Color.Accent,
            propertyChanged: (bindable, value, newValue) => ColorChanged(bindable, (Color)value, (Color)newValue));

        private static void ColorChanged(BindableObject bindable, Color oldValue, Color newValue)
        {
            SettingPage thisListView = (SettingPage)bindable;
            thisListView._topBoxView.Color = newValue;
        }
        public SettingPage()
        {
            ColorChanged(this, SeperatorColor, SeperatorColor);
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
                    _topBoxView,
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