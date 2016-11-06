using System;
using App2Night.CustomView.Page;
using App2Night.CustomView.View;
using App2Night.ViewModel;
using Xamarin.Forms;

namespace App2Night.View
{
    public class CreatePartyPage : ContentPageWithInfo<CreatePartyViewModel>
    {
        private Entry _entryName = new Entry();
        private Entry _descriptionEntry = new Entry();
        private readonly DatePicker _datePicker = new DatePicker
        {
            MinimumDate = System.DateTime.Now,
            MaximumDate = System.DateTime.Now.AddMonths(12)
        };
        private Picker _musicGenreSwitch = new Picker();
        private CustomButton _cancelButton = new CustomButton
        {
            Text = "\uf00c",
            ButtonLabel ={FontFamily = "FontAwesome", FontSize = 50},
        };
        private CustomButton _acceptButton = new CustomButton
        {
            Text = "\uf00d",
            ButtonLabel = { FontFamily = "FontAwesome",FontSize = 50},
        };

        private ImageFromPortable _image = new ImageFromPortable(null)
        {
            HeightRequest = 100,
            WidthRequest = 100,
            ImagePath = "App2Night.Data.Image.default.png"
        };
        public CreatePartyPage()
        {
            _acceptButton.ButtonTapped += Accept;
            _cancelButton.ButtonTapped += Cancel;
            Title = "Create Party";
            Content = new Grid
            {
                Padding = new Thickness(10),
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                },
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                },
                Children =
                {
                    {
                        new Label
                    {
                        Text = "Name",
                        HorizontalOptions = LayoutOptions.Start
                    },0,1 },
                    {
                        new Label
                    {
                        Text = "Description",
                        HorizontalOptions = LayoutOptions.Start
                    },0,2 },
                    {
                        new Label
                    {
                        Text = "Date",
                        HorizontalOptions = LayoutOptions.Start
                    },0,3 },
                    {
                        new Label
                    {
                        Text = "Music Genre",
                        HorizontalOptions = LayoutOptions.Start
                    },0,4 },

                    {_image,0,0 },

                    {_entryName,1,1 },
                    {_descriptionEntry,1,2 },
                    {_datePicker,1,3 },
                    {_musicGenreSwitch,1,4 },

                    {_cancelButton,0,5 },
                    {_acceptButton,1,5 }
                }
            };
            Grid.SetColumnSpan(_image,2);
        }

        private void Accept(Object o, EventArgs e)
        {
            TappedAnimation(_acceptButton);
        }

        private void Cancel(Object o, EventArgs e)
        {
            TappedAnimation(_cancelButton);
        }

        private void TappedAnimation(Xamarin.Forms.View view)
        {
            var animation = new Animation(d =>
            {
                view.Scale = d;
            }, 1, 1.6);
            var nextAnimation = new Animation(d =>
            {
                view.Scale = d;
            }, 1.6, 1);
            animation.Commit(this, "Scale", finished: delegate
            {
                nextAnimation.Commit(this, "Descale");
            });
        }

        public override void Dispose()
        {
            base.Dispose();
            _acceptButton.ButtonTapped -= Accept;
            _cancelButton.ButtonTapped -= Cancel;

        }
    }
}