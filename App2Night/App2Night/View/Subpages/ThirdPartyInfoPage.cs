using App2Night.CustomView.View;
using App2Night.Helper.ValueConverter;
using App2Night.ViewModel.Subpages;
using MvvmNano.Forms;
using Xamarin.Forms;

namespace App2Night.View.Subpages
{
    public class ThirdPartyInfoPage : MvvmNanoContentPage<ThirdPartyInfoViewModel>
    {
        public ThirdPartyInfoPage()
        {
            BindToViewModel(this, TitleProperty, vm => vm.License.ProjectName);

            //Toolbar item for project site redirection
            var toolbarItem = new ToolbarItem
            {
                Text = "Website"
            };
            ToolbarItems.Add(toolbarItem);
            BindToViewModel(toolbarItem, MenuItem.CommandProperty, vm => vm.OpenWebsiteCommand);

            //Label that displays the project description
            var descriptionLabel = new Label
            {
                VerticalTextAlignment = TextAlignment.Start,
                HorizontalTextAlignment = TextAlignment.Start,
                Margin = 5
            };
            BindToViewModel(descriptionLabel, Label.TextProperty, vm => vm.License.Description);
            BindToViewModel(descriptionLabel, IsVisibleProperty, vm => vm.License.Description, converter: new StringNotEmptyConverter());

            //Label that displays the license text.
            var licenseLabel = new Label
            { 
                VerticalTextAlignment = TextAlignment.Start,
                HorizontalTextAlignment = TextAlignment.Start,
                Margin = 5
            };
            BindToViewModel(licenseLabel, Label.TextProperty, vm => vm.License.LicenseText);

            //Container that contains (haha) the licenseLabel.
            var licenseContainer = new EnhancedContainer
            {
                Content = licenseLabel
            };
            BindToViewModel(licenseContainer, EnhancedContainer.NameProperty, vm => vm.License.LicenseName); 

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