using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using Tollminder.Core.ViewModels.Payments;
using UIKit;
using Tollminder.Touch.Controllers;

namespace Tollminder.Touch.Views.PaymentViews
{
    public partial class CreditCardsViewController : BaseViewController<CreditCardsViewModel>
    {
        private MvxSimpleTableViewSource creditCardsTableViewSource;

        public CreditCardsViewController() : base("CreditCardsViewController", null)
        {
        }

        protected override void InitializeObjects()
        {
            base.InitializeObjects();

            SetBackground(@"Images/tab_background.png");
            CreditCardsNavigationItem.Title = "Your Credit Cards";
            CreditCardsNavigationBar.TitleTextAttributes = new UIStringAttributes() { ForegroundColor = UIColor.White };
            CreditCardsNavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIImage.FromFile("Images/ic_back.png"), UIBarButtonItemStyle.Plain, null);
            CreditCardsNavigationItem.RightBarButtonItem = new UIBarButtonItem("+", UIBarButtonItemStyle.Plain, null);

            creditCardsTableViewSource = new MvxSimpleTableViewSource(CreditCardsTableView, CreditCardsTableViewCell.Key, CreditCardsTableViewCell.Key);
            CreditCardsTableView.Source = creditCardsTableViewSource;
            CreditCardsTableView.EstimatedRowHeight = 90f;
            CreditCardsTableView.RowHeight = UITableView.AutomaticDimension;
        }

        protected override void InitializeBindings()
        {
            base.InitializeBindings();

            var bindingSet = this.CreateBindingSet<CreditCardsViewController, CreditCardsViewModel>();
            bindingSet.Bind(creditCardsTableViewSource).To(vm => vm.CrediCards);
            bindingSet.Bind(CreditCardsNavigationItem.LeftBarButtonItem).To(vm => vm.CloseCreditCardsCommand);
            bindingSet.Bind(CreditCardsNavigationItem.RightBarButtonItem).To(vm => vm.AddCreditCardsCommand);
            bindingSet.Apply();
            CreditCardsTableView.ReloadData();
        }
    }
}

