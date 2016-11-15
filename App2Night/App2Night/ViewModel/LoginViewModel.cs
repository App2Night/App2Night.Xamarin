using System;
using MvvmNano;

namespace App2Night.ViewModel
{
    public class LoginViewModel : MvvmNanoViewModel
    {
        MvvmNanoCommand MoveToDashboardCommand => new MvvmNanoCommand(() => NavigateTo<DashboardViewModel>());
        public MvvmNanoCommand StartLoginCommand => new MvvmNanoCommand(StartLogin);
        public string Password { get; set; }
        public string Username { get; set; }

        private void StartLogin()
        {
            if (Password != null && Username != null)
            {
                //TODO remove event when mvvmnano is updated
                //NavigateTo<DashboardViewModel>();
                CloseViewEvent?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                //TODO information box appears
            }
        }

        public event EventHandler CloseViewEvent;

    }
}