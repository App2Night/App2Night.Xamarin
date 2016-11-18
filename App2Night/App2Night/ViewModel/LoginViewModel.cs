using System;
using System.Threading.Tasks;
using App2Night.Model.HttpModel;
using App2Night.Model.Model;
using App2Night.Service.Interface;
using MvvmNano;

namespace App2Night.ViewModel
{
    public class LoginViewModel : MvvmNanoViewModel
    {
        private string _password;
        private string _username;
        private bool _signUp;
        private string _email;
        private bool _agbAccepted;
        MvvmNanoCommand MoveToDashboardCommand => new MvvmNanoCommand(() => NavigateTo<DashboardViewModel>());
        public MvvmNanoCommand StartLoginCommand => new MvvmNanoCommand(async () => await FormSubmitted());

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyPropertyChanged(nameof(ValidPassword));
                NotifyPropertyChanged(nameof(CanSubmitForm));
            }
        }

        public bool ValidPassword => ValidatePassword();

        private bool ValidatePassword()
        {
            return !string.IsNullOrEmpty(Password) && Password.Length >= 3;
        }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                NotifyPropertyChanged(nameof(ValidUsername));
                NotifyPropertyChanged(nameof(CanSubmitForm));
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                NotifyPropertyChanged(nameof(ValidEmail));
                NotifyPropertyChanged(nameof(CanSubmitForm));
            }
        }

        bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;
            if (!email.Contains("@")) return false;
            return true;
        }

        private bool _validEmail;

        public bool ValidEmail => ValidateEmail(Email);

        public bool SignUp
        {
            get { return _signUp; }
            set
            {
                _signUp = value;
                NotifyPropertyChanged(nameof(SubmitFormButtonText));
                NotifyPropertyChanged(nameof(CanSubmitForm));
            }
        }



        public bool AgbAccepted
        {
            get { return _agbAccepted; }
            set
            {
                _agbAccepted = value;
                NotifyPropertyChanged(nameof(CanSubmitForm));
            }
        }

        public string SubmitFormButtonText => SignUp ? "Sign up" : "SignUp";

        public bool ValidUsername => ValidateUsername();

        private bool ValidateUsername()
        {
            return !string.IsNullOrEmpty(Username) &&_username.Length >= 3;
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

        public MvvmNanoCommand ContinueAnonymCommand => new MvvmNanoCommand(ContinueAnonym);

        private void ContinueAnonym()
        {
            NavigateTo<DashboardViewModel>();
        }

        private async Task FormSubmitted()
        {
            Result result = null;

            //Create user
            if (this.SignUp)
            {
                var signUpData = new SignUp
                {
                    Email = Email,
                    Password = Password,
                    Username = Username
                };
                result = await MvvmNanoIoC.Resolve<IDataService>().CreateUser(signUpData); 
                //TODO add result handling 
            }
            else 
            { 
                 result = await MvvmNanoIoC.Resolve<IDataService>().RequestToken(Username, Password);
                //TODO add result handling 
            }
            if (result.Success)
                NavigateTo<DashboardViewModel>();
        }
    }
}