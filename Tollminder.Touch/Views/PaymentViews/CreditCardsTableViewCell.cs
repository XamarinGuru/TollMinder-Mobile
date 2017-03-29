using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;
using Tollminder.Core.ViewModels.Payments;

namespace Tollminder.Touch.Views.PaymentViews
{
    public partial class CreditCardsTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("CreditCardsTableViewCell");
        public static readonly UINib Nib;

        static CreditCardsTableViewCell()
        {
            Nib = UINib.FromName("CreditCardsTableViewCell", NSBundle.MainBundle);
        }

        protected CreditCardsTableViewCell(IntPtr handle) : base(handle)
        {
            this.DelayBind(() =>
            {
                var bindingSet = this.CreateBindingSet<CreditCardsTableViewCell, CreditCardAuthorizeDotNetViewModel>();
                bindingSet.Bind(CreditCardLastDigits).To(vm => vm.CreditCard.PaymentProfile.CardNumber);
                bindingSet.Bind(RemoveCrediCard).To(vm => vm.RemoveCreditCardCommand);
                bindingSet.Apply();
            });
        }
    }
}
