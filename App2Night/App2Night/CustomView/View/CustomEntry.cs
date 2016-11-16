using System;
using Xamarin.Forms;

namespace App2Night
{
	public class CustomEntry : Grid
	{
		// Only FontAwesome images 
		public string ImageString { 
			get 
			{
				return _ImageLabel.Text;
			} 
			set 
			{
				_ImageLabel.Text = value;
			} 
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
				new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)},
				new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)},
			};
			Children.Add(_ImageLabel, 0, 0);
			Children.Add(Entry, 1, 0);

		}
	}
}
