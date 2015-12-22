using System;
using Android.OS;
using Tollminder.Droid.Handlers;
using Tollminder.Droid.Inerfaces;
using Tollminder.Droid.Helpers;

namespace Tollminder.Droid.Services
{
	public class MessengerAndroidService<T> : BaseAndroidService, IDroidMessenger where T : BaseHandler
	{
		public virtual Messenger Messenger { get; set; } 
		public virtual Messenger MessengerService { get; set; }
		protected T ClientHandler { get; set; }

		public override void Start ()
		{
			base.Start ();
			ClientHandler = (T)Activator.CreateInstance (typeof(T), this); 
			Messenger = new Messenger (ClientHandler);
		}

		public override void Stop ()
		{
			base.Stop ();
			DroidMessanging.SendMessage(ServiceConstants.UnregisterClient,MessengerService,Messenger);
			ClientHandler.Service = null;
			Messenger = null;
		}
	}
}

