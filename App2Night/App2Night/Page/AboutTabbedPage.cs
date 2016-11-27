using App2Night.Page.SubPages;
using App2Night.PageModel.SubPages;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.Page
{
    public sealed class AboutTabbedPage : TabbedPage
    {
        public AboutTabbedPage()  
        {
            var aboutAppPage = new AboutAppPage {BindingContext = new AboutAppViewModel()};
            var thirdPartyPage = new ThirdPartyPage {BindingContext = new ThirdPartyViewModel()};

            Children.Add(aboutAppPage);
            Children.Add(thirdPartyPage);
             
            SelectedItem = Children[0];
        }
    }
}