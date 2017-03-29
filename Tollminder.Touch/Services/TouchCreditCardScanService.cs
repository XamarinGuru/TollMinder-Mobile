using System;
using System.Threading.Tasks;
using Card.IO;
using Tollminder.Core.Services.ScanCrediCard;
using MvvmCross.Platform;
using MvvmCross.Platform.iOS.Views;
using MvvmCross.Platform.Platform;
using UIKit;
using Tollminder.Core.Models.PaymentData;

namespace Tollminder.Touch.Services
{
    public class TouchCreditCardScanService : ICreditCardScanService
    {
        internal static void Initialize()
        {
            Mvx.RegisterSingleton<ICreditCardScanService>(new TouchCreditCardScanService());
            Mvx.Trace(MvxTraceLevel.Diagnostic, "ICreditCardScanService registered");
        }

        public TouchCreditCardScanService() { }

        private IMvxIosModalHost _modalHost;
        private Action<AddCreditCard> _callback;
        private CardIOPaymentViewController _paymentViewController;

        public void ScanCardInfo(Action<AddCreditCard> callback, CreditCardScanOptions creditCardScanOptions = null)
        {
            if (creditCardScanOptions == null)
            {
                creditCardScanOptions = new CreditCardScanOptions();
            }

            _callback = callback;
            _modalHost = Mvx.Resolve<IMvxIosModalHost>();
            _paymentViewController = new CardIOPaymentViewController()
            {
                GuideColor = new UIColor(255, 255, 255, 1.0f)//ColorFromHex(creditCardScanOptions.GuideColor)
            };

            _modalHost.PresentModalViewController(_paymentViewController, true);
        }

        public Task<AddCreditCard> ScanCardInfoAsync(CreditCardScanOptions creditCardScanOptions = null)
        {
            var tcs = new TaskCompletionSource<AddCreditCard>();
            ScanCardInfo(tcs.SetResult, creditCardScanOptions);
            return tcs.Task;
        }

        public void UserDidCancelPaymentViewController(CardIOPaymentViewController paymentViewController)
        {
            _callback?.Invoke(AddCreditCard.Empty);
            _paymentViewController.DismissViewController(true, null);
            _modalHost.NativeModalViewControllerDisappearedOnItsOwn();
        }

        public void UserDidProvideCreditCardInfo(CreditCardInfo cardInfo, CardIOPaymentViewController paymentViewController)
        {
            var creditCard = cardInfo == null ? AddCreditCard.Empty : new AddCreditCard
            {
                //CardNumber = cardInfo.CardNumber,
                //Cvv = cardInfo.Cvv,
                //ExpirationMonth = cardInfo.ExpiryMonth,
                //ExpirationYear = cardInfo.ExpiryYear
            };

            _callback?.Invoke(creditCard);
            _paymentViewController.DismissViewController(true, null);
            _modalHost.NativeModalViewControllerDisappearedOnItsOwn();
        }

        private UIColor ColorFromHex(string hex)
        {
            var rgbValue = Convert.ToInt32(hex.Substring(1, hex.Length), 16);
            var red = (nfloat)(((rgbValue & 0xFF0000) >> 16) / 255.0);
            var green = (nfloat)(((rgbValue & 0xFF00) >> 8) / 255.0);
            var blue = (nfloat)((rgbValue & 0xFF) / 255.0);

            return new UIColor(red, green, blue, 1.0f);
        }
    }
}