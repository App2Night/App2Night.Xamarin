using FreshMvvm;

namespace App2Night.Page
{
    public class AboutTabbedPage : FreshTabbedNavigationContainer
    {
        public AboutTabbedPage()
        {  
            Children.Add(new AboutAppPage());
            Children.Add(new ThirdPartyPage());
            CurrentPage = Children[0]; 
        }
    }
}