using Tollminder.Core.ViewModels.Payments;
using UIKit;
using MvvmCross.Binding.BindingContext;
using Tollminder.Touch.Views.PaymentViews;
using Tollminder.Touch.Controllers;

namespace Tollminder.Touch.Views
{
    public partial class PaymentViewController : BaseViewController<PayViewModel>
    {
        private PaymentTableViewSource notPayedTripsTableViewSource;

        public PaymentViewController() : base("PaymentViewController", null) { }

        protected override void InitializeObjects()
        {
            base.InitializeObjects();

            SetBackground(@"Images/tab_background.png");
            PayNavigationItem.Title = "Your Unpaid Trips";
            PayNavigationBar.TitleTextAttributes = new UIStringAttributes() { ForegroundColor = UIColor.White };
            PayNavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIImage.FromFile(@"Images/ic_back.png"), UIBarButtonItemStyle.Plain, null);
            PayNavigationItem.RightBarButtonItem = new UIBarButtonItem(UIImage.FromFile(@"Images/ProfileView/ic_card.png"), UIBarButtonItemStyle.Plain, null);

            notPayedTripsTableViewSource = new PaymentTableViewSource(NotPayedTripsTableVIew);
            NotPayedTripsTableVIew.Source = notPayedTripsTableViewSource;
            NotPayedTripsTableVIew.EstimatedRowHeight = 90f;
            NotPayedTripsTableVIew.RowHeight = UITableView.AutomaticDimension;
        }

        protected override void InitializeBindings()
        {
            base.InitializeBindings();

            var bindingSet = this.CreateBindingSet<PaymentViewController, PayViewModel>();
            bindingSet.Bind(notPayedTripsTableViewSource).To(vm => vm.NotPayedTrips);
            bindingSet.Bind(PayNavigationItem.LeftBarButtonItem).To(vm => vm.BackToMainPageCommand);
            bindingSet.Bind(PayNavigationItem.RightBarButtonItem).To(vm => vm.PayCommand);
            bindingSet.Bind(AmountLabel).To(vm => vm.Amount);
            bindingSet.Apply();
            NotPayedTripsTableVIew.ReloadData();
        }
    }
}