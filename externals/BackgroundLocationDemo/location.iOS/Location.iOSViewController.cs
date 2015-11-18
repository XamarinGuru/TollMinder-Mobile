using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;

namespace Location.iOS
{
	public partial class Location_iOSViewController : UIViewController
	{
		#region Computed Properties
		public static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public static LocationManager Manager { get; set;}
		#endregion

		#region Constructors
		public Location_iOSViewController (IntPtr handle) : base (handle)
		{
			// As soon as the app is done launching, begin generating location updates in the location manager
			Manager = new LocationManager();
			Manager.StartLocationUpdates();
		}
		#endregion 

		#region Override Methods
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		#endregion

		#region Public Methods
		public void HandleLocationChanged (object sender, LocationUpdatedEventArgs e)
		{
			// Handle foreground updates
			CLLocation location = e.Location;

			LblAltitude.Text = location.Altitude + " meters";
			LblLongitude.Text = location.Coordinate.Longitude.ToString ();
			LblLatitude.Text = location.Coordinate.Latitude.ToString ();
			LblCourse.Text = location.Course.ToString ();
			LblSpeed.Text = location.Speed.ToString ();

			Console.WriteLine ("foreground updated");
		}

		#endregion

		#region View lifecycle
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// It is better to handle this with notifications, so that the UI updates
			// resume when the application re-enters the foreground!
			// Manager.LocationUpdated += HandleLocationChanged;

			// Screen subscribes to the location changed event
			UIApplication.Notifications.ObserveDidBecomeActive ((sender, args) => {
				Manager.LocationUpdated += HandleLocationChanged;
			});

			// Whenever the app enters the background state, we unsubscribe from the event 
			// so we no longer perform foreground updates
			UIApplication.Notifications.ObserveDidEnterBackground ((sender, args) => {
				Manager.LocationUpdated -= HandleLocationChanged;
			});
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}
		#endregion
	}
}

