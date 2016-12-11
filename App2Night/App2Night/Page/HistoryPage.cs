using App2Night.CustomView.Page;
using App2Night.CustomView.Template;
using App2Night.CustomView.View;
using App2Night.Model.Model;
using App2Night.PageModel;
using App2Night.ValueConverter;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.Page
{
    public class HistoryPage : CustomContentPage 
    {
        /// <summary>
        /// Lists the Parties in a ListView. 
        /// </summary>
        public HistoryPage()
        {
            Title = "History";
            NoContentWarningMessage = "You did not participate a party yet.";

            var listView = new ListView(ListViewCachingStrategy.RecycleElement)
            {
                RowHeight = 150,
                IsPullToRefreshEnabled = true,
                ItemTemplate = new DataTemplate(typeof(HistoryTemplate))
            };
            listView.SetBinding(ListView.ItemsSourceProperty, "Parties");
            listView.SetBinding(ListView.RefreshCommandProperty, "RefreshCommand");
            listView.SetBinding(ListView.IsRefreshingProperty, "IsRefreshing");

            listView.ItemTapped += PartieSelected;
            var mainScroll = new ScrollView
            {
                Content = listView,
                Orientation = ScrollOrientation.Vertical
            };
            Content = mainScroll;

            this.SetBinding(CustomContentPage.ShowNoContentWarningProperty, nameof(HistoryViewModel.PartyAvailable), converter: new InvertBooleanConverter());
        }

        /// <summary>
        /// Gets fired if a partie item gets tapped on. 
        /// Focus of the Item is null.
        /// Will display the party info view.
        /// </summary> 
        private void PartieSelected(object sender, object o)
        {
            var listView = (ListView)sender;

            if (listView.SelectedItem != null)
            {
                var party = (Party)listView.SelectedItem;
                PreviewItemSelected<Party, PartyPreviewView>(party, new object[] {Width, Height});
                listView.SelectedItem = null; 
            }
        }
    }
}
