using System;
using Android.Content;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Tollminder.Droid.AndroidServices;

namespace Tollminder.Droid.Services
{
	public class DroidGeolocationWatcher : DroidServiceStarter, IGeoLocationWatcher
	{
		#region IGeoLocationWatcher implementation
		public DroidGeolocationWatcher ()
		{
			ServiceIntent = new Intent (ApplicationContext, typeof (GeolocationService));
		}

		public bool IsBound { get; private set; } = false;

		private GeoLocation _location;
		public virtual GeoLocation Location {
			get {
				return _location;
			}
			set {
				_location = value;
				Mvx.Resolve<IMvxMessenger> ().Publish (new LocationMessage (this, value));
#if DEBUG
				Log.LogMessage (value.ToString ());
#endif
			}
		}

		public virtual void StartGeolocationWatcher ()
		{
			if (!IsBound && ApplicationContext.IsGooglePlayServicesInstalled ()) {
				Start ();
				IsBound = true;
			}
		}

		public virtual void StopGeolocationWatcher ()
		{
			if (IsBound) {
				Stop ();
				IsBound = false;
			}
		}

		public virtual void StartUpdatingHighAccuracyLocation ()
		{
			if (IsBound) {
				Stop ();
				ServiceIntent.PutExtra ("interval", 20);
				Start ();
			}
		}

		public virtual void StopUpdatingHighAccuracyLocation ()
		{
			if (IsBound) {
				Stop ();
				ServiceIntent.PutExtra ("interval", 400);
				Start ();
			}
		}
		#endregion
	}
}