﻿using App2Night.CustomView.Page;
using App2Night.ViewModel.Subpages;
using MvvmNano;
using Xamarin.Forms;

namespace App2Night.View.Subpages
{
    public class AboutAppPage : ContentPageWithInfo<AboutAppViewModel>
    {
        public AboutAppPage()
        {
            //Set up page
            SetViewModel(MvvmNanoIoC.Resolve<AboutAppViewModel>()); 
            Title = "About App"; 

            var label = new Label();
            BindToViewModel(label, Label.TextProperty, o => o.TestString);
            Content = label;
        }
    }
}