// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Tollminder.Touch.Views
{
	[Register ("HomeView")]
	partial class HomeView
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel ActivityLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel GeoLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel GeoLabelData { get; set; }

		void ReleaseDesignerOutlets ()
		{
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
		}
	}
}
