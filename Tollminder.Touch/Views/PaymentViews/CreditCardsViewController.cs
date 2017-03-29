using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.iOS.Views;
using Tollminder.Core.ViewModels.Payments;
using UIKit;

namespace Tollminder.Touch.Views.PaymentViews
{
    public partial class CreditCardsViewController : MvxViewController<CreditCardsViewModel>
    {
        private MvxSimpleTableViewSource creditCardsTableViewSource;

        public CreditCardsViewController() : base("CreditCardsViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile(@"Images/tab_background.png").Scale(View.Frame.Size));
            CreditCardsNavigationItem.Title = "Your Credit Cards";
            CreditCardsNavigationBar.TitleTextAttributes = new UIStringAttributes() { ForegroundColor = UIColor.White };
            CreditCardsNavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIImage.FromFile("Images/ic_back.png"), UIBarButtonItemStyle.Plain, null);
            CreditCardsNavigationItem.RightBarButtonItem = new UIBarButtonItem("+", UIBarButtonItemStyle.Plain, null);

            creditCardsTableViewSource = new MvxSimpleTableViewSource(CreditCardsTableView, CreditCardsTableViewCell.Key, CreditCardsTableViewCell.Key);
            CreditCardsTableView.Source = creditCardsTableViewSource;
            CreditCardsTableView.EstimatedRowHeight = 90f;
            CreditCardsTableView.RowHeight = UITableView.AutomaticDimension;

            InitializeBindings();
        }

        private void InitializeBindings()
        {
            var bindingSet = this.CreateBindingSet<CreditCardsViewController, CreditCardsViewModel>();
            bindingSet.Bind(creditCardsTableViewSource).To(vm => vm.CrediCards);
            bindingSet.Bind(CreditCardsNavigationItem.LeftBarButtonItem).To(vm => vm.CloseCreditCardsCommand);
            bindingSet.Bind(CreditCardsNavigationItem.RightBarButtonItem).To(vm => vm.AddCreditCardsCommand);
            bindingSet.Apply();
            CreditCardsTableView.ReloadData();
        }
    }
}

