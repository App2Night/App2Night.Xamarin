using App2Night.CustomView.Page;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.Page
{
    public class PartyPage : CustomContentPage
    {
        public PartyPage()
        {
            this.SetBinding(TitleProperty, "Party.Name");
        }
    }
}