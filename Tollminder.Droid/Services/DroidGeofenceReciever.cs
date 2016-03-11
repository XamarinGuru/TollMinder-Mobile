using System;
using Android.Content;
using Android.Gms.Location;
using Tollminder.Core.Helpers;

namespace Tollminder.Droid.Services
{
	[BroadcastReceiver(Enabled = true)]
	[IntentFilter(new[] { "com.tollminder.geofence.Recieve" })]
	public class DroidGeofenceReciever : BroadcastReceiver
	{
		#region implemented abstract members of BroadcastReceiver

		public override void OnReceive (Context context, Intent intent)
		{
			Log.LogMessage ("GEOFENCE TRIGGERED GEOFENCE TRIGGERED GEOFENCE TRIGGERED GEOFENCE TRIGGERED GEOFENCE TRIGGERED");
			var geofencingEvent = GeofencingEvent.FromIntent (intent);
			if (geofencingEvent.HasError) {
//				return StartCommandResult.Sticky;
			}
			int geofenceTransition = geofencingEvent.GeofenceTransition;

			if (geofenceTransition == Geofence.GeofenceTransitionExit) {				
				var data = geofencingEvent.TriggeringLocation.GetGeolocationFromAndroidLocation ();
			}

		}
		#endregion

		public override Android.OS.IBinder PeekService (Context myContext, Intent service)
		{
			return base.PeekService (myContext, service);
		}
	}
}

