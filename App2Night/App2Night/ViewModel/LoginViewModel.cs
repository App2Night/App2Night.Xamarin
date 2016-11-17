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
        public MvvmNanoCommand ContinueAnonymCommand => new MvvmNanoCommand(ContinueAnonym);

        private void ContinueAnonym()
        {
            NavigateTo<DashboardViewModel>();
        }

        private void StartLogin()
        {
            if (Password != null && Username != null)
            { 
                //TODO Login
                NavigateTo<DashboardViewModel>(); 
            }
            else
            {
                //TODO information box appears
            }
        } 
    }
}