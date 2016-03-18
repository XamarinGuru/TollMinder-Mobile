using System;
using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.Locations;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Tollminder.Core.Helpers;

namespace Tollminder.Droid.Services
{
	public class DroidGeolocationReciever : BroadcastReceiver
	{
		public override void OnReceive (Context context, Intent intent)
		{
			ApplicationContext = context;
		}

		private Context _context;
		public virtual Context ApplicationContext {
			get {
				if (_context == null && Mvx.CanResolve<IMvxAndroidCurrentTopActivity> ()) {
					_context = Mvx.Resolve<IMvxAndroidCurrentTopActivity> ().Activity.ApplicationContext;
				}
				return _context;
			} 
			set { _context = value; }
		}
	}
}

