using MvvmNano.Forms;
using PartyUp.CustomView;
using PartyUp.ViewModel;
using Xamarin.Forms;

namespace PartyUp.View
{
    public class EventPickerPage : MvvmNanoContentPage<EventPickerViewModel>
    {
        public EventPickerPage()
        {
            //var eventListView = new ListPartyView(ListViewCachingStrategy.RecycleElement);
            //BindToViewModel(eventListView, ListPartyView.ItemsSourceProperty, vm => vm.Events);
            //BindToViewModel(eventListView, ListPartyView.SelectedItemProperty, vm => vm.SelectedParty);

            //eventListView.ItemTemplate  = new DataTemplate(() =>
            //{
            //    var nameCell = new TextCell();
            //    nameCell.SetBinding(TextCell.TextProperty, "Name"); 
            //    return nameCell;
            //}); 



            //Content = eventListView;

            var swipeView = new SwipeView();
            BindToViewModel(swipeView, SwipeView.ItemSourceProperty, vm => vm.Events);
            Content = swipeView;
        }
    }
}
