using Android.App;
using Android.Gms.Common.Apis;
using Android.OS;

namespace Tollminder.Droid.AndroidServices
{
	public abstract class GoogleApiService : Service, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
	{		
		protected GoogleApiClient GoogleApiClient { get; set; }

		public override void OnDestroy ()
		{
			base.OnDestroy ();
			DestroyGoogleApiClient ();
		}

		public virtual void OnConnectionFailed (Android.Gms.Common.ConnectionResult result)
		{
			
		}

		public virtual void OnConnected (Bundle connectionHint)
		{
			
		}

		public virtual void OnConnectionSuspended (int cause)
		{
			
		}

		protected virtual void DestroyGoogleApiClient ()
		{
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