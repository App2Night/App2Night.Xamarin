using System;
using System.Collections.Generic;
using App2Night.Model.Model;
using App2Night.ViewModel.Subpages;
using MvvmNano;
using MvvmNano.Forms;
using Xamarin.Forms;

namespace App2Night.View.Subpages
{
    public class ThirdPartyPage : MvvmNanoContentPage<ThirdPartyViewModel>
    {
        public ThirdPartyPage()
        { 
            SetViewModel(MvvmNanoIoC.Resolve<ThirdPartyViewModel>());

            Title = "Third party librarys"; 

            var template = new DataTemplate(() =>
            {
                var text = new TextCell();
                text.SetBinding(TextCell.TextProperty, "ProjectName");
                return text;
            });

            var libraryLicensesListView = new ListView
            { 
                ItemTemplate = template,
                HeightRequest = 40
            };

            BindToViewModel(libraryLicensesListView, ListView.ItemsSourceProperty, vm => vm.LicensesList);
            BindToViewModel(libraryLicensesListView, ListView.SelectedItemProperty, vm => vm.SelectedLicense);

            Content = libraryLicensesListView;
        }
    }
}