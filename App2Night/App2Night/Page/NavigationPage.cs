using App2Night.CustomView.View;
using App2Night.Data.Language;
using App2Night.PageModel;
using Xamarin.Forms;

namespace App2Night.Page
{
    public class NavigationPage : ContentPage
    {
        #region Views
        public ListView MenuListView { get; }= new ListView(ListViewCachingStrategy.RecycleElement);
        Label _nameLabel = new Label
        {
            FontSize = 18,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
        };

        Label _nextPartyLabel = new Label
        {
            FontSize = 18,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
        };

        private Button _logoutBtn = new Button
        {
            Text = AppResources.Logout
        };

        private CustomButton _logInButton = new CustomButton
        {
            ButtonLabel = { Text = AppResources.Login },
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center
        };
        #endregion
        
        public NavigationPage()
        {
            _nameLabel.SetBinding(Label.TextProperty, "User.Name");

            _logoutBtn.SetBinding(Button.CommandProperty, nameof(NavigationViewModel.LogOutCommand));
            _logInButton.SetBinding(CustomButton.CommandProperty, nameof(NavigationViewModel.LogInCommand));

            var loginContentView = LoginContentView();
            //var partyContentView = PartyContentView();
            var logoutContentView = LogoutContentView();

            loginContentView.SetBinding(IsVisibleProperty, nameof(NavigationViewModel.IsLogInContentView));
            logoutContentView.SetBinding(IsVisibleProperty, nameof(NavigationViewModel.IsLogOutContentView));
            _logoutBtn.SetBinding(IsVisibleProperty, nameof(NavigationViewModel.IsLogInContentView));
             
            if(Device.OS == TargetPlatform.iOS) Padding = new Thickness(0, 20, 0, 0);
            Title = AppResources.Menu;
            
            Content = new Grid
            {  
                RowSpacing = 0,
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(2, GridUnitType.Star)},
                    new RowDefinition { Height = new GridLength(7, GridUnitType.Star)},
                    new RowDefinition { Height = new GridLength(50, GridUnitType.Absolute)}
                },
                Children =
                {
                    loginContentView,
                   // partyContentView,
                    logoutContentView,
                    {MenuListView, 0, 1},
                    {_logoutBtn, 0, 2}
                }
            };

        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            MenuListView.RowHeight =  (int) (((Height-50)/9.0*7) / 6);
        }

        private ContentView LoginContentView ()
        {
            return new ContentView
            {
                Content = new Grid
                {
                    ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star)},
                        new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star)},
                    },
                    Children =
                    {
                        {_nameLabel,0,1 }
                    }
                }
            };
        }
        private ContentView PartyContentView ()
        {
            return new ContentView
            {
                Content = new Grid
                {
                    ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star)},
                        new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star)},
                    },
                    Children =
                    {
                        {_nextPartyLabel,1,0 }
                    }
                }
            };
        }
        private ContentView LogoutContentView ()
        {
            return new ContentView
            {
                Content = _logInButton
            };
        }
    }
}