using System;
using Xamarin.Forms;

namespace App2Night.CustomView
{
    public class PreviewView : ContentView
    {
        public event EventHandler CloseViewEvent;

        public void CloseView()
        {
            if(CloseViewEvent!=null)
                CloseViewEvent(this, EventArgs.Empty);
        }
    }
}