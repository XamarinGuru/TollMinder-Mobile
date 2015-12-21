using Android.OS;
using Tollminder.Droid.Handlers;
using System;

namespace Tollminder.Droid.AndroidServices
{	
	public abstract class MessengerGoogleApiService<T> : GoogleApiService where T : BaseHandler
	{
		public Messenger MessengerClient { get; set; }

		private Messenger _messenger;
		private T _messengerHandler;

		public override IBinder OnBind (Android.Content.Intent intent)
		{
			return _messenger.Binder;
		}

		public override void OnCreate ()
		{
			base.OnCreate ();
			CreateMessenger ();
		}

		public override void OnDestroy ()
		{
			base.OnDestroy ();
			DestroyMessenger ();
		}

		protected virtual void CreateMessenger ()
		{
			_messengerHandler = (T)Activator.CreateInstance (typeof(T), this);
			_messenger = new Messenger (_messengerHandler);
		}

		protected virtual void DestroyMessenger ()
		{
			_messengerHandler.Dispose ();
			_messengerHandler = null;
			_messenger = null;
		}
	}
}

