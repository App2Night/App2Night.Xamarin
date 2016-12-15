using System;
using System.Collections.ObjectModel;
using System.Linq;
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

		[AlsoNotifyFor(nameof(UserName))]
		public Model.Model.User User { get; set;}
		public string UserName => User != null ? User.Name : string.Empty;

        private bool _isLogIn;
        public bool IsLogIn
		{
			get { return _isLogIn; }
			private set {_isLogIn = value;}
		}

		private bool _isLogOut = true;
		public bool IsLogOut
		{
			get { return _isLogOut; }
			private set {_isLogOut = value;}
		}

		bool _isNextParty;
		public bool IsNextParty
		{
			get { return _isNextParty;}
			private set {_isNextParty = value;}
		}

		Party _party;
		[AlsoNotifyFor(nameof(PartyName))]
		public Party NextParty 
		{ 
			get {return _party;} 
			set {_party = value;}
		}

		public string PartyName => NextParty != null ? NextParty.Name : string.Empty;

		private Party GetNextParty() 
		{
			var nextParty = _dataService.SelectedPartys.OrderBy(o => o.Date).FirstOrDefault(o => o.Date.Date >= DateTime.Today);
			return nextParty;
		}

        public NavigationViewModel()
        {
            _storageService = FreshIOC.Container.Resolve<IStorageService>();
            _dataService = FreshIOC.Container.Resolve<IDataService>();
			_dataService.GetUser();
			User = _dataService.User;
            _storageService.IsLoginChanged += LoginChanged;
			_dataService.UserUpdated += UserUpdated;
			_dataService.SelectedPartiesUpdated += SelectedPartiesChanged;
        }

		private void LoginChanged(object sender, bool b)
		{
			IsLogIn = b;
			IsNextParty = b;
			IsLogOut = !b;
		}
		private void UserUpdated(object sender, Model.Model.User e)
		{
			User = e;
			RaisePropertyChanged(nameof(UserName));
		}

		public async Task OpenLogin()
		{
			//Check if user is loged in -> Token is available 
			//If Token is available -> Refresh token
			//If not -> Open SignUp! 
			//
		}
		private void SelectedPartiesChanged(object sender, EventArgs b)
		{
			NextParty = GetNextParty();
			if (IsLogIn && (NextParty != null)) IsNextParty = true;
		}

        public Command<License> OpenLicenseCommand => new Command<License>(async (license) => await CoreMethods.PushPageModel<EditProfileViewModel>(license));
		public Command MoveToUserEditCommand => new Command(async () => await CoreMethods.PushPageModel<EditProfileViewModel>(_dataService, modal:true));
		public Command MoveToPartyDetailPage => new Command(async () => await CoreMethods.PushPageModel<PartyDetailViewModel>(NextParty, modal:true));

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