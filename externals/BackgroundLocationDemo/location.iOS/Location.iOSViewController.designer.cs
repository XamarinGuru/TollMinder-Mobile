// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;

namespace Location.iOS
{
	[Register ("Location_iOSViewController")]
	partial class Location_iOSViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel LblAltitude { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel LblCourse { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel LblLatitude { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel LblLongitude { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel LblSpeed { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (LblAltitude != null) {
				LblAltitude.Dispose ();
				LblAltitude = null;
			}
			if (LblCourse != null) {
				LblCourse.Dispose ();
				LblCourse = null;
			}
			if (LblLatitude != null) {
				LblLatitude.Dispose ();
				LblLatitude = null;
			}
			if (LblLongitude != null) {
				LblLongitude.Dispose ();
				LblLongitude = null;
			}
			if (LblSpeed != null) {
				LblSpeed.Dispose ();
				LblSpeed = null;
			}
		}
	}
}
