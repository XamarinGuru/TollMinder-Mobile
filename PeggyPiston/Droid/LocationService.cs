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

			} else {
				var errorString = String.Format("There is a problem with Google Play Services on this device: {0} - {1}", queryResult, GooglePlayServicesUtil.GetErrorString(queryResult));
				PeggyUtils.DebugLog(errorString, logChannel);
				throw new Exception(errorString);
			}
		}

		public void connectGoogleAPI()
		{
			System.Diagnostics.Debug.Assert(_googleApiLocService != null);

			if (!_googleApiLocService.IsConnectionCallbacksRegistered(this)) _googleApiLocService.RegisterConnectionCallbacks(this);
			if (!_googleApiLocService.IsConnectionFailedListenerRegistered(this)) _googleApiLocService.RegisterConnectionFailedListener(this);
			if (!_googleApiLocService.IsConnected || !_googleApiLocService.IsConnecting) _googleApiLocService.Connect();
		}
		public void disconnectGoogleAPI()
		{
			if (_googleApiLocService != null && _googleApiLocService.IsConnected) {
				if (_googleApiLocService.IsConnectionCallbacksRegistered(this)) _googleApiLocService.UnregisterConnectionCallbacks(this);
				if (_googleApiLocService.IsConnectionFailedListenerRegistered(this)) _googleApiLocService.UnregisterConnectionFailedListener(this);
				_googleApiLocService.Disconnect();
			}

		}


		public void OnConnected(Bundle connectionHint)
		{
			PeggyUtils.DebugLog("service connected", logChannel);

			if (LocRequest == null)
			{
				throw new Exception("Unknown location request. Set this first by using property LocRequest or constructor.");
			}

			// location api.
			LocationServices.FusedLocationApi.RequestLocationUpdates(_googleApiLocService, LocRequest, this);

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
