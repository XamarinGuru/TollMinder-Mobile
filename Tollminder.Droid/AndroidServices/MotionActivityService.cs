using System;
using Tollminder.Droid.Helpers;
using Tollminder.Droid.Handlers;
using Android.Gms.Fitness;
using Android.Gms.Fitness.Request;
using Android.Gms.Fitness.Data;
using Android.Gms.Common.Apis;
using Android.Gms.Common;
using Android.App;
using Android.OS;

namespace Tollminder.Droid.AndroidServices
{
	[Service]
	public class MotionActivityService : GoogleApiService<MotionServiceHanlder> , IOnDataPointListener
	{
		
		public static readonly int ResolutionRequest = 101;
		public static readonly string ResultBundleKey = "resultbundle";

		public override void OnCreate ()
		{
			base.OnCreate ();
			GoogleApiClient = new GoogleApiClient.Builder (this)
				.AddApi (FitnessClass.SENSORS_API)
				.AddScope (new Scope (Scopes.FitnessActivityRead))
				.AddScope (new Scope (Scopes.FitnessLocationRead))
				.AddConnectionCallbacks (this)
				.AddOnConnectionFailedListener (this)
				.Build ();
			GoogleApiClient.Connect ();
		}
		public override void OnConnected (Android.OS.Bundle connectionHint)
		{
			base.OnConnected (connectionHint);
			GetFitnessSensor ();
		}

		public override void OnConnectionFailed (ConnectionResult result)
		{
			base.OnConnectionFailed (result);
			if (ConnectionResult.SignInRequired == result.ErrorCode) {
				Bundle resultBundle = new Bundle ();
				resultBundle.PutParcelable (ResultBundleKey, result);
				DroidMessanging.SendMessage (MotionConstants.StartResolutuon, MessengerClient, null, resultBundle);
				return;
			}
		}

		private async void GetFitnessSensor()
		{
			var dataSourceResult = await FitnessClass.SensorsApi.AddAsync (GoogleApiClient, new SensorRequest.Builder ()
				.SetDataType (DataType.TypeActivitySample)
				.Build (), this);
		}

		public void OnDataPoint (DataPoint dataPoint)
		{
			var asd = dataPoint;
		}
	}
}

