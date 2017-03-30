using Tollminder.Core.ViewModels.Payments;
using MvvmCross.iOS.Views;
using UIKit;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;

namespace Tollminder.Touch.Views
{
    public partial class PaymentViewController : MvxViewController<PayViewModel>
    {
        private MvxSimpleTableViewSource notPayedTripsTableViewSource;

        public PaymentViewController() : base("PaymentViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile(@"Images/tab_background.png").Scale(View.Frame.Size));
            PayNavigationItem.Title = "Your Credit Cards";
            PayNavigationBar.TitleTextAttributes = new UIStringAttributes() { ForegroundColor = UIColor.White };
            PayNavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIImage.FromFile("Images/ic_back.png"), UIBarButtonItemStyle.Plain, null);
            PayNavigationItem.RightBarButtonItem = new UIBarButtonItem(UIImage.FromFile("Images/ProfileView/ic_card.png"), UIBarButtonItemStyle.Plain, null);

            notPayedTripsTableViewSource = new MvxSimpleTableViewSource(NotPayedTripsTableVIew, CreditCardsTableViewCell.Key, CreditCardsTableViewCell.Key);
            NotPayedTripsTableVIew.Source = notPayedTripsTableViewSource;
            NotPayedTripsTableVIew.EstimatedRowHeight = 90f;
            NotPayedTripsTableVIew.RowHeight = UITableView.AutomaticDimension;

            InitializeBindings();
        }

        private void InitializeBindings()
        {
            var bindingSet = this.CreateBindingSet<PaymentViewController, PayViewModel>();
            bindingSet.Bind(notPayedTripsTableViewSource).To(vm => vm.NotPayedTrips);
            bindingSet.Bind(PayNavigationItem.LeftBarButtonItem).To(vm => vm.BackToMainPageCommand);
            bindingSet.Bind(PayNavigationItem.RightBarButtonItem).To(vm => vm.PayCommand);
            bindingSet.Apply();
            NotPayedTripsTableVIew.ReloadData();
        }
    }
}

