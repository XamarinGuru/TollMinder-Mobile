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
	[Register ("PaymentViewController")]
	partial class PaymentViewController
	{
		[Outlet]
		UIKit.UITableView NotPayedTripsTableVIew { get; set; }

		[Outlet]
		UIKit.UINavigationBar PayNavigationBar { get; set; }

		[Outlet]
		UIKit.UINavigationItem PayNavigationItem { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (PayNavigationBar != null) {
				PayNavigationBar.Dispose ();
				PayNavigationBar = null;
			}

			if (PayNavigationItem != null) {
				PayNavigationItem.Dispose ();
				PayNavigationItem = null;
			}

			if (NotPayedTripsTableVIew != null) {
				NotPayedTripsTableVIew.Dispose ();
				NotPayedTripsTableVIew = null;
			}
		}
	}
}
