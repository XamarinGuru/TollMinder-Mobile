using Tollminder.Core.Services;
using Tollminder.Core.Models;
using Tollminder.Core.Helpers;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Platform;
using System;
using System.Reactive.Linq;

namespace Tollminder.Touch.Services
{
	public class TouchGeolocationWatcher : TouchLocation, IGeoLocationWatcher
	{	
		private IObservable<long> _pollUpdates;
		private IDisposable _subscription;
	
		public TouchGeolocationWatcher ()
		{
			_pollUpdates = Observable.Interval (TimeSpan.FromSeconds (20));
		}

		public bool IsBound { get; private set; } = false;

		public override GeoLocation Location {
			get { return base.Location;	}
			set {
				base.Location = value;
				#if DEBUG
				Log.LogMessage ("MESSAGE WITH LOCATION PUBLISH");
				#endif
				Mvx.Resolve<IMvxMessenger> ().Publish (new LocationMessage (this, Location));
				#if RELEASE
				Log.LogMessage (value.ToString ());
				#endif
			}
		}

		#region IGeoLocationWatcher implementation

		public virtual void StartGeolocationWatcher()
		{
			if (!IsBound) {
				StartLocationUpdates ();
				CreateIntervalUpdate ();
				IsBound = true;				
			}	
		}

		public virtual void StopGeolocationWatcher ()
		{
			if (IsBound) {
				DestoyIntervalUpdate ();
				StoptLocationUpdates ();
				IsBound = false;
			}
		}

		protected override void LocationIsUpdated (object sender, CoreLocation.CLLocationsUpdatedEventArgs e)
		{
			base.LocationIsUpdated (sender, e);
			StoptLocationUpdates ();
		}

		protected virtual void DestoyIntervalUpdate ()
		{
			_subscription?.Dispose ();
		}

		protected virtual void CreateIntervalUpdate ()
		{
			_subscription = _pollUpdates.Subscribe ((_) => {
				StartLocationUpdates ();
			});
		}

		public virtual void StartUpdatingHighAccuracyLocation ()
		{
			DestoyIntervalUpdate ();
			_pollUpdates = Observable.Interval (TimeSpan.FromSeconds (4));
		}

		public virtual void StopUpdatingHighAccuracyLocation ()
		{
			DestoyIntervalUpdate ();
			_pollUpdates = Observable.Interval (TimeSpan.FromSeconds (20));
		}
		#endregion
	}
}