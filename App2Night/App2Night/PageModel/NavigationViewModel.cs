using System.Threading.Tasks;
using Acr.UserDialogs;
using App2Night.Model.Model;
using App2Night.PageModel.SubPages;
using App2Night.Service.Interface;
using FreshMvvm;
using PropertyChanged;
using Xamarin.Forms;

namespace App2Night.PageModel
{
    [ImplementPropertyChanged]
    public class NavigationViewModel : FreshBasePageModel
    {
        private readonly IStorageService _storageService;
        private IDataService _dataService;
        private bool _isLogIn;

        public Model.Model.User User => FreshIOC.Container.Resolve<IDataService>().User;

        public bool IsLogIn
        {
            get { return _isLogIn; }
            private set
            {
                _isLogIn = value;
                if (value == true)
                {
                    IsLogInContentView = true;
                    IsLogOutContentView = false;
                }
                else
                {
                    IsLogInContentView = false;
                    IsLogOutContentView = true;
                }
            } 
        }

        public bool IsLogInContentView { get; private set; } 

        public bool IsLogOutContentView { get; private set; } = true;

        public bool IsPartyContentView { get; private set; }

        public NavigationViewModel()
        {
            _storageService = FreshIOC.Container.Resolve<IStorageService>();
            _storageService.IsLoginChanged += LoginChanged;
        }

        private void LoginChanged(object sender, bool b)
        {
            IsLogIn = b;
        }

        public async Task OpenMore(Party party)
        {
            await CoreMethods.PushPageModel<PartyDetailViewModel>(party);
        }
        public async Task OpenLogin()
        {
            //Check if user is loged in -> Token is available 
            //If Token is available -> Refresh token
            //If not -> Open SignUp! 
            //
        }
        public Command<License> OpenLicenseCommand => new Command<License>(async (license) => await CoreMethods.PushPageModel<EditProfileViewModel>(license));
        public Command MoveToUserEditCommand => new Command(async () => await CoreMethods.PushPageModel<EditProfileViewModel>());

        public Command LogOutCommand => new Command(async () => await _storageService.DeleteStorage());

        public Command LogInCommand => new Command(async () => await CoreMethods.PushPageModel<LoginViewModel>());

        private async Task Sync()
        {
            using (UserDialogs.Instance.Loading("Sync"))
            {
                var result = await FreshIOC.Container.Resolve<IDataService>().RequestPartyWithFilter(); 
            }
        } 
    }
}