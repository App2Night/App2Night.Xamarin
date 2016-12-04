using System.IO;
using System.Reflection;
using App2Night.Data.Language;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.Page.SubPages
{
    public class AgbPage : FreshBaseContentPage
    {
        public AgbPage()
        {
            Title = AppResources.AgbHeader;

            var assembly = typeof(AgbPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("App2Night.Data.Licenses.AGB.txt");
            string text = "";
            using (var reader = new System.IO.StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
            Content = new ScrollView {Content = new Label {Text = text} , Padding = new Thickness(5)};
        }
    }
}