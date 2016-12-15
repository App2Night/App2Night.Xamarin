using System;
using App2Night.CustomView.Template;
using App2Night.Model.Enum;
using App2Night.Service.Helper;
using Xamarin.Forms;

namespace App2Night.CustomView.View
{
    public class CommitmentStateView : CustomButton
    {

        public static BindableProperty CommitmentStatePendingProperty = BindableProperty.Create(nameof(CommitmentStatePending),
            typeof(bool),
            typeof(CommitmentStateView),
            false,
            propertyChanged: (bindable, value, newValue) =>
            ((CommitmentStateView)bindable).CommitmentStatePendingChanged((bool)newValue));

        public bool CommitmentStatePending
        {
            get { return (bool)GetValue(CommitmentStatePendingProperty); }
            set { SetValue(CommitmentStatePendingProperty, value); }
        } 

        public static BindableProperty HostedByUserProperty = BindableProperty.Create(nameof(HostedByUser), typeof(object), typeof(CommitmentStateView), false,
            propertyChanged: (bindable, value, newValue) => ((CommitmentStateView)bindable).HostedByUserSet((bool)newValue)); 

        public bool HostedByUser
        {
            get { return (bool)GetValue(HostedByUserProperty); }
            set { SetValue(HostedByUserProperty, value); }
        } 

        public static BindableProperty CommitmentStateProperty = BindableProperty.Create(nameof(CommitmentState), 
            typeof(PartyCommitmentState), 
            typeof(CommitmentStateView), 
            PartyCommitmentState.Rejected, 
            propertyChanged: (bindable, value, newValue) =>
            {
                ((CommitmentStateView)bindable).CommitmentStateChanged((PartyCommitmentState)newValue);
            });

        public PartyCommitmentState CommitmentState
        {
            get { return (PartyCommitmentState)GetValue(CommitmentStateProperty); }
            set { SetValue(CommitmentStateProperty, value); }
        }

        public CommitmentStateView()
        {
            Text = "\uf006";
            FontFamily = "FontAwesome";
            RejectParty();
        } 

        private void HostedByUserSet(bool hostedByUser)
        {
            IsEnabled = !hostedByUser;
            Text = hostedByUser ? "\uf015" : "\uf006";

            if(HostedByUser)
                ButtonLabel.TextColor = Color.Black;
            else
                CommitmentStateChanged(CommitmentState);
        }

        private void CommitmentStatePendingChanged(bool pending)
        {
            var animationName = "PendingAnimation";
            if (pending)
            {
                var animation = new Animation(d =>
                {
                    this.RotationY = 180 * d;
                });

                animation.Commit(this, animationName, length: 750U, repeat: () => true, finished: (d, b) =>
                {
                    RotationY = 0;
                }); 
            }
            else
            {
                this.AbortAnimation(animationName);
            }
        }

        private void CommitmentStateChanged(PartyCommitmentState partyCommitmentState)
        {
            if (partyCommitmentState == PartyCommitmentState.Rejected)
            {
                RejectParty();
            }
            else if (partyCommitmentState == PartyCommitmentState.Accepted)
            {
                AcceptParty();
            }
            else if (partyCommitmentState == PartyCommitmentState.Noted)
            {
                NoteParty();
            }
        }

        private void RejectParty()
        {
            // sets btn back to star with a white color
            Text = "\uf006";
            ButtonLabel.TextColor = System.Drawing.Color.LightGray.ToXamarinColor(); 
        }

        private void AcceptParty()
        {
            // sets btn to heart with a red color
            Text = "\uf004";
            ButtonLabel.TextColor = Color.Red; 
        }

        private void NoteParty()
        {
            // sets btn to star, change color to yellow
            Text = "\uf005";
            ButtonLabel.TextColor = Color.Yellow; 
        }
    }
}