using System;
using System.Threading.Tasks;
using App2Night.CustomView;
using App2Night.CustomView.Page;
using App2Night.CustomView.Template;
using App2Night.CustomView.View;
using App2Night.Model.Enum;
using App2Night.Model.Model;
using App2Night.ViewModel;
using MvvmNano.Forms;
using Xamarin.Forms;

namespace App2Night.View
{
    public class HistoryPage : ContentPageWithPreview<HistoryViewModel>
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

                ItemTemplate = new DataTemplate(typeof(HistoryTemplate)),
                ItemsSource = new []
                {
                    new Party
                    {
                        Name = "DH goes Party",
                        MusicGenre = MusicGenre.Rock,
                        Date = DateTime.Now.AddDays(30)
                    },
                    new Party(),
                    new Party(),
                    new Party(),
                    new Party(),
                    new Party(),
                    new Party(),
                    new Party(),
                    new Party(),
                }
            };
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
            PreviewItemSelected<Party, PartyPreviewView>(party, new object[] {Width, Height});
        }
    }
}
