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
    [Register("CreditCardViewController")]
    partial class CreditCardViewController
    {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView ActivityIndicatorAcceptSDKDemo { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UITextField CardNumberTextField { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UITextField CardVerificationCodeTextField { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UITextField ExpirationMonthTextField { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UITextField ExpirationYearTextField { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIButton GetTokenButton { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIView HeaderView { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UITextView TextViewShowResults { get; set; }

        [Action("BackButtonButtonTapped:")]
        [GeneratedCode("iOS Designer", "1.0")]
        partial void BackButtonButtonTapped(UIKit.UIButton sender);

        [Action("GetTokenButtonTapped:")]
        [GeneratedCode("iOS Designer", "1.0")]
        partial void GetTokenButtonTapped(UIKit.UIButton sender);

        [Action("HideKeyBoard:")]
        [GeneratedCode("iOS Designer", "1.0")]
        partial void HideKeyBoard(UIKit.UITapGestureRecognizer sender);

        void ReleaseDesignerOutlets()
        {
            if (ActivityIndicatorAcceptSDKDemo != null)
            {
                ActivityIndicatorAcceptSDKDemo.Dispose();
                ActivityIndicatorAcceptSDKDemo = null;
            }

            if (CardNumberTextField != null)
            {
                CardNumberTextField.Dispose();
                CardNumberTextField = null;
            }

            if (CardVerificationCodeTextField != null)
            {
                CardVerificationCodeTextField.Dispose();
                CardVerificationCodeTextField = null;
            }

            if (ExpirationMonthTextField != null)
            {
                ExpirationMonthTextField.Dispose();
                ExpirationMonthTextField = null;
            }

            if (ExpirationYearTextField != null)
            {
                ExpirationYearTextField.Dispose();
                ExpirationYearTextField = null;
            }

            if (GetTokenButton != null)
            {
                GetTokenButton.Dispose();
                GetTokenButton = null;
            }

            if (HeaderView != null)
            {
                HeaderView.Dispose();
                HeaderView = null;
            }

            if (TextViewShowResults != null)
            {
                TextViewShowResults.Dispose();
                TextViewShowResults = null;
            }
        }
    }
}