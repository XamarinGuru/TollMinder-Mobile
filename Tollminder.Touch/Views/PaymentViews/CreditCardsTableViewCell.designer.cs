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
    [Register ("CreditCardsTableViewCell")]
    partial class CreditCardsTableViewCell
    {
        [Outlet]
        UIKit.UILabel CreditCardLastDigits { get; set; }


        [Outlet]
        UIKit.UIButton RemoveCrediCard { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CreditCardLastDigits != null) {
                CreditCardLastDigits.Dispose ();
                CreditCardLastDigits = null;
            }

            if (RemoveCrediCard != null) {
                RemoveCrediCard.Dispose ();
                RemoveCrediCard = null;
            }
        }
    }
}