using System;
using System.Diagnostics;

namespace App2Night.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();


            try
            {
                LoadApplication(new App2Night.App());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}
