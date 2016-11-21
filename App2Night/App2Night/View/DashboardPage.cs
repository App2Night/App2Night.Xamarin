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
        Map _headerMap = new Map()
        {
            HeightRequest = 200
        };

        CachedImage profilePicture = new CachedImage()
        {
            HeightRequest = 100,
            WidthRequest = 100,
            Margin = new Thickness(10) 
        }; 

        private HorizontalGallerieView historyGallerieView = new HorizontalGallerieView
        {
            Columns = 2,
            Rows = 1, 
            Template = typeof(QuadraticPartyTemplate)
        };

        private HorizontalGallerieView interestingPartieGallerie = new HorizontalGallerieView
        {
            Columns = 1,
            Template = typeof(QuadraticPartyTemplate)
        }; 

        private HorizontalGallerieView myPartieGallerie = new HorizontalGallerieView
        {
            Columns = 1,
            Template = typeof(QuadraticPartyTemplate)
        }; 

        public DashboardPage()
        { 
            profilePicture.SetImage("App2Night.Data.Image.default.png", SourceOrigin.Resource);

            //User info view
            var userInfoView = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
                },
                Children =
                {
                    profilePicture 
                } 
                
            };
            var usernameLabel = new Label();
            var joinedAtLabel = new Label() {Text = "Dabei seit: 30.10.2016"};
            var profileDetails = new List<Xamarin.Forms.View>
            {
                usernameLabel,
                joinedAtLabel
            };
            userInfoView.RowDefinitions.Add(new RowDefinition {Height = new GridLength(1, GridUnitType.Star)});
            for (int index = 0; index < profileDetails.Count; index++)
            {
                Xamarin.Forms.View view = profileDetails[index];
                userInfoView.RowDefinitions.Add(new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)});
                userInfoView.Children.Add(view, 1, index + 1);
            }
            userInfoView.RowDefinitions.Add(new RowDefinition {Height = new GridLength(1, GridUnitType.Star)});
            Grid.SetRowSpan(profilePicture, profileDetails.Count + 2);

            EnhancedContainer userInfoContainer = new EnhancedContainer
            {
				Name = AppResources.User,
                ButtonText = "\uf0ad",
                Content = userInfoView,
                NoContentWarningVisible = false,
                TopSpacerVisible = false
            }; 
            BindToViewModel(userInfoContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToUserEditCommand);

            //Interesting partie view
            var interestingPartieContainer = new EnhancedContainer
            {
				Name = AppResources.IntParty, 
                Content = interestingPartieGallerie,
				ContentMissingText = AppResources.ContIntParty
            };
            interestingPartieGallerie.ElementTapped += PartieSelected;
            BindToViewModel(interestingPartieGallerie, GallerieView.ItemSourceProperty, vm => vm.InterestingPartiesForUser);
            BindToViewModel(interestingPartieContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToPartyPicker);
            BindToViewModel(interestingPartieContainer, EnhancedContainer.NoContentWarnignVisibleProperty, vm => vm.InterestingPartieAvailable, converter: new InvertBooleanConverter());

            //Users parties view
            var myPartiesContainer = new EnhancedContainer
            {
				Name = AppResources.MyParty,
                Content = myPartieGallerie,
				ContentMissingText = AppResources.ContMyParty
            };
            myPartieGallerie.ElementTapped +=PartieSelected;
            BindToViewModel(myPartiesContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToMyPartiesCommand);
            BindToViewModel(myPartieGallerie, GallerieView.ItemSourceProperty, vm => vm.Selectedparties);
            BindToViewModel(myPartiesContainer, EnhancedContainer.NoContentWarnignVisibleProperty, vm => vm.SelectedpartiesAvailable, converter: new InvertBooleanConverter());

            //Partie history
            var historyContainer = new EnhancedContainer
            {
				Name = AppResources.History, 
                Content = historyGallerieView,
				ContentMissingText = AppResources.ContHistory
            };
            BindToViewModel(historyContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToHistoryCommand);
            historyGallerieView.ElementTapped += PartieSelected;
            BindToViewModel(historyGallerieView, GallerieView.ItemSourceProperty, vm => vm.PartyHistory);
            BindToViewModel(historyContainer, EnhancedContainer.NoContentWarnignVisibleProperty, vm => vm.PartyHistoryAvailable, converter: new InvertBooleanConverter());

            //Main layout
            var mainLayout = new Grid()
            {  
                RowSpacing = 0,
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)},

                },
                Children =
                {
                    new MapWrapper(_headerMap),
                    {userInfoContainer, 0, 1},
                    { myPartiesContainer,0, 2},
                    { interestingPartieContainer,0,3},
                    { historyContainer,0, 4}
                } 
            };

            var mainScroll = new ScrollView
            {
                Content = mainLayout,
                Orientation = ScrollOrientation.Vertical
            }; 
            Content = mainScroll;
        }

        private void PartieSelected(object sender, object o)
        {
            PreviewItemSelected<Party, PartyPreviewView>((Party) o, new object[] {Width, Height});
        }

        private int lastColumns = 1;
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            //Handle big screens
            int columns =  (int) Math.Ceiling((Width-200)/300); //Available columns
            if (lastColumns != columns)
            {
                lastColumns = columns;
                historyGallerieView.Columns = columns;
                interestingPartieGallerie.Columns = columns;
                myPartieGallerie.Columns = columns;
            }

            profilePicture.HeightRequest = Width/2 - 20;
            profilePicture.WidthRequest = Width/2 - 20; 
        }

    }
}
