using System;

using Android.App;

using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.Locations;
using Android.Util;
using Android.OS;
using Android.Content;


namespace PeggyPiston.Droid
{
	[Service]
	public class LocationService : Service, IGoogleApiClientConnectionCallbacks, IGoogleApiClientOnConnectionFailedListener, Android.Gms.Location.ILocationListener
	{

		private IGoogleApiClient _googleAPI;
		private readonly Context _context;
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


		public void StartLocationUpdates (Context context) 
		{        

			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			else
			{
				_context = context;
			}

			/*
			 * old Android.locations version
			 * 
			var locationCriteria = new Criteria();                    
			locationCriteria.Accuracy = Accuracy.NoRequirement;        
			locationCriteria.PowerRequirement = Power.NoRequirement;                    
			var locationProvider = LocMgr.GetBestProvider(locationCriteria, true);
			LocMgr.RequestLocationUpdates(locationProvider, 2000, 0, this);
			*/


			LocRequest = new LocationRequest();
			LocRequest.SetPriority(100);
			LocRequest.SetFastestInterval(500);
			LocRequest.SetInterval(1000);

			initializeGoogleAPI();

		}


		public void connectGoogleAPI()
		{
			System.Diagnostics.Debug.Assert(_googleAPI != null);

			if (!_googleAPI.IsConnectionCallbacksRegistered(this))
			{
				_googleAPI.RegisterConnectionCallbacks(this);
			}
			if (!_googleAPI.IsConnectionFailedListenerRegistered(this))
			{
				_googleAPI.RegisterConnectionFailedListener(this);
			}
			if (!_googleAPI.IsConnected || !_googleAPI.IsConnecting)
			{
				_googleAPI.Connect();
			}
		}

		public void disconnectGoogleAPI()
		{
			if (_googleAPI != null && _googleAPI.IsConnected)
			{
				if (_googleAPI.IsConnectionCallbacksRegistered(this))
				{
					_googleAPI.UnregisterConnectionCallbacks(this);
				}
				if (_googleAPI.IsConnectionFailedListenerRegistered(this))
				{
					_googleAPI.UnregisterConnectionFailedListener(this);
				}
				_googleAPI.Disconnect();
			}
		}


		public void OnConnected(Bundle connectionHint)
		{
			Log.Debug("LocationService", "logged connected", connectionHint);
			if (LocRequest == null)
			{
				throw new Exception("Unknown location request. Set this first by using property LocRequest or constructor.");
			}

			LocationServices.FusedLocationApi.RequestLocationUpdates(_googleAPI, LocRequest, this);
		}

		public void OnConnectionSuspended(int cause)
		{
			Log.Debug("LocationService", "logged OnConnectionSuspended", cause);

		}

		public void OnConnectionFailed(ConnectionResult result)
		{
			Log.Debug("LocationService", "logged OnConnectionFailed", result);

		}

		public event EventHandler<LocationChangedEventArgs> LocationChanged = delegate { };
		public void OnLocationChanged (Location location)
		{
			LocationChanged (this, new LocationChangedEventArgs (location));
			Log.Debug ("LocationService", String.Format ("Latitude is {0}", location.Latitude));
			Log.Debug ("LocationService", String.Format ("Longitude is {0}", location.Longitude));
			Log.Debug ("LocationService", String.Format ("Altitude is {0}", location.Altitude));
			Log.Debug ("LocationService", String.Format ("Speed is {0}", location.Speed));
			Log.Debug ("LocationService", String.Format ("Accuracy is {0}", location.Accuracy));
			Log.Debug ("LocationService", String.Format ("Bearing is {0}", location.Bearing));
		}

		public event EventHandler<LocationChangedEventArgs> ProviderEnabled = delegate { };
		public void OnProviderEnabled (Location location)
		{
			ProviderEnabled (this, new LocationChangedEventArgs (location));
			Log.Debug("LocationService", "logged OnProviderEnabled");
		}

		public event EventHandler<LocationChangedEventArgs> ProviderDisabled = delegate { };
		public void OnProviderDisabled (Location location)
		{
			ProviderDisabled (this, new LocationChangedEventArgs (location));
			Log.Debug("LocationService", "logged OnProviderDisabled");
		}

		public event EventHandler<LocationChangedEventArgs> StatusChanged = delegate { };
		public void OnStatusChanged (Location location)
		{
			StatusChanged (this, new LocationChangedEventArgs (location));
			Log.Debug("LocationService", "logged OnStatusChanged");
		}


		private void initializeGoogleAPI()
		{
			int queryResult = GooglePlayServicesUtil.IsGooglePlayServicesAvailable(_context);

			if (queryResult == ConnectionResult.Success)
			{
				_googleAPI = new GoogleApiClientBuilder(_context).AddApi(LocationServices.Api).AddConnectionCallbacks(this).AddOnConnectionFailedListener(this).Build();
			}
			else
			{
				var errorString = String.Format("There is a problem with Google Play Services on this device: {0} - {1}", queryResult, GooglePlayServicesUtil.GetErrorString(queryResult));
				Log.Error("LocationService", errorString);
				throw new Exception(errorString);
			}
		}


	}

}

/*
using System;
using Android.App;
using Android.OS;
using Android.Content;


using Android.Gms.Common;
using Android.Gms.Common.Apis
using Android.Gms.Location;

namespace PeggyPiston.Droid
{
	[Service]
	public class LocationService : Service, IGooglePlayServicesClientConnectionCallbacks, IGooglePlayServicesClientOnConnectionFailedListener
	{

		private LocationClient locClient = new LocationClient (this, this, this);


		IBinder binder;
		public override IBinder OnBind (Intent intent)
		{
			binder = new LocationServiceBinder (this);
			return binder;
		}

		public override StartCommandResult OnStartCommand (Intent intent, StartCommandFlags flags, int startId)
		{
			return StartCommandResult.Sticky;
		}

		public void StartLocationUpdates () 
		{        

			/*
			 * old Android.locations version
			 * 
			var locationCriteria = new Criteria();                    
			locationCriteria.Accuracy = Accuracy.NoRequirement;        
			locationCriteria.PowerRequirement = Power.NoRequirement;                    
			var locationProvider = LocMgr.GetBestProvider(locationCriteria, true);
			LocMgr.RequestLocationUpdates(locationProvider, 2000, 0, this);
			*/
/*

			mGoogleApiClient = new GoogleApiClient.Builder(this)
				.addApi(LocationServices.API)
				.addConnectionCallbacks(this)
				.addOnConnectionFailedListener(this)
				.build();


			Android.Gms.Location.LocationRequest locRequest = new Android.Gms.Location.LocationRequest ();

			locRequest.SetPriority(100);
			locRequest.SetFastestInterval(500);
			locRequest.SetInterval(1000);


			locMgr.RequestLocationUpdates(locRequest);


		}


		public void OnLocationChanged (Location location)
		{
			System.Diagnostics.Debug.WriteLine ("Latitude: " + location.Latitude.ToString());
			System.Diagnostics.Debug.WriteLine ("Longitude: " + location.Longitude.ToString());
		}


		public void OnConnected (Bundle p0)
		{
			throw new NotImplementedException ();
		}
		public void OnDisconnected ()
		{
			throw new NotImplementedException ();
		}

		public void OnConnectionFailed (ConnectionResult result)
		{
			throw new NotImplementedException ();
		}
	}
}

*/