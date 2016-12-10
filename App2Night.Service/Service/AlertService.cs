using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using App2Night.Model.Enum;
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
                /*
                 * Inform the user about the successfull creation of the account 
                 * and that he will recieve a confirmation email.
                 */ 
                Device.BeginInvokeOnMainThread(async ()=> await UserDialogs.Instance.AlertAsync(new AlertConfig
                {
                    Title = "User created!",
                    Message = $"Welcome to App2Night {username}! We send you an e-mail with a confirmation link. You can create parties after confirming your account.",
                    OkText = "I understand."
                })); 
            }
            else 
                RequestFailureHandler(requestResult, "User creation failed");
        }

        public void PartyPullFinished(Result<IEnumerable<Party>>  requestResult)
        {
            if (requestResult.Success)
            { 
                /*
                 * Inform the user how many parties were found.
                 */
                var message = string.Format("We found {0} parties near you.", requestResult.Data.Count());
                Device.BeginInvokeOnMainThread(()=>UserDialogs.Instance.Toast(CreateToastConfig(message, ToastState.Success)));
            }
            else
                RequestFailureHandler(requestResult, "Searching for parties failed.");
        }

        public void LoginFinished(Result requestResult)
        {
            if (requestResult.Success)
            {
                
                /*
                 * Inform the user that he got logged in successfully.
                 */
                var message = "User logged in.";
                UserDialogs.Instance.Toast(CreateToastConfig(message, ToastState.Success));
            }
            else
                RequestFailureHandler(requestResult, "Login failed");
        }

        public void CommitmentStateChangedAlert(PartyCommitmentState noted, bool success)
        {
            var message = success
                ? string.Format("Commitment state changed to {0}", noted) //RESOURCE
                : "Changing the commitment state failed"; //RESOURCE

            var toastConfig = CreateToastConfig(message, success ? ToastState.Success : ToastState.Failure);

            UserDialogs.Instance.Toast(toastConfig);
        }

        public async Task<bool> PartyBatchRefreshFinished(IEnumerable<Result> requestBatch)
        {
            var failedRequests = requestBatch.Count(o => !o.Success);
            if (failedRequests == 0)
            {
                var toastConfig = CreateToastConfig("We successfully synchronised you parties.", ToastState.Success); //RESOURCE
                UserDialogs.Instance.Toast(toastConfig);
                return false;
            } 

            //Ask the user if he wants to retry loading the parties.
            var result = await UserDialogs.Instance.ConfirmAsync(
                "We were not able to load all party data, retry?", "Loading parties failed.", "Ok", "Cancel"); //RESOURCE
            return result; 
        }

        /// <summary>
        /// Handles request results that failed.
        /// Creates fitting messages for differnt request errors.
        /// </summary>
        /// <param name="result">Request result.</param>
        /// <param name="title">Title of the alert.</param>
        void RequestFailureHandler(Result result, string title)
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

        /// <summary>
        /// Creates a <see cref="ToastConfig"/> with a value for <see cref="ToastConfig.BackgroundColor"/> and <see cref="ToastConfig.MessageTextColor"/> set.
        /// The created <see cref="ToastConfig"/> contains the given message.
        /// </summary>
        /// <param name="message">Message to be shown by the toast.</param>
        /// <param name="state">The state (Success, Failure, Neutral) to be shown.</param>
        /// <returns></returns>
        ToastConfig CreateToastConfig(string message, ToastState state = ToastState.Neutral)
        {
            var config = new ToastConfig(message);
            /*
             * Set different colors depending on the ToastState.
             */
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

    /// <summary>
    /// States for the CreateToastConfig method./>
    /// </summary>
    public enum ToastState
    {
        Success,
        Failure,
        Neutral
    }
}