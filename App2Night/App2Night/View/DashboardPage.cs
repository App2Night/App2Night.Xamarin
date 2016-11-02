using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App2Night.CustomPage;
using App2Night.CustomView;
using App2Night.ViewModel;
using MvvmNano.Forms;
using PartyUp.Model.Model;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App2Night.View
{
    public class DashboardPage : ContentPageWithPreview<DashboardViewModel>
    {
        Map _headerMap = new Map()
        {
            HeightRequest = 200
        };

        RoundImage profilePicture = new RoundImage(null)
        {
            HeightRequest = 100,
            WidthRequest = 100,
            Margin = new Thickness(10),
            EdgeColor = Color.Maroon
        }; 

        private GallerieView historyGallerieView = new GallerieView
        {
            MaxRows = 3,
            ElementSize = 100,
        };

        private GallerieView interestingPartieGallerie = new GallerieView
        {
            ElementSize = 150
        };

        private GallerieView myPartieGallerie = new GallerieView
        {
            ElementSize = 150
        };

        public DashboardPage()
        {
            //Dummy elements
            var dummys = new List<string>();
            for (int i = 0; i < 40; i++)
            {
                dummys.Add("Dummy " + (i + 1));
            }
            myPartieGallerie.ItemSource = dummys.Where(o => dummys.IndexOf(o) < 2);

            interestingPartieGallerie.ItemSource = dummys.Where(o => dummys.IndexOf(o) < 9);
            historyGallerieView.ItemSource = dummys;

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
            var usernameLabel = new Label() {Text = "Hans Peter XXL"};
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
                Name = "User",
                ButtonText = "Edit",
                Content = userInfoView
            };
            BindToViewModel(userInfoContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToUserEditCommand);


            //Interesting partie view
            var interestingPartieContainer = new EnhancedContainer
            {
                Name = "Events near you",
                ButtonText = "More",
                Content = interestingPartieGallerie
            };
            //BindToViewModel(interestingPartieGallerie, GallerieView.ItemSourceProperty, vm => vm.InterestingPartiesForUser);
            BindToViewModel(interestingPartieContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToPartyPicker);

            //Users parties view
            var myPartiesContainer = new EnhancedContainer
            {
                Name = "MyEvents",
                Content = myPartieGallerie
            };
            myPartieGallerie.ElementTapped +=PartieSelected;
                BindToViewModel(myPartiesContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToMyPartiesCommand);

            //Partie history
            var historyContainer = new EnhancedContainer
            {
                Name = "History",
                ButtonText = "More",
                Content = historyGallerieView
            };
            BindToViewModel(historyContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToHistoryCommand);


            //Main layout
            var mainLayout = new StackLayout()
            {
                Spacing = 0,
                Children =
                {
                    new MapWrapper(_headerMap),
                    new BoxView
                    {
                        Color = Color.Black,
                        HeightRequest = 1
                    },
                    userInfoContainer,
                    myPartiesContainer,
                    interestingPartieContainer,
                    historyContainer
                }
            };

            var mainScroll = new ScrollView
            {
                Content = mainLayout,
                Orientation = ScrollOrientation.Vertical
            };
            Grid.SetRowSpan(mainScroll, 2);

            Content = mainScroll;
        }

        private void PartieSelected(object sender, object o)
        {
            PreviewItemSelected<Party, PartyPreviewView>((Party) o, new object[] {Width, Height});
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            profilePicture.HeightRequest = Width/2 - 20;
            profilePicture.WidthRequest = Width/2 - 20;
        }

    }
}
