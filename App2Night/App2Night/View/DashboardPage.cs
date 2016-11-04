﻿using System.Collections.Generic;
using System.Linq;
using App2Night.CustomPage;
using App2Night.CustomView;
using App2Night.CustomView.Template;
using App2Night.CustomView.View;
using App2Night.ViewModel;
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

        RoundImage profilePicture = new RoundImage("App2Night.Data.Image.default.png")
        {
            HeightRequest = 100,
            WidthRequest = 100,
            Margin = new Thickness(10),
            EdgeColor = Color.Maroon
        }; 

        private HorizontalGallerieView historyGallerieView = new HorizontalGallerieView
        {
            Columns = 3,
            Rows = 2, 
            Template = typeof(QuadraticPartyTemplate)
        };

        private HorizontalGallerieView interestingPartieGallerie = new HorizontalGallerieView
        {
            Columns = 2,
            Template = typeof(QuadraticPartyTemplate)
        }; 

        private HorizontalGallerieView myPartieGallerie = new HorizontalGallerieView
        {
            Columns = 2,
            Template = typeof(QuadraticPartyTemplate)
        }; 

        public DashboardPage()
        { 
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
                ButtonText = "\uf0ad",
                Content = userInfoView
            };
            BindToViewModel(userInfoContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToUserEditCommand);

            //Interesting partie view
            var interestingPartieContainer = new EnhancedContainer
            {
                Name = "Events near you", 
                Content = interestingPartieGallerie
            };
            interestingPartieGallerie.ElementTapped += PartieSelected;
            BindToViewModel(interestingPartieGallerie, GallerieView.ItemSourceProperty, vm => vm.InterestingPartiesForUser);
            BindToViewModel(interestingPartieContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToPartyPicker);

            //Users parties view
            var myPartiesContainer = new EnhancedContainer
            {
                Name = "MyEvents",
                Content = myPartieGallerie
            };
            myPartieGallerie.ElementTapped +=PartieSelected;
            BindToViewModel(myPartiesContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToMyPartiesCommand);
            BindToViewModel(myPartieGallerie, GallerieView.ItemSourceProperty, vm => vm.NextPartiesForUser);

            //Partie history
            var historyContainer = new EnhancedContainer
            {
                Name = "History", 
                Content = historyGallerieView
            };
            BindToViewModel(historyContainer, EnhancedContainer.CommandProperty, vm => vm.MoveToHistoryCommand);
            historyGallerieView.ElementTapped += PartieSelected;
            BindToViewModel(historyGallerieView, GallerieView.ItemSourceProperty, vm => vm.PartyHistory);

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