using System;
using Xamarin.Forms;

namespace PeggyPiston
{
	public class MainScreen : ContentPage
	{

		protected readonly string logChannel = "MainScreen";
		private IGeoLocation _locationProvider;
		private string _currentLocation;
		private double _currentAccuracy;
		private double _currentLattitude;
		private double _currentLongitude;

		int count = 1;

		public MainScreen ()
		{

			_locationProvider = DependencyService.Get<IGeoLocation>();

			_currentAccuracy = 100000;
			_currentLocation = "";
			_currentLattitude = 0;
			_currentLongitude = 0;

			MessagingCenter.Subscribe<IGeoLocation,double>(this, PeggyConstants.channelLocationAccuracyReady, HandleLocationReady);
			MessagingCenter.Subscribe<IGeoLocation,string>(this, PeggyConstants.channelLocationService, HandleLocationUpdate);
			MessagingCenter.Subscribe<IGeoLocation,string>(this, PeggyConstants.channelLocationUnavailable, HandleLocationUnavailable);
			MessagingCenter.Subscribe<IGeoLocation,string>(this, PeggyConstants.channelDebug, HandleDebugVoice);


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

		public void HandleLocationReady(IGeoLocation service, double accuracy)
		{
			if (accuracy <= PeggyConstants.highAccuracyRequirement) {
				PeggyUtils.DebugLog ("Location accuracy is " + accuracy, logChannel);
				_currentAccuracy = accuracy;

				// query the service and figure out where we are.
				bool hasChanged = false;
				Double lat = _locationProvider.GetCurrentLattitude();
				Double lon = _locationProvider.GetCurrentLongitude();

				// 5 decimal places in lat/long is equal to 1.1 meter.
				// so, a 20 meter requirement is really going to be 22.  meh.  whatever.
				if (Math.Abs(Math.Floor (lat * 10000) - Math.Floor (_currentLattitude * 10000)) >= PeggyConstants.distanceRequirement) {
					_currentLattitude = lat;
					hasChanged = true;
				}
				if (Math.Abs(Math.Floor (lon * 10000) - Math.Floor (_currentLongitude * 10000)) >= PeggyConstants.distanceRequirement) {
					_currentLongitude = lon;
					hasChanged = true;
				}

				if (hasChanged) {
					// lookup our new address!
					var locHandle = LocationWebServiceClient.FetchCurrentAddress(_currentLattitude, _currentLongitude);
					PeggyUtils.DebugLog ("LocationWebServiceClient locHandle: " + locHandle, logChannel);
				}



			} else {
				PeggyUtils.DebugLog ("still determining location", PeggyConstants.channelVoice);
			}
		}

		public void HandleLocationUpdate(IGeoLocation service, string newLocation)
		{
			if (_currentLocation != newLocation) {
				PeggyUtils.DebugLog ("your current address is " + newLocation, PeggyConstants.channelVoice);
				_currentLocation = newLocation;
			}
		}

		public void HandleDebugVoice(IGeoLocation service, string debugText)
		{
			PeggyUtils.DebugLog (debugText, PeggyConstants.channelVoice);
		}
		public void HandleLocationUnavailable(IGeoLocation service, string debugText)
		{
			PeggyUtils.DebugLog(debugText, PeggyConstants.channelLocationUnavailable);
		}

	}
}


