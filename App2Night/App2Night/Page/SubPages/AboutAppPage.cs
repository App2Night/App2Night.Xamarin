using System.Collections.Generic;
using App2Night.CustomView.View;
using App2Night.Model.Language;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.Page.SubPages
{
    public class AboutAppPage : FreshBaseContentPage
    {
        public AboutAppPage()
        { 
            Title = AppResources.AboutTitle;  

            var generalViewSize = 150;
            var iconImage = new Image
            {
                Source = ImageSource.FromResource("App2Night.Data.Image.icon.png"),
                Margin = new Thickness(8)
            }; 
            var generalView = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(generalViewSize * 0.66, GridUnitType.Absolute)},
                },
                Children =
                {
                    new Label
                    {
						Text = AppResources.GeneralInfo,
                        Margin = new Thickness(15, 5)

                    },
                    { iconImage, 1, 0 }
                }
            };

            var projectViewHeight = 120;
            var projectLabel = new Label
            {
				Text = AppResources.GeneralContent,
                Margin = new Thickness(15, 5)
            };
            var githubIconLabel = new CustomButton
            {
                FontFamily = "FontAwesome",
                Text = "\uf09b",
                FontSize = 80,
                Margin = new Thickness(5)
            };
            githubIconLabel.SetBinding(CustomButton.CommandProperty, "OpenProjectSiteCommand");

            var projectView = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(projectViewHeight, GridUnitType.Absolute)},
                },
                Children =
                {
                    projectLabel,
                    {githubIconLabel, 1, 0 }
                }
            };

            var tableRoot = new TableRoot
            {
                new List<TableSection>
                { 
					new TableSection(AppResources.General) //General
                    {
                        new ViewCell { View = generalView, Height = generalViewSize}
                    },
					new TableSection(AppResources.Project) //Project
                    {
                        new ViewCell { View = projectView, Height = projectViewHeight}
                    },
                },
               
            };

            Content = new TableView(tableRoot)
            {
                HasUnevenRows = true
            };
        }
    }
}