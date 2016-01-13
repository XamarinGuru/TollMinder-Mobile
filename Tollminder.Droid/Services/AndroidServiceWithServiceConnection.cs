using Android.App;
using Android.Content;
using Tollminder.Droid.Inerfaces;
using Tollminder.Droid.ServicesConnections;
using Tollminder.Droid.Handlers;
using Tollminder.Droid.Helpers;

namespace Tollminder.Droid.Services
{
	/// <summary>
	/// Android service object with service connection.
	/// Where T is android service that you bind.
	/// Where Y is client handler what will handle service messages.
	/// Where Z is service connection type.
	/// </summary>
	public class AndroidServiceWithServiceConnection<T,Y,Z> : MessengerAndroidService<Y> where T : Service where Y : BaseHandler where Z : BaseServiceConnection , new () 
	{
		private readonly Intent ServiceIntent;
		protected virtual BaseServiceConnection ServiceConnection { get; private set; }

		public AndroidServiceWithServiceConnection ()
		{
 			ServiceIntent = new Intent (ApplicationContext, typeof(T));
			ServiceConnection = new Z ();
		}

		public override void Start ()
		{
			base.Start ();
			ServiceConnection.Messenger = this;
			ApplicationContext.BindService (ServiceIntent, ServiceConnection, Bind.AutoCreate);
		}

		public override void Stop ()
		{
			base.Stop ();
			ApplicationContext.UnbindService (ServiceConnection);
			ServiceConnection.Messenger = null;
		}
	}
}