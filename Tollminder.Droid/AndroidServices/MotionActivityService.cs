using System;
using Tollminder.Droid.Helpers;
using Tollminder.Droid.Handlers;
using Android.Gms.Common.Apis;
using Android.Gms.Common;
using Android.App;
using Android.OS;
using Java.Util.Concurrent;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using Android.Gms.Location;
using Android.Content;

namespace Tollminder.Droid.AndroidServices
{
	[Service(Enabled = true, Exported = false)]
	public class MotionActivityService : GoogleApiService<MotionServiceHanlder> 
	{
		
		public static readonly int ResolutionRequest = 101;
		public static readonly string ResultBundleKey = "resultbundle";

		public override void OnCreate ()
		{
			base.OnCreate ();
			CreateGoogleApiClient (ActivityRecognition.API);
			Connect ();
		}
		public override void OnConnected (Android.OS.Bundle connectionHint)
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
			var status = await ActivityRecognition.ActivityRecognitionApi.RequestActivityUpdatesAsync (GoogleApiClient, 1000, GetActivityPendingIntent);
			#if DEBUG
			Log.LogMessage(string.Format ("GoogleApiClient connected : {0}", status));
			#endif
		}

		public virtual async void StopMotionSerivce ()
		{
			if (GoogleApiClient != null && GoogleApiClient.IsConnected) {
				var status = await ActivityRecognition.ActivityRecognitionApi.RemoveActivityUpdatesAsync (GoogleApiClient, GetActivityPendingIntent);
				#if DEBUG
				Log.LogMessage(string.Format ("GoogleApiClient connected : {0}", status));
				#endif				
			}
		}

		public override StartCommandResult OnStartCommand (Android.Content.Intent intent, StartCommandFlags flags, int startId)
		{			
			ActivityRecognitionResult result = ActivityRecognitionResult.ExtractResult (intent);
			#if DEBUG
			Log.LogMessage(string.Format ("Most probable reuslt - {0}", result));
			#endif
			SendMessage (result?.MostProbableActivity.GetMotionType ().PutMotionType ());
			#if DEBUG
			Log.LogMessage(string.Format ("Most probable reuslt - {0}", result.MostProbableActivity.GetMotionType ()));
			#endif
			return base.OnStartCommand (intent, flags, startId);


		}

		protected virtual void SendMessage (Bundle bundle)
		{
			if (MessengerClient != null) {
				DroidMessanging.SendMessage (MotionConstants.GetMotion, MessengerClient, null, bundle);
			}
		}

		private PendingIntent _motionActivityPendingIntent;
		private PendingIntent GetActivityPendingIntent 
		{
			get {
				if (_motionActivityPendingIntent != null) {
					return _motionActivityPendingIntent;
				}
				Intent intent = new Intent (this, typeof(MotionActivityService));
				_motionActivityPendingIntent = PendingIntent.GetService (this, 0, intent, PendingIntentFlags.UpdateCurrent);
				return _motionActivityPendingIntent;
			}
		}
	}
}

