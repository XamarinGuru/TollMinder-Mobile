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
    [Register ("CreditCardsViewController")]
    partial class CreditCardsViewController
    {
        [Outlet]
        UIKit.UINavigationBar CreditCardsNavigationBar { get; set; }


        [Outlet]
        UIKit.UINavigationItem CreditCardsNavigationItem { get; set; }


        [Outlet]
        UIKit.UITableView CreditCardsTableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CreditCardsNavigationBar != null) {
                CreditCardsNavigationBar.Dispose ();
                CreditCardsNavigationBar = null;
            }

            if (CreditCardsNavigationItem != null) {
                CreditCardsNavigationItem.Dispose ();
                CreditCardsNavigationItem = null;
            }

            if (CreditCardsTableView != null) {
                CreditCardsTableView.Dispose ();
                CreditCardsTableView = null;
            }
        }
    }
}