using MvvmNano;
using PartyUp.Model;
using PartyUp.Model.Model;

namespace PartyUp.ViewModel
{
    public class EventViewModel : MvvmNanoViewModel<Event>
    {
        public Event Event { get; private set; }

        public override void Initialize(Event pEvent)
        {
            base.Initialize(pEvent);
            Event = pEvent;
        }
    }
}
