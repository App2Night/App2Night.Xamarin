using System.Collections.ObjectModel;
using System.Diagnostics;
using App2Night.Service.Interface;
using App2Night.ViewModel.Subpages;
using MvvmNano;
using PartyUp.Model.Model;

namespace App2Night.ViewModel
{
    public class DashboardViewModel : MvvmNanoViewModel
    {
        public ObservableCollection<Party> InterestingPartiesForUser => MvvmNanoIoC.Resolve<IDataService>().Partys;
        public ObservableCollection<Party> PartyHistory => MvvmNanoIoC.Resolve<IDataService>().Partys;
        public ObservableCollection<Party> NextPartiesForUser => MvvmNanoIoC.Resolve<IDataService>().Partys;

        public MvvmNanoCommand MoveToUserEditCommand => new MvvmNanoCommand(() => NavigateTo<EditProfileViewModel>());
        public MvvmNanoCommand MoveToMyPartiesCommand => new MvvmNanoCommand(() => NavigateTo<MyPartysViewModel>());
        public MvvmNanoCommand MoveToHistoryCommand => new MvvmNanoCommand(() => NavigateTo<HistoryViewModel>());
        public MvvmNanoCommand MoveToPartyPicker => new MvvmNanoCommand(() => NavigateTo<PartyPickerViewModel>());

        public DashboardViewModel()
        {
            InterestingPartiesForUser.CollectionChanged += (sender, args) =>
            {
                Debug.WriteLine("Change!!!!");
            };
        }

        
    }
}