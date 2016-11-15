using System.Collections.Generic;
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
            //Set up page
            SetViewModel(MvvmNanoIoC.Resolve<ThirdPartyViewModel>());
            Title = "Third Party";
            var label = new Label();

            TableView layoutView = new TableView
            {
                HasUnevenRows = true
            };
            var root = new TableRoot("Test");
            var tableOne = new TableSection("TEst1");
            tableOne.Add(new ViewCell
            {
                View = new Label { Text = "Das ist ein Test LAbel!!!"}
            });
            tableOne.Add(new ViewCell
            {
                View = new BoxView { Color = Color.Red, HeightRequest =  200}
            });
            var tableTwo = new TableSection("TEst2");
            tableTwo.Add(new ViewCell
            {
                View = new Label { Text = "Das ist ein Test LAbel!!!" }
            });
            tableTwo.Add(new ViewCell
            {
                View = label
            });
            root.Add(tableOne);
            root.Add(tableTwo);  
            layoutView.Root = root; 

            BindToViewModel(label, Label.TextProperty, o => o.TestString);
            Content = layoutView;
        }
    }
}