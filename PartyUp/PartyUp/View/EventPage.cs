using MvvmNano.Forms;
using PartyUp.ViewModel;

namespace PartyUp.View
{
    public class EventPage : MvvmNanoContentPage<EventViewModel>
    {
        public EventPage()
        {
            BindToViewModel(this, TitleProperty, vm => vm.Party.Name);
        }
    }
}