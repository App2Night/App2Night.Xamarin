using MvvmNano;
using PartyUp.Model;
using PartyUp.Model.Model;

namespace PartyUp.ViewModel
{
    public class EventViewModel : MvvmNanoViewModel<Party>
    {
        public Party Party { get; private set; }

        public override void Initialize(Party pParty)
        {
            base.Initialize(pParty);
            Party = pParty;
        }
    }
}
