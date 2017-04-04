// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Tollminder.Touch.Views.PaymentViews
{
	[Register ("CardsForPayTableViewCell")]
	partial class CardsForPayTableViewCell
	{
		[Outlet]
		UIKit.UINavigationBar CardsForPayNavigationBar { get; set; }

		[Outlet]
		UIKit.UINavigationItem CardsForPayNavigationItem { get; set; }

		[Outlet]
		UIKit.UITableView CardsForPayTableVIew { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CardsForPayNavigationBar != null) {
				CardsForPayNavigationBar.Dispose ();
				CardsForPayNavigationBar = null;
			}

			if (CardsForPayNavigationItem != null) {
				CardsForPayNavigationItem.Dispose ();
				CardsForPayNavigationItem = null;
			}

			if (CardsForPayTableVIew != null) {
				CardsForPayTableVIew.Dispose ();
				CardsForPayTableVIew = null;
			}
		}
	}
}
