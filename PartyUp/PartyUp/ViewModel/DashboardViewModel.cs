using System;
using System.Collections.ObjectModel;
using MvvmNano;
using PartyUp.Model.Model;
using PartyUp.Service.Interface;
using PartyUp.View.Subpages;
using PartyUp.ViewModel.Subpages;

namespace PartyUp.ViewModel
{
    public class DashboardViewModel : MvvmNanoViewModel
    {
        public ObservableCollection<Party> InterestingPartiesForUser => MvvmNanoIoC.Resolve<ICacheService>().Partys;

        public MvvmNanoCommand MoveToUserEditCommand => new MvvmNanoCommand(()=> NavigateTo<EditProfileViewModel>());
        public MvvmNanoCommand MoveToMyPartiesCommand => new MvvmNanoCommand(()=> NavigateTo<MyPartysViewModel>()); 
        public MvvmNanoCommand MoveToHistoryCommand => new MvvmNanoCommand(() => NavigateTo<HistoryViewModel>());
        public MvvmNanoCommand MoveToPartyPicker => new MvvmNanoCommand(() => NavigateTo<EventPickerViewModel>());
    }
}