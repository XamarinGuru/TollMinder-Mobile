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
        private PaymentTableViewSource cardsForPayTableViewSource;

        static CardsForPayTableViewCell()
        {
            Nib = UINib.FromName("CardsForPayTableViewCell", NSBundle.MainBundle);
        }

        protected CardsForPayTableViewCell(IntPtr handle) : base(handle)
        {
            CardsForPayNavigationItem.Title = "Please, choose one of your cards.";
            CardsForPayNavigationBar.TitleTextAttributes = new UIStringAttributes() { ForegroundColor = UIColor.White };
            CardsForPayNavigationItem.RightBarButtonItem = new UIBarButtonItem("Close", UIBarButtonItemStyle.Plain, null);

            cardsForPayTableViewSource = new PaymentTableViewSource(CardsForPayTableVIew, CardsForPayTableViewCell.Key);//, CardsForPayTableViewCell.Key);
            CardsForPayTableVIew.Source = cardsForPayTableViewSource;
            CardsForPayTableVIew.EstimatedRowHeight = 90f;
            CardsForPayTableVIew.RowHeight = UITableView.AutomaticDimension;

            this.DelayBind(() =>
                {
                    var bindingSet = this.CreateBindingSet<CardsForPayTableViewCell, CreditCardsForPayViewModel>();
                    bindingSet.Bind(CardsForPayNavigationItem.RightBarButtonItem).To(vm => vm.CloseCreditCardsForPayCommand);
                    bindingSet.Bind(cardsForPayTableViewSource).To(vm => vm.CrediCards);
                    bindingSet.Bind(cardsForPayTableViewSource).For(c => c.SelectionChangedCommand).To(vm => vm.ItemSelectedCommand);
                    bindingSet.Apply();
                });
        }
    }
}
