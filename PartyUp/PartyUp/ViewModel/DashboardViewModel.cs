using System;
using MvvmNano;
using PartyUp.View.Subpages;
using PartyUp.ViewModel.Subpages;

namespace PartyUp.ViewModel
{
    public class DashboardViewModel : MvvmNanoViewModel
    {
        public MvvmNanoCommand MoveToUserEditCommand => new MvvmNanoCommand(MoveToUserEdit);

        private void MoveToUserEdit()
        {
            NavigateTo<EditProfileViewModel>();
        }
    }
}