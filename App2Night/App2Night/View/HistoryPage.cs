using System;
using System.Threading.Tasks;
using App2Night.CustomPage;
using App2Night.CustomView;
using App2Night.ViewModel;
using MvvmNano.Forms;
using PartyUp.Model.Enum;
using PartyUp.Model.Model;
using Xamarin.Forms;

namespace App2Night.View
{
    public class HistoryPage : ContentPageWithPreview<HistoryViewModel>
    {
        public HistoryPage()
        {
            Title = "History";
            var listView = new ListView()
            {
                RowHeight = 100,
                ItemTemplate = new DataTemplate(typeof(PartyTemplate)),
                ItemsSource = new Party[]
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
        /// Will display the party info view.
        /// </summary> 
        private async void PartieSelected(object sender, object o)
        {
            var listView = (ListView) sender;
            var party = (Party) listView.SelectedItem;
            PreviewItemSelected<Party, PartyPreviewView>(party, new object[] {Width, Height});
        }
    }
}
