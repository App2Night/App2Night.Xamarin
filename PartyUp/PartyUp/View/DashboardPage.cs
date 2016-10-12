﻿using MvvmNano.Forms;
using PartyUp.ViewModel;
using Xamarin.Forms;

namespace PartyUp.View
{
    public class DashboardPage : MvvmNanoContentPage<DashboardViewModel>
    {
        public DashboardPage()
        {
            var profilePictureView = new ContentView()
            {
                BackgroundColor = Color.Gray
            };

            var lineBox = new BoxView {Color = Color.Aqua};

            var moreInfoLabel = new Label
            {
                Text = "More infos here."
            };

            var mainLayout = new Grid
            {
                Padding = new Thickness(0, 20,0,0),
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Absolute)}, 
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition { Height = new GridLength(4, GridUnitType.Star)}, 
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star)},
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)}, 
                },
                Children =
                {
                     {lineBox,0,1 },
                    { profilePictureView,1,0},
                   
                    {moreInfoLabel, 0, 3 }
                }
            }; 
            Grid.SetRowSpan(profilePictureView, 3);
            Grid.SetColumnSpan(lineBox, 3);
            Grid.SetColumnSpan(moreInfoLabel, 3); 
            Content = mainLayout;
        }
    }
}