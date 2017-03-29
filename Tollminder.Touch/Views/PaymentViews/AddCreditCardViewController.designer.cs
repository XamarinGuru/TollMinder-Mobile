// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Tollminder.Touch.Views
{
    [Register ("AddCreditCardViewController")]
    partial class AddCreditCardViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView AcceptedCardsImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UINavigationBar AddCreditCardNavigationBar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UINavigationItem AddCreditCardNavigationItem { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField CardNumberTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField CvvTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField ExpirationMonthTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField ExpirationYearTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField NameOnCardTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SaveCreditCardButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField ZipCodeTextField { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AcceptedCardsImageView != null) {
                AcceptedCardsImageView.Dispose ();
                AcceptedCardsImageView = null;
            }

            if (AddCreditCardNavigationBar != null) {
                AddCreditCardNavigationBar.Dispose ();
                AddCreditCardNavigationBar = null;
            }

            if (AddCreditCardNavigationItem != null) {
                AddCreditCardNavigationItem.Dispose ();
                AddCreditCardNavigationItem = null;
            }

            if (CardNumberTextField != null) {
                CardNumberTextField.Dispose ();
                CardNumberTextField = null;
            }

            if (CvvTextField != null) {
                CvvTextField.Dispose ();
                CvvTextField = null;
            }

            if (ExpirationMonthTextField != null) {
                ExpirationMonthTextField.Dispose ();
                ExpirationMonthTextField = null;
            }

            if (ExpirationYearTextField != null) {
                ExpirationYearTextField.Dispose ();
                ExpirationYearTextField = null;
            }

            if (NameOnCardTextField != null) {
                NameOnCardTextField.Dispose ();
                NameOnCardTextField = null;
            }

            if (SaveCreditCardButton != null) {
                SaveCreditCardButton.Dispose ();
                SaveCreditCardButton = null;
            }

            if (ZipCodeTextField != null) {
                ZipCodeTextField.Dispose ();
                ZipCodeTextField = null;
            }
        }
    }
}