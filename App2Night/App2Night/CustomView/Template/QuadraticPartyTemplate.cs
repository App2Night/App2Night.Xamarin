using System;
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

        CustomButton _likeButton = new CustomButton
        {
            Text = "\uf006",
            FontFamily = "FontAwesome",
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Start,
            Margin = 10,
            FontSize = 50,
            ButtonLabel = {TextColor = Color.White}
        };

        CustomButton _shareIconLabel = new CustomButton
        {
            FontFamily = "FontAwesome",
            Text = "\uf1e0",
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(8, 0),
            ButtonLabel = { FontSize = 35}
        };
        #endregion

        readonly TapGestureRecognizer _tapGestureRecognizer = new TapGestureRecognizer();
        private PartyCommitmentState _commitmentState = PartyCommitmentState.Rejected;
        public QuadraticPartyTemplate()
        {
            BackgroundColor = Color.White;
            Padding = 8;
            HasShadow = true;
            SetBindings();

            _tapGestureRecognizer.Tapped += TappedLikeBtn;
            _likeButton.GestureRecognizers.Add(_tapGestureRecognizer);
            Content = CreateInputColumns();
        }

        private void SetBindings()
        {
            _titleLabel.SetBinding(Label.TextProperty, "Name");
            _distanceLabel.SetBinding(IsVisibleProperty, "Date", converter: new DateInFutureConverter());
            _shareIconLabel.SetBinding(IsVisibleProperty, "Date", converter: new DateInFutureConverter());
            _likeButton.SetBinding(IsVisibleProperty, "Date", converter: new DateInFutureConverter()); 
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
                    new BoxView {Color = System.Drawing.Color.Goldenrod.ToXamarinColor(), InputTransparent = true},
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
            if (_commitmentState == PartyCommitmentState.Rejected)
            {
                NoteParty();
            }
            else if (_commitmentState == PartyCommitmentState.Accepted)
            {
                RejectParty();
            }
            else if (_commitmentState == PartyCommitmentState.Noted)
            {
                AcceptParty();
            }

            var commitmentParameter = new PartyCommitmentParameter
            {
                Party = (Party) BindingContext,
                CommitmentState = _commitmentState
            };
            FreshIOC.Container.Resolve<DashboardPageModel>().PartyCommitmentStateChangedCommand.Execute(commitmentParameter);
        }

        private void RejectParty()
        {
            // sets btn back to star with a white color
            _likeButton.Text = "\uf006";
            _likeButton.ButtonLabel.TextColor = Color.White;
            _commitmentState = PartyCommitmentState.Rejected;
        }

        private void AcceptParty()
        {
            // sets btn to heart with a red color
            _likeButton.Text = "\uf004";
            _likeButton.ButtonLabel.TextColor = Color.Red;
            _commitmentState = PartyCommitmentState.Accepted;
        }

        private void NoteParty()
        {
            // sets btn to star, change color to 
            _likeButton.Text = "\uf005";
            _likeButton.ButtonLabel.TextColor = Color.Yellow;
            _commitmentState = PartyCommitmentState.Noted;
        } 

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (BindingContext != null)
            {
                var party = (Party) BindingContext;
                party.PropertyChanged += (sender, args) =>
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
                            distance = Math.Round(distance*100);
                            unit = "m";
                        }

                        _distanceLabel.Text =
                            $"{distance} {unit}";
                    }
                };
            }
        }
    }
}