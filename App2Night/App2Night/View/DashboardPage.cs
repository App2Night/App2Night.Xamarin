using System;
using System.Collections.Generic;
using App2Night.CustomView.Page;
using App2Night.CustomView.Template;
using App2Night.CustomView.View;
using App2Night.Data.Language;
using App2Night.Model.Model;
using App2Night.ValueConverter;
using App2Night.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace App2Night.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class DashboardPage : ContentPageWithPreview<DashboardViewModel>
    {
        #region Views

        readonly EnhancedContainer _userInfoContainer = new EnhancedContainer
        {
            Name = AppResources.User,
            ButtonText = "\uf0ad",
            NoContentWarningVisible = false,
            TopSpacerVisible = false
        };

        readonly EnhancedContainer _interestingPartieContainer = new EnhancedContainer
        {
            Name = AppResources.IntParty,
            ContentMissingText = AppResources.ContIntParty
        };

        readonly EnhancedContainer _myPartiesContainer = new EnhancedContainer
        {
            Name = AppResources.MyParty,
            ContentMissingText = AppResources.ContMyParty
        };

        readonly EnhancedContainer _historyContainer = new EnhancedContainer
        {
            Name = AppResources.History,
            ContentMissingText = AppResources.ContHistory
        };

        readonly Map _headerMap = new Map()
        {
            HeightRequest = 200
        };

        readonly Image _profilePicture = new Image
        {
            HeightRequest = 100,
            WidthRequest = 100,
            Margin = new Thickness(10),
            Source = ImageSource.FromResource("App2Night.Data.IconCode.icon.png")
        };

        readonly Label _usernameLabel = new Label();
        readonly Label _emaiLabel = new Label();

        readonly HorizontalGallerieView _historyGallerieView = new HorizontalGallerieView
        {
            Columns = 2,
            Rows = 1,
            Template = typeof(QuadraticPartyTemplate)
        };

        readonly HorizontalGallerieView _interestingPartieGallerie = new HorizontalGallerieView
        {
            Columns = 1,
            Template = typeof(QuadraticPartyTemplate)
        };

        readonly HorizontalGallerieView _myPartieGallerie = new HorizontalGallerieView
        {
            Columns = 1,
            Template = typeof(QuadraticPartyTemplate)
        };
        #endregion

        public DashboardPage()
        {
            #region UserInfoView
            var userInfoView = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
                },
                Children =
                {
                    _profilePicture
                }
            };
            _userInfoContainer.Content = userInfoView;
            BindToViewModel(_usernameLabel, Label.TextProperty, vm => vm.User.Name);
            BindToViewModel(_emaiLabel, Label.TextProperty, vm => vm.User.Email);
            var profileDetails = new List<Xamarin.Forms.View>
            {
                _usernameLabel,
                _emaiLabel
            };
            userInfoView.RowDefinitions.Add(new RowDefinition {Height = new GridLength(1, GridUnitType.Star)});
            for (int index = 0; index < profileDetails.Count; index++)
            {
                Xamarin.Forms.View view = profileDetails[index];
                userInfoView.RowDefinitions.Add(new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)});
                userInfoView.Children.Add(view, 1, index + 1);
            }
            userInfoView.RowDefinitions.Add(new RowDefinition {Height = new GridLength(1, GridUnitType.Star)});
            Grid.SetRowSpan(_profilePicture, profileDetails.Count + 2);
            BindToViewModel(_userInfoContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToUserEditCommand);
            #endregion

            #region InterestingPartieView
            _interestingPartieContainer.Content = _interestingPartieGallerie;
            _interestingPartieGallerie.ElementTapped += PartieSelected;
            BindToViewModel(_interestingPartieGallerie, GallerieView.ItemSourceProperty,
                vm => vm.InterestingPartiesForUser);
            BindToViewModel(_interestingPartieContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToPartyPicker);
            BindToViewModel(_interestingPartieContainer, EnhancedContainer.NoContentWarnignVisibleProperty,
                vm => vm.InterestingPartieAvailable, converter: new InvertBooleanConverter());
            #endregion

            #region MyParties
            _myPartiesContainer.Content = _myPartieGallerie;
            _myPartieGallerie.ElementTapped += PartieSelected;
            BindToViewModel(_myPartiesContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToMyPartiesCommand);
            BindToViewModel(_myPartieGallerie, GallerieView.ItemSourceProperty, vm => vm.Selectedparties);
            BindToViewModel(_myPartiesContainer, EnhancedContainer.NoContentWarnignVisibleProperty,
                vm => vm.SelectedpartiesAvailable, converter: new InvertBooleanConverter());
            #endregion

            #region History
            _historyContainer.Content = _historyGallerieView;
            BindToViewModel(_historyContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToHistoryCommand);
            _historyGallerieView.ElementTapped += PartieSelected;
            BindToViewModel(_historyGallerieView, GallerieView.ItemSourceProperty, vm => vm.PartyHistory);
            BindToViewModel(_historyContainer, EnhancedContainer.NoContentWarnignVisibleProperty,
                vm => vm.PartyHistoryAvailable, converter: new InvertBooleanConverter());
            #endregion
            //Main layout
            var mainLayout = new Grid()
            {
                RowSpacing = 0,
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                },
                Children =
                {
                    new MapWrapper(_headerMap),
                    {_userInfoContainer, 0, 1},
                    {_myPartiesContainer, 0, 2},
                    {_interestingPartieContainer, 0, 3},
                    {_historyContainer, 0, 4}
                }
            };

            var mainScroll = new ScrollView
            {
                Content = mainLayout,
                Orientation = ScrollOrientation.Vertical
            };
            Content = mainScroll;
        }
        /// <summary>
        /// Opens seleted party with <see cref="PartyPreviewView"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="o"></param>
        private void PartieSelected(object sender, object o)
        {
            PreviewItemSelected<Party, PartyPreviewView>((Party) o, new object[] {Width, Height});
        }

        private int lastColumns = 1;
        /// <summary>
        /// Allocates size for <see cref="EnhancedContainer"/>. 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            //Handle big screens
            int columns = (int) Math.Ceiling((Width - 200)/300); //Available columns
            if (lastColumns != columns)
            {
                lastColumns = columns;
                _historyGallerieView.Columns = columns;
                _interestingPartieGallerie.Columns = columns;
                _myPartieGallerie.Columns = columns;
            }

            _profilePicture.HeightRequest = Width/2 - 20;
            _profilePicture.WidthRequest = Width/2 - 20;
        }
    }
}
