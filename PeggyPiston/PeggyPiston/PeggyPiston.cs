using System;

using Xamarin.Forms;

namespace PeggyPiston
{
	public class App : Application
	{

		protected readonly string logChannel = "PeggyPiston";

		private IGeoLocation _locationProvider;
		private string _currentLocation;
		private double _currentAccuracy;
		private double _currentLattitude;
		private double _currentLongitude;

		public App () {
			_locationProvider = DependencyService.Get<IGeoLocation>();

			_currentAccuracy = 100000;
			_currentLocation = "";
			_currentLattitude = 0;
			_currentLongitude = 0;

			MessagingCenter.Subscribe<IGeoLocation,double>(this, PeggyConstants.channelLocationAccuracyReady, HandleLocationReady);
			MessagingCenter.Subscribe<IGeoLocation,string>(this, PeggyConstants.channelLocationService, HandleLocationUpdate);
			MessagingCenter.Subscribe<IGeoLocation,string>(this, PeggyConstants.channelLocationUnavailable, HandleLocationUnavailable);
			MessagingCenter.Subscribe<IGeoLocation,string>(this, PeggyConstants.channelDebug, HandleDebugVoice);

			MainPage = new MainScreen();
		}


		public void HandleLocationReady(IGeoLocation service, double accuracy) {
			_currentAccuracy = accuracy;

			if (_currentAccuracy <= PeggyConstants.highAccuracyRequirement) {
				PeggyUtils.DebugLog ("Location accuracy is " + _currentAccuracy, logChannel);

				// query the service and figure out where we are.
				bool hasChanged = false;
				Double lat = _locationProvider.GetCurrentLattitude();
				Double lon = _locationProvider.GetCurrentLongitude();

				// 5 decimal places in lat/long is equal to 1.1 meter.
				// so, a 20 meter requirement is really going to be 22 meters.  meh.  whatever.
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


		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
			PeggyUtils.DebugLog ("Peggy Piston has gone to sleep.", PeggyConstants.channelVoice);
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
			PeggyUtils.DebugLog ("Peggy Piston has woken up.", PeggyConstants.channelVoice);
		}
	}
}
