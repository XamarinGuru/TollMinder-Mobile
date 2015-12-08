using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.Gms.Location;
using Android.Support.V7.App;
using Android.Graphics;
using System.Collections.Generic;
using Tollminder.Droid.Views;
using Cirrious.CrossCore;
using Tollminder.Core.Services;

namespace Tollminder.Droid.Services
{
	[Service(Name = "tollminder.droid.services.GeofenceTransitionsIntentService",
		Label = "GeofenceTransitionsIntentService",
		Exported = false )]
	public class GeofenceTransitionsIntentService : IntentService
	{
		public GeofenceTransitionsIntentService () : base("GeofenceTransitionsIntentService")
		{
				
		}

		public override void OnCreate ()
		{
			base.OnCreate ();

		}

		public static readonly int GeofenceTransitionsIntentServiceCode = 101;
		#region implemented abstract members of IntentService
		protected override void OnHandleIntent (Intent intent)
		{
			var geofencingEvent = GeofencingEvent.FromIntent (intent);
			if (geofencingEvent.HasError) {
				return;
			}
			int geofenceTransition = geofencingEvent.GeofenceTransition;

			if (geofenceTransition == Geofence.GeofenceTransitionExit) {
				
				string geofenceTransitionDetails = GetGeofenceTransitionDetails (this, geofenceTransition, geofencingEvent.TriggeringGeofences);

				SendNotification (geofenceTransitionDetails);

//				Mvx.Resolve<IGeoLocationWatcher> ().Location = geofencingEvent.TriggeringLocation.GetGeolocationFromAndroidLocation ();
			}

		}

		string GetGeofenceTransitionDetails (Context context, int geofenceTransition, IList<IGeofence> triggeringGeofences)
		{
			string geofenceTransitionString = GetTransitionString (geofenceTransition);

			var triggeringGeofencesIdsList = new List<string> ();
			foreach (IGeofence geofence in triggeringGeofences) {
				triggeringGeofencesIdsList.Add (geofence.RequestId);
			}
			var triggeringGeofencesIdsString = string.Join (", ", triggeringGeofencesIdsList);

			return string.Format ("{0}: {1}",geofenceTransitionString, triggeringGeofencesIdsString);
		}

		void SendNotification (string notificationDetails)
		{
			var notificationIntent = new Intent (ApplicationContext, typeof(HomeView));

			var stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create (this);
			stackBuilder.AddParentStack (Java.Lang.Class.FromType (typeof(HomeView)));
			stackBuilder.AddNextIntent (notificationIntent);

			var notificationPendingIntent =	stackBuilder.GetPendingIntent (0, (int)PendingIntentFlags.UpdateCurrent);

			var builder = new NotificationCompat.Builder (this);
			builder.SetSmallIcon (Resource.Mipmap.Icon)
				.SetLargeIcon (BitmapFactory.DecodeResource (Resources, Resource.Mipmap.Icon))
				.SetColor (Color.Red)
				.SetContentTitle (notificationDetails)
				.SetContentText ("Notification")
				.SetContentIntent (notificationPendingIntent);

			builder.SetAutoCancel (true);

			var mNotificationManager = (NotificationManager)GetSystemService (Context.NotificationService);
			mNotificationManager.Notify (0, builder.Build ());
		}

		string GetTransitionString (int transitionType)
		{
			switch (transitionType) {
			case Geofence.GeofenceTransitionEnter:
				return "ENTER";
			case Geofence.GeofenceTransitionExit:
				return "EXIT";
			default:
				return "What do you mean ?";
			}
		}
		#endregion
		
	}
}

