using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;
using Tollminder.Core.ViewModels.Payments;

namespace Tollminder.Touch.Views.PaymentViews
{
    public partial class CardsForPayTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("CardsForPayTableViewCell");
        public static readonly UINib Nib;
        private MvxSimpleTableViewSource cardsForPayTableViewSource;

        static CardsForPayTableViewCell()
        {
            Nib = UINib.FromName("CardsForPayTableViewCell", NSBundle.MainBundle);
        }

        protected CardsForPayTableViewCell(IntPtr handle) : base(handle)
        {
            this.DelayBind(() =>
                {
                    cardsForPayTableViewSource = new MvxSimpleTableViewSource(CardsForPayTableVIew, CreditCardsTableViewCell.Key, CreditCardsTableViewCell.Key);
                    CardsForPayTableVIew.Source = cardsForPayTableViewSource;
                    CardsForPayTableVIew.EstimatedRowHeight = 90f;
                    CardsForPayTableVIew.RowHeight = UITableView.AutomaticDimension;

                    var bindingSet = this.CreateBindingSet<CardsForPayTableViewCell, CreditCardsForPayViewModel>();
                    bindingSet.Bind(CardsForPayNavigationButtonClose).To(vm => vm.CloseCreditCardsForPayCommand);
                    bindingSet.Bind(cardsForPayTableViewSource).To(vm => vm.CrediCards);
                    bindingSet.Bind(cardsForPayTableViewSource).For(c => c.SelectionChangedCommand).To(vm => vm.ItemSelectedCommand);
                    bindingSet.Apply();
                });
        }
    }
}
