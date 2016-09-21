// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Tollminder.Touch.Views
{
	[Register ("HomeView")]
	partial class HomeView
	{
		[Outlet]
		UIKit.UILabel ActivityLabel { get; set; }

		[Outlet]
		UIKit.UILabel GeoLabel { get; set; }

		[Outlet]
		UIKit.UILabel GeoLabelData { get; set; }

		[Outlet]
		UIKit.UITextView LogArea { get; set; }

		[Outlet]
		UIKit.UIButton StartButton { get; set; }

		[Outlet]
		UIKit.UILabel StatusLabel { get; set; }

		[Outlet]
		UIKit.UIButton StopButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (StatusLabel != null) {
				StatusLabel.Dispose ();
				StatusLabel = null;
			}

			if (ActivityLabel != null) {
				ActivityLabel.Dispose ();
				ActivityLabel = null;
			}

			if (GeoLabel != null) {
				GeoLabel.Dispose ();
				GeoLabel = null;
			}

			if (GeoLabelData != null) {
				GeoLabelData.Dispose ();
				GeoLabelData = null;
			}

			if (LogArea != null) {
				LogArea.Dispose ();
				LogArea = null;
			}

			if (StartButton != null) {
				StartButton.Dispose ();
				StartButton = null;
			}

			if (StopButton != null) {
				StopButton.Dispose ();
				StopButton = null;
			}
		}
	}
}
