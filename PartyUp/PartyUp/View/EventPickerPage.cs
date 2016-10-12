using MvvmNano.Forms;
using PartyUp.ViewModel;
using Xamarin.Forms;

namespace PartyUp.View
{
    public class EventPickerPage : MvvmNanoContentPage<EventPickerViewModel>
    {
        public EventPickerPage()
        {
            var eventListView = new ListView(ListViewCachingStrategy.RecycleElement);
            BindToViewModel(eventListView, ListView.ItemsSourceProperty, vm => vm.Events);
            BindToViewModel(eventListView, ListView.SelectedItemProperty, vm => vm.SelectedEvent);

            eventListView.ItemTemplate  = new DataTemplate(() =>
            {
                var nameCell = new TextCell();
                nameCell.SetBinding(TextCell.TextProperty, "Name"); 
                return nameCell;
            }); 



            Content = eventListView;
        }
    }
}
