using Android.App;
using Android.Gms.Common.Apis;
using Android.OS;
using Tollminder.Droid.Handlers;
using Tollminder.Core.Helpers;

namespace Tollminder.Droid.AndroidServices
{
	public abstract class GoogleApiService<T> : MessengerGoogleApiService<T>, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener where T : BaseHandler
	{		
		protected GoogleApiClient GoogleApiClient { get; set; }

		public override void OnDestroy ()
		{
			base.OnDestroy ();
			DestroyGoogleApiClient ();
		}

		public virtual void Connect()
		{
			if (!GoogleApiClient.IsConnected) {
				GoogleApiClient.Connect ();				
			}
		}

		public virtual void OnConnectionFailed (Android.Gms.Common.ConnectionResult result)
		{
			Connect ();
		}

		public virtual void OnConnected (Bundle connectionHint)
		{
			
		}

		public virtual void OnConnectionSuspended (int cause)
		{
			Connect ();
		}

		protected virtual void DestroyGoogleApiClient ()
		{
			Log.LogMessage ("API CLIENT DESTYOED");
			GoogleApiClient?.UnregisterConnectionCallbacks (this);
			GoogleApiClient?.UnregisterConnectionFailedListener (this);
			GoogleApiClient?.Disconnect ();
			GoogleApiClient = null;
		}

		protected virtual void CreateGoogleApiClient  (Api api)
		{
			GoogleApiClient = new GoogleApiClient.Builder (this).AddApi (api).AddConnectionCallbacks (this).AddOnConnectionFailedListener (this).Build ();
		}
	}
}