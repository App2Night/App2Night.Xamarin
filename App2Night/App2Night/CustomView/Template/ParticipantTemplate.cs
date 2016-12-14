using App2Night.Model.Enum;
using App2Night.Model.Model;
using App2Night.Service.Helper;
using Xamarin.Forms;

namespace App2Night.CustomView.Template
{
    public class ParticipantTemplate : Frame
    {
        #region views

        private Label _userIconLabel = new Label
        {
            Text = "\uf007",
            FontFamily = "FontAwesome",
            FontSize = 18
        };

        private Label _nameLabel = new Label
        {
            Margin = 5
        };

        #endregion

        public ParticipantTemplate()
        {
            Padding = 5;
            Margin = 5;
            this.HasShadow = Device.OS == TargetPlatform.Android;
           
            _nameLabel.SetBinding(Label.TextProperty, nameof(Participant.UserName));

            Content = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }
                },
                Children =
                {
                    _userIconLabel,
                    {_nameLabel, 0, 1 }
                }
            };
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext != null)
            {
                var participant = (Participant)BindingContext;
                var acceptColor = System.Drawing.Color.PaleGreen.ToXamarinColor().MultiplyAlpha(0.7);
                var noteColor = System.Drawing.Color.Orange.ToXamarinColor().MultiplyAlpha(0.7);
                var color = participant.UserCommitmentState == PartyCommitmentState.Accepted
                        ? acceptColor
                        : noteColor;
                if (Device.OS == TargetPlatform.Android)
                {
                    BackgroundColor = color.MultiplyAlpha(0.3);
                }
                else
                    OutlineColor = color;
            }
        }
    }
}