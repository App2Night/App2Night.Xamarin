using System;
using App2Night.CustomView.View;
using App2Night.CustomViews;
using App2Night.Model.Enum;
using App2Night.Model.Model;
using App2Night.ViewModel.Subpages;
using MvvmNano;
using MvvmNano.Forms;
using Xamarin.Forms;

namespace App2Night.View.Subpages
{
    public class EditProfilePage : MvvmNanoContentPage<EditProfileViewModel>
    {
		#region Nodes
        public User User
        {
			get
			{
				return new User
				{
					Name = _nameEntry.Input.Text,
					Gender = _genderPicker.Input.SelectedItem,
					Email = _emailEntry.Input.Text,
				};  
			}
            set 
			{
				_nameEntry.Input.Text = value.Name;
				_emailEntry.Input.Text = value.Email;
				// set gender 
				if (value.Gender == Gender.Men) 
				{ 
					_genderPicker.Input.SelectedIndex = 0; 
				} else if (value.Gender == Gender.Woman) 
				{ 
					_genderPicker.Input.SelectedIndex = 1; 
				} else 
				{
					_genderPicker.Input.SelectedIndex = 2;
				}

			}
        }

		readonly InputContainer<Entry> _nameEntry = new InputContainer<Entry> { Input = { Placeholder = "Name" }, Image ="\uf128" };

        readonly InputContainer<Entry> _emailEntry = new InputContainer<Entry>{ Input = { Placeholder = "Email Address" }, Image = "\uf003" };

        readonly InputContainer<Entry> _addressEntry = new InputContainer<Entry>{ Input = { Placeholder = "Address" }, Image = "\uf1ae" };

		readonly InputContainer<Entry> _ageEntry = new InputContainer<Entry> { Input = { Placeholder = "Age", Keyboard = Keyboard.Numeric }, Image = "\uf1ae" };

		readonly InputContainer<EnumBindablePicker<Gender>> _genderPicker = new InputContainer<EnumBindablePicker<Gender>>{ Image = "\uf183" };

        private readonly CustomButton _cancelBtn = new CustomButton
        {
            Text = "\uf00d",
            ButtonLabel = { FontFamily = "FontAwesome", FontSize = 50 },
        };

        private readonly CustomButton _okBtn = new CustomButton
        {
            Text = "\uf00c",
            ButtonLabel = { FontFamily = "FontAwesome", FontSize = 50 },
        };
		#endregion
        /// <summary>
        /// Page where Name, Email, Address etc. can be changed.
        /// </summary>
        public EditProfilePage()
        {
            // set title and add Command for ViewModel
            Title = "Edit Profile";
			// bind to view model
			BindToViewModel(_cancelBtn, CustomButton.CommandProperty, vm => vm.MoveToCancelCommand);
            BindToViewModel(_okBtn, CustomButton.CommandProperty, vm => vm.MoveTOkCommand);
			// set event handler
            _okBtn.ButtonTapped += OnOkBtnTapped;
            // set Content with two grids. first one contains all information about the user. last one has a cancel and ok btn.

            Grid grid = new Grid()
            {
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
                },
                Children =
                {
                    {
                        new Grid
                        {
                            Padding = new Thickness(10),
                            RowDefinitions = new RowDefinitionCollection
                            {
                                new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                                new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                                new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                                new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                            },
                            Children =
                            {
                                {_nameEntry, 0, 0},
                                {_emailEntry, 0, 1},
                                {_addressEntry, 0, 2},
                                {_genderPicker, 0, 3}
                            }
                        },
                        0, 0
                    },
                    {
                        new BoxView
                        {
                            HeightRequest = 1,
                            BackgroundColor = Color.Black
                        },
                        0, 1
                    },
                    {
                        new Grid
                        {
                            ColumnDefinitions = new ColumnDefinitionCollection
                            {
                                new ColumnDefinition {Width = new GridLength(50, GridUnitType.Star)},
                                new ColumnDefinition {Width = new GridLength(50, GridUnitType.Star)}
                            },
                            Children =
                            {
                                {_cancelBtn, 0, 0},
                                {_okBtn, 1, 0},
                            }
                        },
                        0, 2
                    }
                }
            };
			var mainScroll = new ScrollView
			{
				Content = grid,
				Orientation = ScrollOrientation.Vertical
			};
			Content = mainScroll;
        }

        private void OnOkBtnTapped(object sender, EventArgs e)
        {
            // TODO handle tap
        }

        public override void Dispose()
        {
            base.Dispose();
			// remove event handler
            _okBtn.ButtonTapped -= OnOkBtnTapped;
        }
    }
}