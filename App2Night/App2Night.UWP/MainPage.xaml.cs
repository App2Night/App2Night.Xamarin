namespace PartyUp.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new App2Night.App());
        }
    }
}
