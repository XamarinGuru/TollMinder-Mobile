// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
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
            if (AmountLabel != null) {
                AmountLabel.Dispose ();
                AmountLabel = null;
            }

            if (BillingDateLabel != null) {
                BillingDateLabel.Dispose ();
                BillingDateLabel = null;
            }

            if (TollRoadLabel != null) {
                TollRoadLabel.Dispose ();
                TollRoadLabel = null;
            }

            if (TransactionIdLabel != null) {
                TransactionIdLabel.Dispose ();
                TransactionIdLabel = null;
            }
        }
    }
}