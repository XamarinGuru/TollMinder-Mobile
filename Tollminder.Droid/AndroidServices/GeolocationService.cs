using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.Locations;
using Android.OS;
using MvvmCross.Droid.Platform;
using MvvmCross.Platform;
using Tollminder.Core.Helpers;
using Tollminder.Core.Services.GeoData;
using Tollminder.Core.Services.Settings;
using Tollminder.Droid.BroadcastReceivers;

namespace Tollminder.Droid.AndroidServices
{
    [Service(Enabled = true, Exported = false)]
    public class GeolocationService : GoogleApiService, Android.Gms.Location.ILocationListener
    {
        public static string DistanceIntervalString = "distance_interval";
        public static string TimeIntervalString = "time_interval";

        LocationRequest _request;

        public virtual int DistanceInterval { get; set; }
        public virtual int TimeInterval { get; set; }

        GeolocationReceiver _reciever;
        public GeolocationReceiver Reciever
        {
            get
            {
                return _reciever ?? (_reciever = new GeolocationReceiver());
            }
        }

        PendingIntent _geolocationPendingIntent;
        protected PendingIntent GeolocationPendingIntent
        {
            get
            {
                return _geolocationPendingIntent ?? (_geolocationPendingIntent = PendingIntent.GetBroadcast(this, 0, new Intent("com.tollminder.GeolocationReciever").
                                                                                                            SetPackage("com.tollminder"), PendingIntentFlags.UpdateCurrent));
            }
        }

        protected virtual LocationRequest LocationRequest
        {
            get
            {
                return _request ?? (_request = new LocationRequest().SetPriority(LocationRequest.PriorityHighAccuracy).SetSmallestDisplacement(DistanceInterval).SetInterval(TimeInterval));
            }
        }

        public override void OnCreate()
        {
            base.OnCreate();
            RegisterGeolocationBroadcastReceiver();
            CreateGoogleApiClient(LocationServices.API);
            Connect();
        }

        void RegisterGeolocationBroadcastReceiver()
        {
            var intentFilter = new IntentFilter();
            intentFilter.AddAction("com.tollminder.GeolocationReciever");
            RegisterReceiver(Reciever, intentFilter);
        }

        void UnRegisterGeolocationBroadcastReceiver()
        {
            UnregisterReceiver(Reciever);
            GeolocationPendingIntent.Cancel();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            DistanceInterval = intent.GetIntExtra(DistanceIntervalString, SettingsService.DistanceIntervalDefault);
            TimeInterval = intent.GetIntExtra(TimeIntervalString, SettingsService.TimeIntervalDefault);
            return StartCommandResult.RedeliverIntent;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            StopLocationUpdate();
            UnRegisterGeolocationBroadcastReceiver();
        }

        public virtual void StopLocationUpdate()
        {
            if (GoogleApiClient != null && GoogleApiClient.IsConnected)
            {
                LocationServices.FusedLocationApi.RemoveLocationUpdates(GoogleApiClient, GeolocationPendingIntent);
            }
        }

        public virtual void StartLocationUpdate()
        {
            Log.LogMessage("START LOCATION UPDATES ");
            LocationServices.FusedLocationApi.RequestLocationUpdates(GoogleApiClient, LocationRequest, GeolocationPendingIntent);
        }

        public override void OnConnected(Bundle connectionHint)
        {
            base.OnConnected(connectionHint);
            StartLocationUpdate();
            Log.LogMessage("GoogleApiClient connected");
        }

        public override void OnConnectionFailed(Android.Gms.Common.ConnectionResult result)
        {
            base.OnConnectionFailed(result);
        }

        public override void OnConnectionSuspended(int cause)
        {
            base.OnConnectionSuspended(cause);
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public void OnLocationChanged(Location location)
        {
            var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(Application.Context);
            setup.EnsureInitialized();

            Mvx.Resolve<IGeoLocationWatcher>().Location = location.GetGeolocationFromAndroidLocation();
        }
    }
}