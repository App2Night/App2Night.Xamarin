using App2Night.CustomView.Page;
using App2Night.CustomView.Template;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.Page.SubPages
{
    public class MyPartysPage : CustomContentPage 
    {
        public MyPartysPage()
        {
            NoContentWarningMessage = "You did not note, accept or create a party yet."; 

            var listView = new ListView
            {
               IsPullToRefreshEnabled = true,
               ItemTemplate = new DataTemplate(typeof(PartyTemplate))
            };

            listView.SetBinding(ListView.ItemsSourceProperty, "MyParties");
            listView.SetBinding(ListView.SelectedItemProperty, "SelectedParty");
            listView.SetBinding(ListView.RefreshCommandProperty, "RefreshCommand");
            listView.SetBinding(ListView.IsRefreshingProperty, "IsRefreshing");

            this.SetBinding(CustomContentPage.ShowNoContentWarningProperty, "NoContentAvailable");

            Content = listView;
        }
    }
}