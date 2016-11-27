using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.Page
{
    public class PartyPage : FreshBaseContentPage
    {
        public PartyPage()
        {
            this.SetBinding(TitleProperty, "Party.Name");
        }
    }
}