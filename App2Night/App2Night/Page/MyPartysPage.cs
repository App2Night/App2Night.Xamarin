using System;
using App2Night.CustomView.Page;
using App2Night.CustomView.Template;
using App2Night.CustomView.View;
using App2Night.Model.Model;
using Xamarin.Forms;

namespace App2Night.Page
{
    public class MyPartysPage : CustomContentPage
    {
        private ListView _partyListView;

        public MyPartysPage()
        {
            NoContentWarningMessage = "You did not note, accept or create a party yet."; 

            //Switch to history toolbar icon
            var historyToolbarIcon = new ToolbarItem
            {
                Text = "History" 
            };
            historyToolbarIcon.SetBinding(MenuItem.CommandProperty, "NavigateToHistoryCommand");
            ToolbarItems.Add(historyToolbarIcon);

            _partyListView = new ListView(ListViewCachingStrategy.RecycleElement)
            {
               IsPullToRefreshEnabled = true,
               ItemTemplate = new DataTemplate(typeof(PartyTemplate)),
               RowHeight = 150
            };

            _partyListView.SetBinding(ListView.ItemsSourceProperty, "MyParties"); 
            _partyListView.SetBinding(ListView.RefreshCommandProperty, "RefreshCommand");
            _partyListView.SetBinding(ListView.IsRefreshingProperty, "IsRefreshing");

            //Open preview page if party is selected
            _partyListView.ItemSelected += PartyListViewItemSelected; 

            this.SetBinding(ShowNoContentWarningProperty, "NoContentAvailable");

            Content = _partyListView;
        }

        private void PartyListViewItemSelected(object sender, SelectedItemChangedEventArgs selectedItemChangedEventArgs)
        {
            if (_partyListView.SelectedItem != null)
            {
                var selectedParty = (Party) _partyListView.SelectedItem;
                PreviewItemSelected<Party, PartyPreviewView>(selectedParty, new object[] {Width, Height});
                _partyListView.SelectedItem = null;
            }
        }
    }
}