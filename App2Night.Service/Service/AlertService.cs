using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using App2Night.Model.Model;
using App2Night.Service.Helper;
using App2Night.Service.Interface;
using Xamarin.Forms;

namespace App2Night.Service.Service
{
    public class AlertService : IAlertService
    {  
        public void UserCreationFinished(Result requestResult, string username)
        {
            if (requestResult.Success)
            {
                //Handle success state.
                Device.BeginInvokeOnMainThread(async ()=> await UserDialogs.Instance.AlertAsync(new AlertConfig
                {
                    Title = "User created!",
                    Message = $"Welcome to App2Night {username}! We send you an e-mail with a confirmation link. You can create parties after confirming your account.",
                    OkText = "I understand."
                })); 
            }
            else
                DefaultFailureHandler(requestResult, "User creation failed");
        }

        public void PartyPullFinished(Result<IEnumerable<Party>>  requestResult)
        {
            if (requestResult.Success)
            {
                //Handle success state.
                var message = string.Format("We found {0} parties near you.", requestResult.Data.Count()); 
                using (UserDialogs.Instance.Toast(createToastConfig(message, ToastState.Success))) { }
            }
            else
                DefaultFailureHandler(requestResult, "Searching for parties failed.");
        }

        public void LoginFinished(Result requestResult)
        {
            if (requestResult.Success)
            {
                //Handle success state.
                var message = "User logged in.";
                UserDialogs.Instance.Toast(createToastConfig(message, ToastState.Success));
            }
            else
                DefaultFailureHandler(requestResult, "Login failed");
        }

        void DefaultFailureHandler(Result result, string title)
        {
            var message = "The last request got lost and we can't find it anymore :( " +
                          "Dont feel betrayed, we will handle your future requests even more carefull and give him more time! " +
                          "To help us handling your request, please make sure you are actually connected to the internet.";
            Device.BeginInvokeOnMainThread(async () =>
            {
                await UserDialogs.Instance.AlertAsync(new AlertConfig
                {
                    Title = title,
                    Message = message
                });
            });
        }

        ToastConfig createToastConfig(string message, ToastState state = ToastState.Neutral)
        {
            var config = new ToastConfig(message);
            switch (state)
            {
                case ToastState.Success:
                    config.BackgroundColor = ((Color)Application.Current.Resources["SuccessColor"]).ToSystemDrawingColor();
                    config.MessageTextColor = ((Color)Application.Current.Resources["SuccessTextColor"]).ToSystemDrawingColor(); 
                    break;
                case ToastState.Failure:
                    config.BackgroundColor = ((Color)Application.Current.Resources["FailureColor"]).ToSystemDrawingColor();
                    config.MessageTextColor = ((Color)Application.Current.Resources["FailureTextColor"]).ToSystemDrawingColor(); 
                    break;
                case ToastState.Neutral:
                    config.BackgroundColor = ((Color)Application.Current.Resources["NeutralColor"]).ToSystemDrawingColor();
                    config.MessageTextColor = ((Color)Application.Current.Resources["NeutralTextColor"]).ToSystemDrawingColor();  
                    break; 
            }
            return config;
        }
    }

    public enum ToastState
    {
        Success,
        Failure,
        Neutral
    }
}