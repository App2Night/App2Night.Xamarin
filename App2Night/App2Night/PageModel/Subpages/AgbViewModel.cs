using System.IO;
using System.Reflection;
using App2Night.Page.SubPages;
using FreshMvvm;
using PropertyChanged;

namespace App2Night.PageModel.SubPages
{
    [ImplementPropertyChanged]
    public class AgbViewModel : FreshBasePageModel
    {
        private string _agbText = "";
        public string AgbText
        {
           get { return _agbText; }
        }

        private void LoadAgb()
        {
            var assembly = typeof(AgbPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("App2Night.Data.Licenses.AGB.txt");
            using (var reader = new System.IO.StreamReader(stream))
            {
                _agbText = reader.ReadToEnd();
            }
        }

        public AgbViewModel()
        {
            LoadAgb();
        }
    }
}