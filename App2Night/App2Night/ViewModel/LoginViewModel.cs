using MvvmNano;

namespace App2Night.ViewModel
{
    public class LoginViewModel : MvvmNanoViewModel
    {
        MvvmNanoCommand MoveToDashboardCommand => new MvvmNanoCommand(() => NavigateTo<DashboardViewModel>());
    }
}