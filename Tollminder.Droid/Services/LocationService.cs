using System;
using Tollminder.Core.Services;
using Tollminder.Core.Models;
using Tollminder.Core.Helpers;
using Android.App;
using Android.Locations;
using Android.Content;
using Android.Widget;
using Android.OS;

namespace Tollminder.Droid
{
	public class LocationService : Service 
	{
		public static string BROADCAST_ACTION = "Hello World";
		public static int TWO_MINUTES = 1000 * 60 * 2;
		public LocationManager _locationManager;
		public GeoLocationListener _listener;
		public Location previousBestLocation = null;

		Intent _intent;
		int counter = 0;


		public override void OnCreate() 
		{
			base.OnCreate();
			_intent = new Intent(BROADCAST_ACTION);      
		}


		public override void OnStart(Intent intent, int startId) 
		{      
			_locationManager = (LocationManager) GetSystemService(Context.LocationService);
			_listener = new GeoLocationListener();        
			_locationManager.RequestLocationUpdates(LocationManager.NetworkProvider, 4000, 0, _listener);
			_locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 4000, 0, _listener);
		}

		public override Android.OS.IBinder OnBind (Intent intent)
		{
			return null;
		}

		protected bool IsBetterLocation(Location location, Location currentBestLocation) {
			if (currentBestLocation == null) {
				// A new location is always better than no location
				return true;
			}

			// Check whether the new location fix is newer or older
			long timeDelta = location.Time - currentBestLocation.Time;
			bool isSignificantlyNewer = timeDelta > TWO_MINUTES;
			bool isSignificantlyOlder = timeDelta < -TWO_MINUTES;
			bool isNewer = timeDelta > 0;

			// If it's been more than two minutes since the current location, use the new location
			// because the user has likely moved
			if (isSignificantlyNewer) {
				return true;
				// If the new location is more than two minutes older, it must be worse
			} else if (isSignificantlyOlder) {
				return false;
			}

			// Check whether the new location fix is more or less accurate
			int accuracyDelta = (int) (location.Accuracy - currentBestLocation.Accuracy);
			bool isLessAccurate = accuracyDelta > 0;
			bool isMoreAccurate = accuracyDelta < 0;
			bool isSignificantlyLessAccurate = accuracyDelta > 200;

			// Check if the old and new location are from the same provider
			bool isFromSameProvider = IsSameProvider (location.Provider,
				                          currentBestLocation.Provider);

			// Determine location quality using a combination of timeliness and accuracy
			if (isMoreAccurate) {
				return true;
			} else if (isNewer && !isLessAccurate) {
				return true;
			} else if (isNewer && !isSignificantlyLessAccurate && isFromSameProvider) {
				return true;
			}
			return false;
		}



		/** Checks whether two providers are the same */
		private bool IsSameProvider(String provider1, String provider2) {
			if (provider1 == null) {
				return provider2 == null;
			}
			return provider1.Equals(provider2);
		}

	
		public override void OnDestroy() {       
			// handler.removeCallbacks(sendUpdatesToUI);     
			base.OnDestroy();
			_locationManager.RemoveUpdates(_listener);        
		}  


		public class GeoLocationListener : Java.Lang.Object, Android.Locations.ILocationListener
		{
			public void OnLocationChanged(Location loc)
			{
//				Log.i("**************************************", "Location changed");
//				if(isBetterLocation(loc, previousBestLocation)) {
//					loc.getLatitude();
//					loc.getLongitude();             
//					intent.putExtra("Latitude", loc.getLatitude());
//					intent.putExtra("Longitude", loc.getLongitude());     
//					intent.putExtra("Provider", loc.getProvider());                 
//					SendBroadcast(intent);          
//
//				}                               
			}

			public void OnProviderDisabled(String provider)
			{
//				Toast.MakeText( GetAppl(), "Gps Disabled", Toast. ).show();
			}


			public void OnProviderEnabled(String provider)
			{
//				Toast.MakeText(, "Gps Enabled", Toast.LENGTH_SHORT).show();
			}


			public void OnStatusChanged(string provider, Availability status, Bundle extras)
			{

			}

		}
	}
}

