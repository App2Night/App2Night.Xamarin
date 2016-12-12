using System;
using System.Collections.ObjectModel;
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

        public string Name => User.Name;

        public bool IsLogIn
		{
			get { return _isLogIn; }
			private set
			{
				_isLogIn = value;
				if (value)
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

		private Party _party;
		public Party NextParty 
		{ 
			get {return _party;} 
			set {_party = value;}
		}

		private Party GetNextParty() 
		{
			ObservableCollection<Party> partyList = FreshIOC.Container.Resolve<IDataService>().SelectedPartys;
			Party nextParty = new Party();
			foreach (Party p in partyList)
			{
				if (p.Date > nextParty.Date) nextParty = p;
			}
			return nextParty;
		}

		public bool IsLogInContentView { get; private set; } 

        public bool IsLogOutContentView { get; private set; } = true;

        public bool IsPartyContentView { get; private set; }

        public NavigationViewModel()
        {
            _storageService = FreshIOC.Container.Resolve<IStorageService>();
            _dataService = FreshIOC.Container.Resolve<IDataService>();
            _dataService.GetUser();
            _storageService.IsLoginChanged += LoginChanged;
			_dataService.SelectedPartiesUpdated += SelectedPartiesChanged;
        }

        private void LoginChanged(object sender, bool b)
        {
            IsLogIn = b;
        }
		private void SelectedPartiesChanged(object sender, EventArgs b)
		{
			NextParty = GetNextParty();
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
		public Command MoveToPartyDetailPage => new Command(async () => await CoreMethods.PushPageModel<PartyDetailViewModel>(NextParty));

        public Command LogOutCommand => new Command(async () => await _storageService.DeleteStorage());

        public Command LogInCommand => new Command(async () => await CoreMethods.PushPageModel<LoginViewModel>(null, modal:true));

        private async Task Sync()
        {
            using (UserDialogs.Instance.Loading("Sync"))
            {
                var result = await FreshIOC.Container.Resolve<IDataService>().RequestPartyWithFilter(); 
            }
        } 
    }
}