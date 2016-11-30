using App2Night.Model.Model;
using FreshMvvm;

namespace App2Night.PageModel.SubPages
{
    public class PartyDetailViewModel : FreshBasePageModel 
    {
        Party _party;
        public Party Party
        {
            get { return _party;}
            set { _party = value;  }
        }

        public PartyDetailViewModel(Party parameter)
        {
            Party = parameter;
        } 
    }
}