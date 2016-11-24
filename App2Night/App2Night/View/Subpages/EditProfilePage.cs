using System;
using App2Night.CustomView.View;
using App2Night.Data.Language;
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
		readonly InputContainer<Entry> _nameEntry = new InputContainer<Entry> { Input = { Placeholder = AppResources.Username }, IconCode ="\uf128", ValidationVisible = true };

		readonly InputContainer<Entry> _emailEntry = new InputContainer<Entry>{ Input = { Placeholder = AppResources.EmailAdress}, IconCode = "\uf003", ValidationVisible = true };

		readonly InputContainer<Entry> _addressEntry = new InputContainer<Entry>{ Input = { Placeholder = AppResources.Address }, IconCode = "\uf1ae", ValidationVisible = true };

		readonly InputContainer<Entry> _ageEntry = new InputContainer<Entry> { Input = { Placeholder = AppResources.Age, Keyboard = Keyboard.Numeric }, IconCode = "\uf1ae", ValidationVisible = true };

		readonly InputContainer<EnumBindablePicker<Gender>> _genderPicker = new InputContainer<EnumBindablePicker<Gender>>{ IconCode = "\uf183", ValidationVisible = true };

		// TODO handle IconCode
		Image _image = new Image { BackgroundColor = Color.Gray, HeightRequest = 300};

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
			Title = AppResources.EditProfile;
			// bind to view model
			BindToViewModel(_cancelBtn, CustomButton.CommandProperty, vm => vm.MoveToCancelCommand);
            BindToViewModel(_okBtn, CustomButton.CommandProperty, vm => vm.MoveTOkCommand);
			BindToViewModel(_nameEntry.Input, Entry.TextProperty, vm => vm.User.Name);
			BindToViewModel(_emailEntry.Input, Entry.TextProperty, vm => vm.User.Email);
			BindToViewModel(_ageEntry.Input, Entry.TextProperty, vm => vm.User.Age);
			BindToViewModel(_genderPicker, EnumBindablePicker<Gender>.SelectedItemProperty, vm => vm.User.Gender);

			// set event handler
			_okBtn.ButtonTapped += OnOkBtnTapped;
            // set Content with two grids. first one contains all information about the user. last one has a cancel and ok btn.
			StackLayout stackLayout = new StackLayout()
            {
                Children =
                {
					_image,
					_nameEntry,
					_emailEntry,
					_ageEntry,
					_addressEntry,
					_genderPicker,
                    new BoxView
                    {
                       HeightRequest = 1,
                       BackgroundColor = Color.Black
                    },
                    new Grid
                    {
                        ColumnDefinitions = new ColumnDefinitionCollection
                        {
                            new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                            new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
                        },
                        Children =
                        {
                            {_cancelBtn, 0, 0},
                            {_okBtn, 1, 0},
                        }
                    }
                }
            };
			var mainScroll = new ScrollView
			{
				Content = stackLayout,
				Orientation = ScrollOrientation.Vertical
			};
			Content = mainScroll;
        }

        private void OnOkBtnTapped(object sender, EventArgs e)
        {
			if (_okBtn.Command != null)
			{
				var animation = new Animation(d =>
				{
					_okBtn.Scale = d;
				}, 1, 1.6);
				var nextAnimation = new Animation(d =>
				{
					_okBtn.Scale = d;
				}, 1.6, 1);
				animation.Commit(this, "Scale", finished: delegate
				{
					nextAnimation.Commit(this, "Descale");
				});
				_okBtn.Command.Execute(null);
			}
        }

		private void OnCancelBtnTapped(object sender, EventArgs e)
		{
			if (_cancelBtn.Command != null)
			{
				var animation = new Animation(d =>
				{
					_cancelBtn.Scale = d;
				}, 1, 1.6);
				var nextAnimation = new Animation(d =>
				{
					_cancelBtn.Scale = d;
				}, 1.6, 1);
				animation.Commit(this, "Scale", finished: delegate
				{
					nextAnimation.Commit(this, "Descale");
				});
				_cancelBtn.Command.Execute(null);
			}
		}



        public override void Dispose()
        {
            base.Dispose();
			// remove event handler
            _okBtn.ButtonTapped -= OnOkBtnTapped;
        }
    }
}