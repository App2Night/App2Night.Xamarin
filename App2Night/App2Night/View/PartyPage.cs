using App2Night.ViewModel;
using MvvmNano.Forms;

namespace App2Night.View
{
    public class PartyPage : MvvmNanoContentPage<PartyViewModel>
    {
        public PartyPage()
        {
            BindToViewModel(this, TitleProperty, vm => vm.Party.Name);
        }
    }
}