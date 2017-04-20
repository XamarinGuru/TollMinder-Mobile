using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;
using Tollminder.Core.Models.PaymentData;

namespace Tollminder.Touch.Views.PaymentViews
{
    public partial class NotPayedTripsTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("NotPayedTripsTableViewCell");
        public static readonly UINib Nib;

        static NotPayedTripsTableViewCell()
        {
            Nib = UINib.FromName("NotPayedTripsTableViewCell", NSBundle.MainBundle);
        }

        protected NotPayedTripsTableViewCell(IntPtr handle) : base(handle)
        {
            this.DelayBind(() =>
                {
                    var bindingSet = this.CreateBindingSet<NotPayedTripsTableViewCell, Trip>();
                    bindingSet.Bind(TollRoadLabel).To(vm => vm.TollRoadName);
                    bindingSet.Bind(AmountLabel).To(vm => vm.Cost);
                    bindingSet.Bind(BillingDateLabel).To(vm => vm.PaymentDate);
                    bindingSet.Bind(TransactionIdLabel).To(vm => vm.Transaction);
                    bindingSet.Apply();
                });
        }
    }
}
