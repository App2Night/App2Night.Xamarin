using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.Page
{
    public class ThirdPartyPage : FreshBaseContentPage
    {
        public ThirdPartyPage()
        { 
          

            Title = "Third party librarys"; 

            var template = new DataTemplate(() =>
            {
                var text = new TextCell();
                text.SetBinding(TextCell.TextProperty, "ProjectName");
                return text;
            });

            var libraryLicensesListView = new ListView
            { 
                ItemTemplate = template,
                HeightRequest = 40
            };

            libraryLicensesListView.SetBinding( ListView.ItemsSourceProperty, "LicensesList");
            libraryLicensesListView.SetBinding(  ListView.SelectedItemProperty, "SelectedLicense");

            Content = libraryLicensesListView;
        }
    }
}