using System;
using Android.App;
using Android.Content;
using Android.Widget;

namespace Tollminder.Droid.Services
{
	[Service(Name = "tollminder.droid.services.ReceiveTransitionsIntentService",
		Label = "GeofenceTransitionsIntentService",
		Exported = false )]
	public class GeofenceTransitionsIntentService : IntentService
	{
		public GeofenceTransitionsIntentService () : base("GeofenceTransitionsIntentService")
		{
				
		}
		public static readonly int GeofenceTransitionsIntentServiceCode = 101;
		#region implemented abstract members of IntentService
		protected override void OnHandleIntent (Intent intent)
		{
			Toast.MakeText (this, "GeoFencing is working", ToastLength.Short).Show();
		}
		#endregion
		
	}
}

