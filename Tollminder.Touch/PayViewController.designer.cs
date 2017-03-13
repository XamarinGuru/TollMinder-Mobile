// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Tollminder.Touch
{
    [Register("PayViewController")]
    partial class PayViewController
    {
        [Outlet]
        UIKit.UILabel Amount { get; set; }

        [Outlet]
        UIKit.UIButton ApplePayButton { get; set; }

        [Outlet]
        UIKit.UIButton ProceedToCheckout { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIView HeaderView { get; set; }

        [Action("PayWithApplePay:")]
        [GeneratedCode("iOS Designer", "1.0")]
        partial void PayWithApplePay(UIKit.UIButton sender);

        void ReleaseDesignerOutlets()
        {
            if (Amount != null)
            {
                Amount.Dispose();
                Amount = null;
            }

            if (ApplePayButton != null)
            {
                ApplePayButton.Dispose();
                ApplePayButton = null;
            }

            if (HeaderView != null)
            {
                HeaderView.Dispose();
                HeaderView = null;
            }

            if (ProceedToCheckout != null)
            {
                ProceedToCheckout.Dispose();
                ProceedToCheckout = null;
            }
        }
    }
}