using App2Night.CustomView.Page;
using App2Night.CustomView.Template;
using App2Night.CustomView.View;
using App2Night.Model.Model;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.Page
{
    public class HistoryPage : FreshBaseContentPage 
    {
        /// <summary>
        /// Lists the Parties in a ListView. 
        /// </summary>
        public HistoryPage()
        {
            Title = "History";
            var listView = new ListView(ListViewCachingStrategy.RecycleElement)
            {
                RowHeight = 150,

                ItemTemplate = new DataTemplate(typeof(HistoryTemplate))
            };
            listView.SetBinding(ListView.ItemsSourceProperty, "Parties");
            listView.ItemTapped += PartieSelected;
            var mainScroll = new ScrollView
            {
                Content = listView,
                Orientation = ScrollOrientation.Vertical
            };
            Content = mainScroll;
        }

        /// <summary>
        /// Gets fired if a partie item gets tapped on. 
        /// Focus of the Item is null.
        /// Will display the party info view.
        /// </summary> 
        private void PartieSelected(object sender, object o)
        {
            var listView = (ListView) sender;
            var party = (Party) listView.SelectedItem;
            listView.SelectedItem = null;
            //PreviewItemSelected<Party, PartyPreviewView>(party, new object[] {Width, Height});
        }
    }
}
