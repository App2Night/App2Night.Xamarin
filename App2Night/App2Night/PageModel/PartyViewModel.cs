using App2Night.Model.Model;
using FreshMvvm;

namespace App2Night.PageModel
{
    public class PartyViewModel : FreshBasePageModel
    {
        public Party Party { get; private set; }

        public PartyViewModel(Party party)
        {
            Party = party;
        } 
    }
}
