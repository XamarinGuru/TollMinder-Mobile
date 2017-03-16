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
