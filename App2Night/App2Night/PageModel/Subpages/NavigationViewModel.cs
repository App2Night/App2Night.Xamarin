﻿using System.Threading.Tasks;
using Acr.UserDialogs;
using App2Night.Model.Model;
using App2Night.PageModel.Subpages;
using App2Night.Service.Interface;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.PageModel
{
    public class NavigationViewModel : FreshBasePageModel
    {

        public async Task OpenMore(Party party)
        {
            await CoreMethods.PushPageModel<PartyDetailViewModel>(party);
        }
        public Command SyncCommand => new Command(async ()=> await Sync());

        public async Task OpenLogin()
        {
            //Check if user is loged in -> Token is available 
            //If Token is available -> Refresh token
            //If not -> Open SignUp! 
            //
        }

        private async Task Sync()
        {
            using (UserDialogs.Instance.Loading("Sync"))
            {
                var result = await FreshIOC.Container.Resolve<IDataService>().RequestPartyWithFilter(); 
            }
        } 
    }
}