using App2Night.ViewModel;
using MvvmNano.Forms;

namespace App2Night.View
{
    public class EventPage : MvvmNanoContentPage<EventViewModel>
    {
        public EventPage()
        {
            BindToViewModel(this, TitleProperty, vm => vm.Party.Name);
        }
    }
}