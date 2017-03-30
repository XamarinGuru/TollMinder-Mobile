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
	[Register ("NotPayedTripsTableViewCell")]
	partial class NotPayedTripsTableViewCell
	{
		[Outlet]
		UIKit.UILabel AmountLabel { get; set; }

		[Outlet]
		UIKit.UILabel BillingDateLabel { get; set; }

		[Outlet]
		UIKit.UILabel TollRoadLabel { get; set; }

		[Outlet]
		UIKit.UILabel TransactionIdLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (TollRoadLabel != null) {
				TollRoadLabel.Dispose ();
				TollRoadLabel = null;
			}

			if (AmountLabel != null) {
				AmountLabel.Dispose ();
				AmountLabel = null;
			}

			if (BillingDateLabel != null) {
				BillingDateLabel.Dispose ();
				BillingDateLabel = null;
			}

			if (TransactionIdLabel != null) {
				TransactionIdLabel.Dispose ();
				TransactionIdLabel = null;
			}
		}
	}
}
