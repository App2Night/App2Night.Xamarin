using App2Night.CustomView;
using App2Night.ViewModel;
using MvvmNano.Forms;
using PartyUp.Model.Model;
using Xamarin.Forms;

namespace App2Night.View
{
    public class HistoryPage : MvvmNanoContentPage<HistoryViewModel>
    {
        public HistoryPage()
        {
            var stackLayout = new ListView()
            {
                ItemTemplate = new DataTemplate(typeof(PartyTemplate)),
                ItemsSource = new Party[]
                {
                    new Party(),
                    new Party(),
                    new Party(),
                    new Party(),
                    new Party(),
                }
            };
            Title = "History";
            Content = new ScrollView
            {
                Content = stackLayout,
                Orientation = ScrollOrientation.Vertical
            };
        }
    }
}
