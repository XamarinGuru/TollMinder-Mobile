using System;
using Xamarin.Forms;

namespace PeggyPiston
{
	public class MainScreen : ContentPage
	{

		private IGeoLocation _locationProvider;
		private string _currentLocation;

		int count = 1;

		public MainScreen ()
		{

			_locationProvider = DependencyService.Get<IGeoLocation>();
			_currentLocation = "";

			MessagingCenter.Subscribe<IGeoLocation,string>(this, PeggyConstants.channelLocationService, HandleLocationUpdate);
			MessagingCenter.Subscribe<IGeoLocation,string>(this, PeggyConstants.channelDebug, HandleDebugVoice);
			MessagingCenter.Subscribe<IGeoLocation,string>(this, PeggyConstants.channelLocationUnavailable, HandleLocationUnavailable);


			var layout = new StackLayout
			{
				Orientation = StackOrientation.Vertical,
				Padding = 20
			};

			var grid = new Grid
			{
				RowSpacing = 50
			};

			grid.Children.Add(new Label { Text = "This" }, 0, 0); // Left, First element
			grid.Children.Add(new Label { Text = "text is" }, 1, 0); // Right, First element
			grid.Children.Add(new Label { Text = "in a" }, 0, 1); // Left, Second element
			grid.Children.Add(new Label { Text = "grid!" }, 1, 1); // Right, Second element

			var gridButton = new Button { Text = "So is this Button! Click me." };
			gridButton.Clicked += delegate
			{
				gridButton.Text = string.Format("Thanks! {0} clicks.", count++);
			};
			grid.Children.Add(gridButton, 0, 2); // Left, Third element

			layout.Children.Add(grid);
			Content = layout;

		}

		public void HandleLocationUpdate(IGeoLocation service, string newLocation)
		{
			if (_currentLocation != newLocation) {
				DependencyService.Get<ITextToSpeech> ().Speak ("your current address is " + newLocation);
				_currentLocation = newLocation;
			}
		}

		public void HandleDebugVoice(IGeoLocation service, string debugText)
		{
			DependencyService.Get<ITextToSpeech> ().Speak (debugText);
		}

		public void HandleLocationUnavailable(IGeoLocation service, string debugText)
		{
			PeggyUtils.DebugLog(debugText, PeggyConstants.channelLocationUnavailable);
		}

	}
}


