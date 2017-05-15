// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
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

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel AmountLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AmountLabel != null) {
                AmountLabel.Dispose ();
                AmountLabel = null;
            }

            if (NotPayedTripsTableVIew != null) {
                NotPayedTripsTableVIew.Dispose ();
                NotPayedTripsTableVIew = null;
            }

            if (PayNavigationBar != null) {
                PayNavigationBar.Dispose ();
                PayNavigationBar = null;
            }

            if (PayNavigationItem != null) {
                PayNavigationItem.Dispose ();
                PayNavigationItem = null;
            }
        }
    }
}