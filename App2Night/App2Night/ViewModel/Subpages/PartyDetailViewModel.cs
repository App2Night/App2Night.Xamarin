using App2Night.Model.Model;
using MvvmNano;

namespace App2Night.ViewModel.Subpages
{
    public class PartyDetailViewModel : MvvmNanoViewModel<Party>
    {
        Party _party;
        public Party Party
        {
            get { return _party;}
            set { _party = value;  }
        }

        public override void Initialize(Party parameter)
        {
            base.Initialize(parameter);
            Party = parameter;
        }
    }
}