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
        UIKit.UITextField AmountTextField { get; set; }


        [Outlet]
        UIKit.UIView ApplePayView { get; set; }


        [Outlet]
        UIKit.UIButton ProceedToCheckout { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AmountTextField != null) {
                AmountTextField.Dispose ();
                AmountTextField = null;
            }

            if (ApplePayView != null) {
                ApplePayView.Dispose ();
                ApplePayView = null;
            }

            if (ProceedToCheckout != null) {
                ProceedToCheckout.Dispose ();
                ProceedToCheckout = null;
            }
        }
    }
}