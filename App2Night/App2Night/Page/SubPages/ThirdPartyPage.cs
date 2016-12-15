
using App2Night.Model.Language;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.Page.SubPages
{
    public class ThirdPartyPage : FreshBaseContentPage
    {
        public ThirdPartyPage()
        {  
			Title = AppResources.ThirdPartyTitle; 

            var template = new DataTemplate(() =>
            {
                var text = new TextCell();
                text.SetBinding(TextCell.TextProperty, "ProjectName");
                return text;
            });

            var libraryLicensesListView = new ListView(ListViewCachingStrategy.RecycleElement)
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