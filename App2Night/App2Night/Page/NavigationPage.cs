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
			HorizontalOptions = LayoutOptions.Start,
        };

        private Button _logoutBtn = new Button
        {
            Text = AppResources.Logout
        };

        private CustomButton _logInButton = new CustomButton
        {
            ButtonLabel = { Text = AppResources.Login, FontSize = 18 },
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center
        };
		private CustomButton _editProfileButton = new CustomButton 
		{ 
			ButtonLabel = {FontFamily="FontAwesome", Text= "\uf044" , FontSize=18},
			HorizontalOptions = LayoutOptions.Start,

		};
		private CustomButton _nextPartyButton = new CustomButton 
		{
			ButtonLabel = { FontSize = 18},
			VerticalOptions = LayoutOptions.Center,
			HorizontalOptions = LayoutOptions.Start,
		};
		private ContentView _logoutContentView, _loginContentView;
        #endregion
        
        public NavigationPage()
        {
			_loginContentView = LoginContentView();
			_logoutContentView = LogoutContentView();
			SetBindings();
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
                    _loginContentView,
                    _logoutContentView,
                    {MenuListView, 0, 1},
                    {_logoutBtn, 0, 2}
                }
            };

        }

		private void SetBindings()
		{
			// name of user and party
			_nameLabel.SetBinding(Label.TextProperty, nameof(NavigationViewModel.UserName), stringFormat: "Hello " + "{0}");
			_logoutBtn.SetBinding(Button.CommandProperty, nameof(NavigationViewModel.LogOutCommand));
			_logInButton.SetBinding(CustomButton.CommandProperty, nameof(NavigationViewModel.LogInCommand));
			_nextPartyButton.ButtonLabel.SetBinding(Label.TextProperty, nameof(NavigationViewModel.PartyName), stringFormat: "Your next Party " + "{0}");
			_editProfileButton.SetBinding(CustomButton.CommandProperty, nameof(NavigationViewModel.MoveToUserEditCommand));
			// log in and log out to hide views
			_nameLabel.SetBinding(IsVisibleProperty, nameof(NavigationViewModel.IsLogIn));
			_editProfileButton.ButtonLabel.SetBinding(IsVisibleProperty, nameof(NavigationViewModel.IsLogIn));
			_nextPartyButton.ButtonLabel.SetBinding(IsVisibleProperty, nameof(NavigationViewModel.IsNextParty));
			_logoutContentView.SetBinding(IsVisibleProperty, nameof(NavigationViewModel.IsLogOut));
			_logoutBtn.SetBinding(IsVisibleProperty, nameof(NavigationViewModel.IsLogIn));
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
					Margin = 5,
					RowSpacing = 0,
					ColumnDefinitions = new ColumnDefinitionCollection
					{
						new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)},
						new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto)},
					},
					RowDefinitions = new RowDefinitionCollection
					{
						new RowDefinition { Height = new GridLength(50	, GridUnitType.Absolute)},
						new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)},
						new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)},

					},
					Children =
					{
						{_nameLabel,0,1 },
						{_editProfileButton,1,1 },
						{_nextPartyButton,0,2 }
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