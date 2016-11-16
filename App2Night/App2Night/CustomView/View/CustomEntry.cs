using System;
using Xamarin.Forms;

namespace App2Night
{
	/// <summary>
	/// Custom entry. Contains an image from FontAwesome and an entry.
	/// </summary>
	public class CustomEntry : Grid
	{
		// Only FontAwesome images 
		public string Image
		{ 
			get { return _ImageLabel.Text; } 
			set { _ImageLabel.Text = value; } 
		}

		public bool IsPassword 
		{ 
			get { return Entry.IsPassword; }
			set { Entry.IsPassword = value; }
		}

		public string Placeholder
		{
			get { return Entry.Placeholder; }
			set { Entry.Placeholder = value; }
		}

		public Keyboard Keyboard
		{
			get { return Entry.Keyboard; }
			set { Entry.Keyboard = value; }
		}

		public Entry Entry { get; } = new Entry { VerticalOptions = LayoutOptions.Fill};
		//public new double WidthRequest { get { return Entry.WidthRequest; } set { Entry.WidthRequest = value;} }

		Label _ImageLabel = new Label
		{
			FontFamily = "FontAwesome",
			FontSize = 25
		};

		public CustomEntry()
		{
			ColumnDefinitions = new ColumnDefinitionCollection()
			{ 
				new ColumnDefinition {Width = new GridLength(10, GridUnitType.Star)},
				new ColumnDefinition {Width = new GridLength(90, GridUnitType.Star)},
			};
			Children.Add(_ImageLabel, 0, 0);
			Children.Add(Entry, 1, 0);

		}
	}
}
