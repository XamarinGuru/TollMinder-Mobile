// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace TestGPS
{
	[Register ("HomeViewControler")]
	partial class HomeViewControler
	{
		[Outlet]
		MonoTouch.UIKit.UITableView locationTable { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView mapView { get; set; }

		[Outlet]
		MonoTouch.MapKit.MKMapView map { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel timeStamp { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView locationView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel horizontalAccuracy { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel carse { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel verticalAccuracy { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIToolbar toolBr { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIPickerView picker { get; set; }

		[Outlet]
		MonoTouch.UIKit.UISwitch serviceSwitch { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel label { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField accuracyFiield { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField trackMinute { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel latitudeLbl { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel longitudeLbl { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel altitudeLbl { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel sppeed { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton biginToUpdate { get; set; }

		[Action ("donePickerPress:")]
		partial void donePickerPress (MonoTouch.Foundation.NSObject sender);

		[Action ("enabledStateChanged:")]
		partial void enabledStateChanged (MonoTouch.Foundation.NSObject sender);

		[Action ("callLocation:")]
		partial void callLocation (MonoTouch.Foundation.NSObject sender);

		[Action ("beginLocation:")]
		partial void beginLocation (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (locationTable != null) {
				locationTable.Dispose ();
				locationTable = null;
			}

			if (mapView != null) {
				mapView.Dispose ();
				mapView = null;
			}

			if (map != null) {
				map.Dispose ();
				map = null;
			}

			if (timeStamp != null) {
				timeStamp.Dispose ();
				timeStamp = null;
			}

			if (locationView != null) {
				locationView.Dispose ();
				locationView = null;
			}

			if (horizontalAccuracy != null) {
				horizontalAccuracy.Dispose ();
				horizontalAccuracy = null;
			}

			if (carse != null) {
				carse.Dispose ();
				carse = null;
			}

			if (verticalAccuracy != null) {
				verticalAccuracy.Dispose ();
				verticalAccuracy = null;
			}

			if (toolBr != null) {
				toolBr.Dispose ();
				toolBr = null;
			}

			if (picker != null) {
				picker.Dispose ();
				picker = null;
			}

			if (serviceSwitch != null) {
				serviceSwitch.Dispose ();
				serviceSwitch = null;
			}

			if (label != null) {
				label.Dispose ();
				label = null;
			}

			if (accuracyFiield != null) {
				accuracyFiield.Dispose ();
				accuracyFiield = null;
			}

			if (trackMinute != null) {
				trackMinute.Dispose ();
				trackMinute = null;
			}

			if (latitudeLbl != null) {
				latitudeLbl.Dispose ();
				latitudeLbl = null;
			}

			if (longitudeLbl != null) {
				longitudeLbl.Dispose ();
				longitudeLbl = null;
			}

			if (altitudeLbl != null) {
				altitudeLbl.Dispose ();
				altitudeLbl = null;
			}

			if (sppeed != null) {
				sppeed.Dispose ();
				sppeed = null;
			}

			if (biginToUpdate != null) {
				biginToUpdate.Dispose ();
				biginToUpdate = null;
			}
		}
	}
}
