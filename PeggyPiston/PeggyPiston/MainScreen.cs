using System;
using Xamarin.Forms;

namespace PeggyPiston
{
	public class MainScreen : ContentPage
	{

		protected readonly string logChannel = "MainScreen";

		public MainScreen ()
		{

			var layout = new StackLayout
			{
				Orientation = StackOrientation.Vertical,
				Padding = 20
			};

			var grid = new Grid
			{
				RowSpacing = 50
			};

			/*
			grid.Children.Add(new Label { Text = "This" }, 0, 0); // Left, First element
			grid.Children.Add(new Label { Text = "text is" }, 1, 0); // Right, First element
			grid.Children.Add(new Label { Text = "in a" }, 0, 1); // Left, Second element
			grid.Children.Add(new Label { Text = "grid!" }, 1, 1); // Right, Second element

			var gridButton = new Button { Text = "So is this Button! Click me." };
			int count = 1;
			gridButton.Clicked += delegate
			{
				gridButton.Text = string.Format("Thanks! {0} clicks.", count++);
			};
			grid.Children.Add(gridButton, 0, 2); // Left, Third element
			*/

			PeggyUtils.logViewer.HeightRequest = 600;
			grid.Children.Add (PeggyUtils.logViewer);

			layout.Children.Add(grid);
			Content = layout;

		}

	}
}


