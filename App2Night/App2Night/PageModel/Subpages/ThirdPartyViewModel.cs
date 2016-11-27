using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Acr.UserDialogs;
using App2Night.CustomView.Template;
using App2Night.Model.Model;
using App2Night.Service.Helper;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.PageModel.Subpages
{
    public class ThirdPartyViewModel : FreshBasePageModel
    {
        private License _selectedLicense;

        public IList<License> LicensesList { get; } = new List<License> 
        {
            new License
            {
                ProjectName = "MvvmNano",
                Description = "Small and smart MVVM framework made with ❤ for Xamarin.Forms.\n" +
                              "App2Night is using a modified version.",
                LicenseName = "The MIT License (MIT)",
                ProjectUri = new Uri("https://github.com/aspnetde/MvvmNano"),
                LicenseFileName = "MvvmNano"
            },
            new License
            {
                ProjectName = "Acr.UserDialogs",
                LicenseName = "Microsoft Public License (MS-PL)",
                ProjectUri = new Uri("https://github.com/aritchie/userdialogs"),
                LicenseFileName = "UserDialogs"
            },
            new License
            {
                ProjectName = "PropertyChanged.Fody",
                Description = "Injects INotifyPropertyChanged code into properties at compile time.",
                LicenseName = "The MIT License (MIT)",
                ProjectUri = new Uri("https://github.com/Fody/PropertyChanged"),
                LicenseFileName = "MIT"
            },
            new License
            {
                ProjectName = "Xam.Plugin",
                Description = "Xam.Plugin is the namespace of multiple Xamarin.Forms Plugins created by James Montemagno.\n" +
                              "App2Night is using the Xam.Plugin.Connectivity and Xam.Plugin.Geolocator packages.",
                LicenseName = "The MIT License (MIT)",
                ProjectUri = new Uri("https://github.com/jamesmontemagno/Xamarin.Plugins"),
                LicenseFileName = "XamPlugin"
            },
            new License
            {
                ProjectName = "Newtonsoft.Json",
                Description = "Json.Net is a popular high-performance JSON framework for .Net.",
                LicenseName = "The MIT License (MIT)",
                ProjectUri = new Uri("https://github.com/JamesNK/Newtonsoft.Json"),
                LicenseFileName = "JsonNet"
            }
        };

        
        public License SelectedLicense
        {
            get { return _selectedLicense; }
            set
            {
                if (value != null) 
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        _selectedLicense = value;
                        await CoreMethods.PushPageModel<ThirdPartyInfoViewModel>(_selectedLicense);
                        SelectedLicense = null;
                    }); 
                }
            }
        }

        public ThirdPartyViewModel()
        { 
            using (UserDialogs.Instance.Loading())
            {
                var ioWatch = new Stopwatch();
                ioWatch.Start();
                var tasks = LicensesList.Select(license => Task.Run(() =>
                {
                    try
                    {
                        var assembly = typeof (QuadraticPartyTemplate).GetTypeInfo().Assembly;
                        Stream stream = assembly.GetManifestResourceStream("App2Night.Data.Licenses." + license.LicenseFileName + ".txt");
                        using (var reader = new StreamReader(stream))
                        {
                            license.LicenseText = reader.ReadToEnd();
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                })).ToList();
                Task.WaitAll(tasks.ToArray());
                ioWatch.PrintTime("Reading license files");
            } 
        } 
    }
}