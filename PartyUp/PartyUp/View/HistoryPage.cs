using MvvmNano.Forms;
using PartyUp.ViewModel;
using Xamarin.Forms;

namespace PartyUp.View
{
    public class HistoryPage : MvvmNanoContentPage<HistoryViewModel>
    {
        public HistoryPage()
        {
            var stackLayout = new StackLayout
            {
                Children =
                {
                    new CustomView.ListPartyView(),
                    new CustomView.ListPartyView(),
                    new CustomView.ListPartyView(),
                    new CustomView.ListPartyView(),
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
