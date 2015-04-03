using System;

using Android.App;

using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.Locations;
using Android.Util;
using Android.OS;
using Android.Content;
using Xamarin.Forms;


namespace PeggyPiston.Droid
{
	[Service]
	public class LocationService : Service, IGoogleApiClientConnectionCallbacks, IGoogleApiClientOnConnectionFailedListener, Android.Gms.Location.ILocationListener
	{
		protected readonly string logChannel = "LocationService";

		private IGoogleApiClient _googleApiLocService;
		private IGoogleApiClient _googleApiActivityService;
		public LocationRequest LocRequest
		{
			get;
			set;
		}

		IBinder binder;
		public override IBinder OnBind (Intent intent)
		{
			binder = new LocationServiceBinder (this);
			return binder;
		}


		public void StartLocationUpdates () 
		{        
			LocRequest = new LocationRequest();
			LocRequest.SetPriority(100);
			LocRequest.SetFastestInterval(500);
			LocRequest.SetInterval(1000);

			PeggyUtils.DebugLog("StartLocationUpdates successful", logChannel);

			initializeGoogleAPI();
			connectGoogleAPI();
		}


		private void initializeGoogleAPI()
		{
			int queryResult = GooglePlayServicesUtil.IsGooglePlayServicesAvailable(Forms.Context);

			if (queryResult == ConnectionResult.Success)
			{
				_googleApiLocService = new GoogleApiClientBuilder(Forms.Context)
					.AddApi(LocationServices.Api)
					.AddConnectionCallbacks(this)
					.AddOnConnectionFailedListener(this)
					.Build();

				PeggyUtils.DebugLog("google api client constructed", logChannel);


				// here's where we add the activity recognition.
				// https://developer.android.com/reference/com/google/android/gms/location/ActivityRecognitionApi.html
				// create common interface for DRIVING, WALKING, STILL	
				_googleApiActivityService = new GoogleApiClientBuilder(Forms.Context)
					.AddApi (ActivityRecognition.Api)
					.AddConnectionCallbacks(this)
					.AddOnConnectionFailedListener(this)
					.Build();

			} else {
				var errorString = String.Format("There is a problem with Google Play Services on this device: {0} - {1}", queryResult, GooglePlayServicesUtil.GetErrorString(queryResult));
				PeggyUtils.DebugLog(errorString, logChannel);
				throw new Exception(errorString);
			}
		}

		public void connectGoogleAPI()
		{
			System.Diagnostics.Debug.Assert(_googleApiLocService != null);
			System.Diagnostics.Debug.Assert(_googleApiActivityService != null);

			if (!_googleApiLocService.IsConnectionCallbacksRegistered(this)) _googleApiLocService.RegisterConnectionCallbacks(this);
			if (!_googleApiLocService.IsConnectionFailedListenerRegistered(this)) _googleApiLocService.RegisterConnectionFailedListener(this);
			if (!_googleApiLocService.IsConnected || !_googleApiLocService.IsConnecting) _googleApiLocService.Connect();

			if (!_googleApiActivityService.IsConnectionCallbacksRegistered(this)) _googleApiActivityService.RegisterConnectionCallbacks(this);
			if (!_googleApiActivityService.IsConnectionFailedListenerRegistered(this)) _googleApiActivityService.RegisterConnectionFailedListener(this);
			if (!_googleApiActivityService.IsConnected || !_googleApiActivityService.IsConnecting) _googleApiActivityService.Connect();
		}
		public void disconnectGoogleAPI()
		{
			if (_googleApiLocService != null && _googleApiLocService.IsConnected) {
				if (_googleApiLocService.IsConnectionCallbacksRegistered(this)) _googleApiLocService.UnregisterConnectionCallbacks(this);
				if (_googleApiLocService.IsConnectionFailedListenerRegistered(this)) _googleApiLocService.UnregisterConnectionFailedListener(this);
				_googleApiLocService.Disconnect();
			}

			if (_googleApiActivityService != null && _googleApiActivityService.IsConnected) {
				if (_googleApiActivityService.IsConnectionCallbacksRegistered(this)) _googleApiActivityService.UnregisterConnectionCallbacks(this);
				if (_googleApiActivityService.IsConnectionFailedListenerRegistered(this)) _googleApiActivityService.UnregisterConnectionFailedListener(this);
				_googleApiActivityService.Disconnect();
			}
		}


		public void OnConnected(Bundle connectionHint)
		{
			PeggyUtils.DebugLog("logged OnConnected", logChannel);

			if (LocRequest == null)
			{
				throw new Exception("Unknown location request. Set this first by using property LocRequest or constructor.");
			}

			// location api.
			LocationServices.FusedLocationApi.RequestLocationUpdates(_googleApiLocService, LocRequest, this);

			// activity recognition api
			var intent = new Intent(this, typeof(LocationService));
			var activityRecognitionPendingIntent = PendingIntent.GetService(this, 0, intent, PendingIntentFlags.UpdateCurrent);
			const int detectionInterval = 5000;
			ActivityRecognition.ActivityRecognitionApi.RequestActivityUpdates(_googleApiActivityService, detectionInterval, activityRecognitionPendingIntent);

		}

		protected override void OnHandleIntent(Intent intent) {
			if (ActivityRecognitionResult.HasResult(intent)) {
				var result = ActivityRecognitionResult.ExtractResult(intent);
				var mostProbableActivity = result.MostProbableActivity;
				var confidence = mostProbableActivity.Confidence;
				var activityType = mostProbableActivity.Type;
				var name = GetActivityName(activityType);
			}
		}

		protected string GetActivityName(int activityType) {
			//switch (activityType) {}
		}

		public void OnConnectionSuspended(int cause)
		{
			PeggyUtils.DebugLog("logged OnConnectionSuspended", logChannel);
		}

		public void OnConnectionFailed(ConnectionResult result)
		{
			PeggyUtils.DebugLog("logged OnConnectionFailed", logChannel);
		}

		public event EventHandler<LocationChangedEventArgs> LocationChanged = delegate { };
		public void OnLocationChanged (Location location)
		{
			LocationChanged (this, new LocationChangedEventArgs (location));
			PeggyUtils.DebugLog("logged OnLocationChanged", logChannel);
		}

		public event EventHandler<LocationChangedEventArgs> ProviderEnabled = delegate { };
		public void OnProviderEnabled (Location location)
		{
			ProviderEnabled (this, new LocationChangedEventArgs (location));
			PeggyUtils.DebugLog("logged OnProviderEnabled", logChannel);
		}

		public event EventHandler<LocationChangedEventArgs> ProviderDisabled = delegate { };
		public void OnProviderDisabled (Location location)
		{
			ProviderDisabled (this, new LocationChangedEventArgs (location));
			PeggyUtils.DebugLog("logged OnProviderDisabled", logChannel);
		}

		public event EventHandler<LocationChangedEventArgs> StatusChanged = delegate { };
		public void OnStatusChanged (Location location)
		{
			StatusChanged (this, new LocationChangedEventArgs (location));
			PeggyUtils.DebugLog("logged OnStatusChanged", logChannel);
		}


	}

}
