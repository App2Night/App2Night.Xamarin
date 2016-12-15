using System;
using System.Threading.Tasks;
using App2Night.CustomView.View;
using App2Night.Model.Enum;
using App2Night.Model.Model;
using App2Night.PageModel;
using App2Night.Service.Helper;
using App2Night.ValueConverter;
using FreshMvvm;
using Xamarin.Forms;

namespace App2Night.CustomView.Template
{
    public class QuadraticPartyTemplate : Frame
    {
        #region Views
        Label _distanceLabel = new Label
        {
            TextColor = Color.White
        };

        Label _titleLabel = new Label()
        {
            FontSize = 20,
            HorizontalTextAlignment = TextAlignment.Start
        }; 

        CustomButton _shareIconLabel = new CustomButton
        {
            FontFamily = "FontAwesome",
            Text = "\uf1e0",
            HorizontalOptions = LayoutOptions.Center, 
            ButtonLabel = { FontSize = 35},
            Padding = 10
        };

        private Image _image = new Image
        {
            Aspect = Aspect.AspectFill,
            IsOpaque = false
        };

        CommitmentStateView _likeButton = new CommitmentStateView
        {
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Start,
            Padding = 10,
            FontSize = 50,
        };
        #endregion 

        public QuadraticPartyTemplate()
        {
            BackgroundColor = Color.White;
            Padding = 8;
            HasShadow = true;
            SetBindings();

            _shareIconLabel.ButtonTapped += ShareIconLabelOnButtonTapped;
            _likeButton.ButtonTapped += TappedLikeBtn;
            Content = CreateInputColumns(); 
        }

        private void ShareIconLabelOnButtonTapped(object sender, EventArgs eventArgs)
        {
            Task.Run(async ()=> await FreshIOC.Container.Resolve<DashboardPageModel>().ShareParty((Party) BindingContext));
        }

        private void SetBindings()
        {
            _likeButton.SetBinding(CommitmentStateView.CommitmentStatePendingProperty, "CommitmentStatePending");
			if (Device.OS != TargetPlatform.Android)
            	_image.SetBinding(Image.SourceProperty, nameof(Party.ImageSource));
            _titleLabel.SetBinding(Label.TextProperty, "Name");
            _distanceLabel.SetBinding(IsVisibleProperty, "Date", converter: new DateInFutureConverter());
            _shareIconLabel.SetBinding(IsVisibleProperty, "Date", converter: new DateInFutureConverter());
            _likeButton.SetBinding(CustomButton.IsEnabledProperty, "Date", converter: new DateInFutureConverter());
            _likeButton.SetBinding(CommitmentStateView.CommitmentStateProperty, nameof(Party.CommitmentState));
            _likeButton.SetBinding(CommitmentStateView.HostedByUserProperty, nameof(Party.HostedByUser));
        }

        private Grid CreateInputColumns()
        {
           return new Grid
            {
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = new GridLength(5, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)}
                },
                Children =
                {
                    _image,
                    _distanceLabel,
                    _likeButton,
                    {new Grid
                    {
                        ColumnDefinitions = new ColumnDefinitionCollection
                        {
                           new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star)},
                           new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)},
                        },
                        Children =
                        {
                            {_titleLabel,0,0},
                            {_shareIconLabel,1,0}
                        }
                    },0,1}
                }
            };
        } 

        /// <summary>
        /// Sets <see cref="_likeButton"/> to CommimentState. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TappedLikeBtn(object sender, EventArgs e)
        { 
            FreshIOC.Container.Resolve<DashboardPageModel>().PartyCommitmentStateChangedCommand.Execute((Party)BindingContext);
        } 

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (BindingContext != null)
            {
                var party = (Party) BindingContext;
                ShowDistanceToParty(party);
                party.PropertyChanged += (sender, args) =>
                {
                    ShowDistanceToParty(party);
                };
            }
        }

        private void ShowDistanceToParty(Party party)
        {
            if (party.DistanceToParty == -1)
            {
                //This is the default falue, distance not measured.
                _distanceLabel.Text =
                    $"{party.Location.CityName}\n{party.Location.StreetName} {party.Location.HouseNumber}";
            }
            else
            {
                //Show the distance:
                var distance = party.DistanceToParty;
                var unit = string.Empty;
                if (distance > 1) //Check if distance is above one km
                {
                    distance = Math.Round(distance, 3);
                    unit = "km";
                }
                else
                {
                    distance = Math.Round(distance * 100);
                    unit = "m";
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    _distanceLabel.Text =
                   $"{distance} {unit}";
                });
            }
        }
    }
}