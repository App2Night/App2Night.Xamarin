using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using App2Night.Model.HttpModel;
using App2Night.Model.Model;
using App2Night.Service.Interface;
using FreshMvvm;
using PropertyChanged;
using Xamarin.Forms;

namespace App2Night.PageModel
{
    [ImplementPropertyChanged]
    public class LoginViewModel : FreshBasePageModel
    {
        private readonly IAlertService _alertService;
        private readonly IDataService _dataService;
        public Command MoveToDashboardCommand => new Command(async () => await CoreMethods.PushPageModel<DashboardPageModel>());
        public Command StartLoginCommand => new Command(async () => await FormSubmitted());

        public LoginViewModel(IAlertService alertService, IDataService dataService)
        {
            _alertService = alertService;
            _dataService = dataService;
        }

        [AlsoNotifyFor(nameof(ValidPassword), nameof(CanSubmitForm))] 
        public string Password { get; set; }

        public bool ValidPassword => ValidatePassword();

        private bool ValidatePassword()
        {
            return !string.IsNullOrEmpty(Password) && Password.Length >= 3;
        }

        [AlsoNotifyFor(nameof(ValidUsername), nameof(CanSubmitForm))] 
        public string Username { get; set; }

        [AlsoNotifyFor(nameof(ValidEmail), nameof(CanSubmitForm))]

        public string Email { get; set; }

        bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;
            if (!email.Contains("@")) return false;
            return true;
        }

        private bool _validEmail;

        public bool ValidEmail => ValidateEmail(Email);

        [AlsoNotifyFor(nameof(SignUp))]
        public bool SignUp { get; set; }

        [AlsoNotifyFor(nameof(CanSubmitForm))]
        public bool AgbAccepted { get; set; }

        public string SubmitFormButtonText => SignUp ? "Sign up" : "SignUp";

        public bool ValidUsername => ValidateUsername();

        private bool ValidateUsername()
        {
            return !string.IsNullOrEmpty(Username) &&Username.Length >= 3;
        }

        public bool CanSubmitForm
        {
            get
            {
                if (SignUp)
                    //User opened the signup form.
                    return ValidEmail && ValidUsername && ValidPassword && AgbAccepted;
                else
                    return ValidPassword && ValidUsername;
            }
        }

        public Command ContinueAnonymCommand => new Command(async()=> await ContinueAnonym()); 

        private async Task ContinueAnonym()
        {
            await ClosePage();
        }

        private async Task FormSubmitted()
        {
            Result result = null;
			UserDialogs.Instance.ShowLoading();
                //Create user
                if (this.SignUp)
                {
					UserDialogs.Instance.Loading("Creating user");
                    var signUpData = new SignUp
                    {
                        Email = Email,
                        Password = Password,
                        Username = Username
                    };
                    result = await _dataService.CreateUser(signUpData); 
                    _alertService.UserCreationFinished(result, Username);
                }
                else { 
					UserDialogs.Instance.Loading("Login");
                     result = await _dataService.RequestToken(Username, Password);
                    _alertService.LoginFinished(result); 
                }
                if (result != null && result.Success)
                {
                    SyncData();
                    await ClosePage();
                } 
        }
        void SyncData()
        {
			Task.Run(async () => { 
				var result = await _dataService.BatchRefresh();
				await _alertService.PartyBatchRefreshFinished(result);
				UserDialogs.Instance.HideLoading();
			});
        }

        private async Task ClosePage()
        { 
                await CoreMethods.PopPageModel(true);
        }
    }
}
