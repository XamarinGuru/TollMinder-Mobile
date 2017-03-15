using System;
using PassKit;
using Tollminder.Touch.Controllers;
using UIKit;
using Tollminder.Core.ViewModels;
using ObjCRuntime;
using Foundation;
using MvvmCross.iOS.Views;
using MvvmCross.Binding.BindingContext;

namespace Tollminder.Touch
{
    public partial class PayViewController : MvxViewController, IPKPaymentAuthorizationViewControllerDelegate
    {
        readonly NSString[] supportedPaymentNetworks = {
            PKPaymentNetwork.Amex,
            PKPaymentNetwork.Discover,
            PKPaymentNetwork.MasterCard,
            PKPaymentNetwork.Visa
        };
        static readonly NSString confirmationSegue = (NSString)"CreditCardData";
        PKPaymentToken paymentToken;
        //public new PayViewModel ViewModel
        //{
        //    get { return (PayViewModel)base.ViewModel; }
        //    set { base.ViewModel = value; }
        //}

        public PayViewController() : base("PayViewController", null)
        {
        }

        public PayViewController(IntPtr handle) : base(handle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //this.CreateBinding(ApplePayButton).To<PayViewModel>(vm => vm.GoToCreditCardDataCommand).Apply();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public void PaymentAuthorizationViewControllerDidFinish(PKPaymentAuthorizationViewController controller)
        {
            DismissViewController(true, null);
        }

        public void WillAuthorizePayment(PKPaymentAuthorizationViewController controller)
        {
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier == confirmationSegue)
            {
                var nextViewController = segue.DestinationViewController as CreditCardViewController;
                nextViewController.Request = new MvxViewModelInstanceRequest(new CreditCardViewModel());
                //var viewController = (ConfirmationViewController)segue.DestinationViewController;
                //viewController.TransactionIdentifier = paymentToken.TransactionIdentifier;
            }
            else {
                base.PrepareForSegue(segue, sender);
            }
        }

        public void PerformApplePayCompletion(PKPaymentAuthorizationViewController controller, UIAlertController alert)
        {
            DismissViewController(true, () =>
            {
                PresentViewController(this, true, null);
            });
        }

        private void ShowAuthorizationAlert(string message, UIAlertActionStyle style)
        {
            var alert = new UIAlertController(message, null);
            alert.AddAction(UIKit.UIAlertAction.Create("OK", style, null));
            PresentViewController(alert, true, null);
        }

        //    public void Base64forData(CoreFoundation.DispatchSource.Data data) -> String {
        //    let charSet = CharacterSet.urlQueryAllowed

        //    let paymentString = NSString(data: theData, encoding: String.Encoding.utf8.rawValue)!.addingPercentEncoding(withAllowedCharacters: charSet)

        //    return paymentString!
        //}

        private void PayWithApplePay(object sender, EventHandler e)
        {

            if (PKPaymentAuthorizationViewController.CanMakePayments == false)
            {
                ShowAuthorizationAlert("Apple Pay is not available", UIAlertActionStyle.Cancel);
            }

            if (PKPaymentAuthorizationViewController.CanMakePaymentsUsingNetworks(supportedPaymentNetworks) == false)
            {
                ShowAuthorizationAlert("No Apple Pay payment methods available", UIAlertActionStyle.Cancel);
            }

            var request = new PKPaymentRequest();
            request.CurrencyCode = "USD";
            request.CountryCode = "US";
            request.MerchantIdentifier = "merchant.authorize.net.test.dev15";
            request.SupportedNetworks = supportedPaymentNetworks;
            // DO NOT INCLUDE PKMerchantCapability.capabilityEMV
            request.MerchantCapabilities = PKMerchantCapability.ThreeDS;

            request.PaymentSummaryItems = new PKPaymentSummaryItem[] { PKPaymentSummaryItem.Create("Total", (Foundation.NSDecimalNumber)254.00) };

            var applePayController = new PKPaymentAuthorizationViewController(request);
            applePayController.Delegate = this;

            PresentViewController(applePayController, true, null);
        }

        /// <summary>
        /// This is where you would send your payment to be processed - here we will
        /// simply present a confirmation screen. If your payment processor failed the
        /// payment you would return `completion(PKPaymentAuthorizationStatus.Failure)` instead. Remember to never
        /// attempt to decrypt the payment token on device.
        /// </summary>
        public void DidAuthorizePayment(PKPaymentAuthorizationViewController controller, PKPayment payment, Action<PKPaymentAuthorizationStatus> completion)
        {
            if (payment.Token.PaymentData != null)
            {
                paymentToken = payment.Token;
                completion(PKPaymentAuthorizationStatus.Success);
                PerformSegue(confirmationSegue, this);
                ShowAuthorizationAlert("Authorization Success", UIAlertActionStyle.Cancel);
            }
            else
                ShowAuthorizationAlert("Authorization Failed!", UIAlertActionStyle.Default);
        }
    }
}