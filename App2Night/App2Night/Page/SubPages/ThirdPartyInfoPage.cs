using App2Night.CustomView.View;
using App2Night.Data.Language;
using App2Night.ValueConverter;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.Page.SubPages
{
    public class ThirdPartyInfoPage : FreshBaseContentPage 
    {
        public ThirdPartyInfoPage()
        {
            this.SetBinding( TitleProperty, "License.ProjectName");

            //Toolbar item for project site redirection
            var toolbarItem = new ToolbarItem
            {
                Text = AppResources.Website 
            };
            ToolbarItems.Add(toolbarItem);
            toolbarItem.SetBinding( MenuItem.CommandProperty, "OpenWebsiteCommand");

            //Label that displays the project description
            var descriptionLabel = new Label
            {
                VerticalTextAlignment = TextAlignment.Start,
                HorizontalTextAlignment = TextAlignment.Start,
                Margin = 5
            };
            descriptionLabel.SetBinding( Label.TextProperty, "License.Description");
            descriptionLabel.SetBinding( IsVisibleProperty, "License.Description", converter: new StringNotEmptyConverter());

            //Label that displays the license text.
            var licenseLabel = new Label
            { 
                VerticalTextAlignment = TextAlignment.Start,
                HorizontalTextAlignment = TextAlignment.Start,
                Margin = 5
            };
            licenseLabel.SetBinding( Label.TextProperty, "License.LicenseText");

            //Container that contains (haha) the licenseLabel.
            var licenseContainer = new EnhancedContainer
            {
                Content = licenseLabel
            };
            licenseContainer.SetBinding( EnhancedContainer.NameProperty, "License.LicenseName"); 

            Content = new ScrollView
            {
                Orientation = ScrollOrientation.Vertical, 
                Content = new StackLayout
                {
                    Spacing = 0,
                    Children =
                    {
                        descriptionLabel,
                        licenseContainer
                    }
                }
            };
        }
    }
}