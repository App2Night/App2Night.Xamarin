using System.IO;
using System.Reflection;
using App2Night.Data.Language;
using App2Night.PageModel.SubPages;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.Page.SubPages
{
    public class AgbPage : FreshBaseContentPage
    {
        private Label _agbLabel = new Label();
        public AgbPage()
        {
            Title = AppResources.AgbHeader;

            _agbLabel.SetBinding(Label.TextProperty, nameof(AgbViewModel.AgbText));
            
            Content = new ScrollView {Content = _agbLabel , Padding = new Thickness(5)};
        }
    }
}