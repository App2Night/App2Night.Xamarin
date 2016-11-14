using MvvmNano;

namespace App2Night.ViewModel.Subpages
{
    public class ThirdPartyViewModel : MvvmNanoViewModel
    {
        private readonly string _testString = "Test für Third Party.";

        public string TestString
        {
            get
            {
                return _testString; 
                
            }
        }
    }
}