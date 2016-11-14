using App2Night.CustomView.Page;
using App2Night.View.Subpages;
using App2Night.ViewModel;
using MvvmNano.Forms;
using Xamarin.Forms;

namespace App2Night.View
{
    public class AboutTabbedPage : MvvmNanoTabbedPage<AboutTabbedViewModel>
    {
        public AboutTabbedPage()
        { 
            Children.Add(new AboutAppPage());
            Children.Add(new ThirdPartyPage());
            CurrentPage = Children[0]; 
        }
    }
}