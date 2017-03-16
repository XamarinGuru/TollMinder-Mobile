using System;
using UIKit;
using Tollminder.Touch.Controllers;
using Tollminder.Core.ViewModels;
using PassKit;
using ObjCRuntime;
using Foundation;
using System.Diagnostics;
using MvvmCross.Platform;
using Tollminder.Core.Services.ScanCrediCard;

namespace Tollminder.Touch.Views
{
    public partial class PaymentViewController : BaseViewController<PayViewModel>//, IPKPaymentAuthorizationViewControllerDelegate
    {
        readonly NSString[] supportedPaymentNetworks;
        PKPaymentToken paymentToken;

        public PaymentViewController() : base("PaymentViewController", null)
        {
            supportedPaymentNetworks = new NSString[] {
                PKPaymentNetwork.Amex,
                PKPaymentNetwork.Discover,
                PKPaymentNetwork.MasterCard,
                PKPaymentNetwork.Visa
            };
        }

        public async override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var creditCardScanService = Mvx.Resolve<ICreditCardScanService>();
            var creditCard = await creditCardScanService.ScanCardInfoAsync();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        //public void PaymentAuthorizationViewControllerDidFinish(PKPaymentAuthorizationViewController controller)
        //{
        //    DismissViewController(true, null);
        //}

        //public void WillAuthorizePayment(PKPaymentAuthorizationViewController controller)
        //{
        //}

        //public void PerformApplePayCompletion(PKPaymentAuthorizationViewController controller, UIAlertController alert)
        //{
        //    DismissViewController(true, () =>
        //    {
        //        PresentViewController(this, true, null);
        //    });
        //}

        //private void ShowAuthorizationAlert(string message, UIAlertActionStyle style)
        //{
        //    var alert = new UIAlertController(message, null);
        //    alert.AddAction(UIKit.UIAlertAction.Create("OK", style, null));
        //    PresentViewController(alert, true, null);
        //}

        //private void PayWithApplePay(object sender, EventArgs e)
        //{
        //    if (PKPaymentAuthorizationViewController.CanMakePayments == false)
        //    {
        //        ShowAuthorizationAlert("Apple Pay is not available", UIAlertActionStyle.Cancel);
        //    }

        //    if (PKPaymentAuthorizationViewController.CanMakePaymentsUsingNetworks(supportedPaymentNetworks) == false)
        //    {
        //        ShowAuthorizationAlert("No Apple Pay payment methods available", UIAlertActionStyle.Cancel);
        //    }

        //    var request = new PKPaymentRequest();
        //    request.CurrencyCode = "USD";
        //    request.CountryCode = "US";
        //    request.MerchantIdentifier = "merchant.authorize.net.test.dev13";
        //    request.SupportedNetworks = supportedPaymentNetworks;
        //    // DO NOT INCLUDE PKMerchantCapability.capabilityEMV
        //    request.MerchantCapabilities = PKMerchantCapability.ThreeDS;

        //    request.PaymentSummaryItems = new PKPaymentSummaryItem[] { PKPaymentSummaryItem.Create("Total", new NSDecimalNumber("200")) };
        //    try
        //    {
        //        var applePayController = new PKPaymentAuthorizationViewController(request);
        //        applePayController.Delegate = this;

        //        PresentViewController(applePayController, true, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //    }
        //}

        //public override void ViewWillAppear(bool animated)
        //{
        //    base.ViewWillAppear(animated);

        //    // Display an Apple Pay button if a payment card is available. In your
        //    // app, you might divert the user to a more traditional checkout if
        //    // Apple Pay wasn't set up.

        //    if (PKPaymentAuthorizationViewController.CanMakePaymentsUsingNetworks(supportedPaymentNetworks))
        //    {
        //        var button = new PKPaymentButton(PKPaymentButtonType.Buy, PKPaymentButtonStyle.Black);
        //        button.TouchUpInside += PayWithApplePay;
        //        ProceedToCheckout.TouchUpInside += PayWithApplePay;

        //        button.Center = ApplePayView.Center;
        //        button.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin;
        //        ApplePayView.AddSubview(button);
        //    }
        //}

        ///// <summary>
        ///// Whenever the user changed their shipping information we will receive a callback here.
        /////
        ///// Note that for privacy reasons the contact we receive will be redacted,
        ///// and only have a city, ZIP, and country.
        /////
        ///// You can use this method to estimate additional shipping charges and update
        ///// the payment summary items.
        ///// </summary>
        //[Export("paymentAuthorizationViewController:didSelectShippingContact:completion:")]
        //void DidSelectShippingContact(PKPaymentAuthorizationViewController controller, PKContact contact, PKPaymentShippingAddressSelected completion)
        //{
        //    // Create a shipping method. Shipping methods use PKShippingMethod,
        //    // which inherits from PKPaymentSummaryItem. It adds a detail property
        //    // you can use to specify information like estimated delivery time.
        //    var shipping = new PKShippingMethod
        //    {
        //        Label = "Standard Shipping",
        //        Amount = NSDecimalNumber.Zero,
        //        Detail = "Delivers within two working days"
        //    };

        //    // Note that this is a contrived example. Because addresses can come from
        //    // many sources on iOS they may not always have the fields you want.
        //    // Your application should be sure to verify the address is correct,
        //    // and return the appropriate status. If the address failed to pass validation
        //    // you should return `InvalidShippingPostalAddress` instead of `Success`.

        //    var address = contact.PostalAddress;
        //    var requiresInternationalSurcharge = address.Country != "United States";

        //    PKPaymentSummaryItem[] summaryItems = new PKPaymentSummaryItem[] { PKPaymentSummaryItem.Create("Total", new NSDecimalNumber(AmountTextField.Text)) };//MakeSummaryItems(requiresInternationalSurcharge);

        //    completion(PKPaymentAuthorizationStatus.Success, new[] { shipping }, summaryItems);
        //}

        ///// <summary>
        ///// This is where you would send your payment to be processed - here we will
        ///// simply present a confirmation screen. If your payment processor failed the
        ///// payment you would return `completion(PKPaymentAuthorizationStatus.Failure)` instead. Remember to never
        ///// attempt to decrypt the payment token on device.
        ///// </summary>
        //public void DidAuthorizePayment(PKPaymentAuthorizationViewController controller, PKPayment payment, Action<PKPaymentAuthorizationStatus> completion)
        //{
        //    if (payment.Token.PaymentData != null)
        //    {
        //        paymentToken = payment.Token;
        //        completion(PKPaymentAuthorizationStatus.Success);
        //        //PerformSegue(confirmationSegue, this);
        //        ShowAuthorizationAlert("Authorization Success", UIAlertActionStyle.Cancel);

        //    }
        //    else
        //        ShowAuthorizationAlert("Authorization Failed!", UIAlertActionStyle.Default);
        //}
    }
}

