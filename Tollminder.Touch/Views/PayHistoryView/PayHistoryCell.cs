using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using Tollminder.Core.Models.PaymentData;
using UIKit;

namespace Tollminder.Touch.Views
{
    public partial class PayHistoryCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString(nameof(PayHistoryCell));
        public static readonly UINib Nib = UINib.FromName(nameof(PayHistoryCell), NSBundle.MainBundle);


        public PayHistoryCell(IntPtr handle) : base(handle)
        {

            // Note: this .ctor should not contain any initialization logic.
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<PayHistoryCell, PayHistory>();
                set.Bind(roadNameLabel).To(vm => vm.TollRoadName);
                set.Bind(amountUILabel).To(vm => vm.Amount);
                set.Apply();
            });
        }


        public static PayHistoryCell Create()
        {
            return (PayHistoryCell)Nib.Instantiate(null, null)[0];
        }
    }
}