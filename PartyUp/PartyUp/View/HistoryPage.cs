using MvvmNano.Forms;
using PartyUp.CustomView;
using PartyUp.Model.Model;
using PartyUp.ViewModel;
using Xamarin.Forms;

namespace PartyUp.View
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
