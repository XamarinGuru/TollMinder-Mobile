using Android.App;
using Android.Content;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.OS;
using Tollminder.Core.Helpers;

namespace Tollminder.Droid.AndroidServices
{
	[Service(Enabled = true, Exported = false)]
	public class MotionActivityService : GoogleApiService
	{
		
		public static readonly int ResolutionRequest = 101;
		public static readonly string ResultBundleKey = "resultbundle";

		public override void OnCreate ()
		{
			base.OnCreate ();
			CreateGoogleApiClient (ActivityRecognition.API);
			Connect ();
		}

		public override void OnDestroy ()
		{
			base.OnDestroy ();
			StopMotionSerivce ();
			ActivityPendingIntent.Cancel ();
		}

		public override void OnConnected (Bundle connectionHint)
		{
			base.OnConnected (connectionHint);
			StartMotionService ();
		}

		public virtual async void StartMotionService()
		{
			if (!GoogleApiClient.IsConnected) {
				Connect ();
				return;
			}
			StopMotionSerivce ();
			var status = await ActivityRecognition.ActivityRecognitionApi.RequestActivityUpdatesAsync (GoogleApiClient, 1000, ActivityPendingIntent);
			Log.LogMessage(string.Format ("GoogleApiClient connected : {0}", status));
		}

		public virtual async void StopMotionSerivce ()
		{
			if (GoogleApiClient != null && GoogleApiClient.IsConnected) {
				var status = await ActivityRecognition.ActivityRecognitionApi.RemoveActivityUpdatesAsync (GoogleApiClient, ActivityPendingIntent);
				Log.LogMessage(string.Format ("GoogleApiClient connected : {0}", status));
			}
		}

		public override IBinder OnBind (Intent intent)
		{
			return null;
		}

		private PendingIntent _motionActivityPendingIntent;
		private PendingIntent ActivityPendingIntent 
		{
			get {
				if (_motionActivityPendingIntent != null) {
					return _motionActivityPendingIntent;
				}
				Intent intent = new Intent ("com.tollminder.MotionReciever");
				intent.SetPackage ("com.tollminder");
				_motionActivityPendingIntent = PendingIntent.GetBroadcast (this, 1, intent, PendingIntentFlags.UpdateCurrent);
				return _motionActivityPendingIntent;
			}
		}
	}
}

