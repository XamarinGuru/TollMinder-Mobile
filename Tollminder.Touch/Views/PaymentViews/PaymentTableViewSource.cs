using System;
using System.Collections.Specialized;
using Foundation;
using MvvmCross.Binding.ExtensionMethods;
using MvvmCross.Binding.iOS.Views;
using UIKit;
using Tollminder.Core.ViewModels.Payments;

namespace Tollminder.Touch.Views.PaymentViews
{
    public class PaymentTableViewSource : MvxTableViewSource
    {
        INotifyCollectionChanged notifyCollectionChanged;

        public PaymentTableViewSource(UITableView tableView)
            : base(tableView)
        {
            tableView.RegisterNibForCellReuse(NotPayedTripsTableViewCell.Nib, NotPayedTripsTableViewCell.Key);
            tableView.RegisterNibForCellReuse(CardsForPayTableViewCell.Nib, CardsForPayTableViewCell.Key);
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            if (item is CreditCardsForPayViewModel)
                return tableView.DequeueReusableCell(CardsForPayTableViewCell.Key, indexPath);

            return tableView.DequeueReusableCell(NotPayedTripsTableViewCell.Key, indexPath) as NotPayedTripsTableViewCell;
        }

        public override System.Collections.IEnumerable ItemsSource
        {
            get
            {
                return base.ItemsSource;
            }
            set
            {
                if (notifyCollectionChanged != null)
                    notifyCollectionChanged.CollectionChanged -= ItemsSourceCollectionChaged;

                base.ItemsSource = value;

                notifyCollectionChanged = ItemsSource as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                    notifyCollectionChanged.CollectionChanged += ItemsSourceCollectionChaged;
            }
        }

        private void ItemsSourceCollectionChaged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewStartingIndex == 0 && ItemsSource.Count() > 1)
            {
                TableView.ScrollToRow(NSIndexPath.FromRowSection(0, 0), UITableViewScrollPosition.Top, true);
            }
        }

        public override nfloat EstimatedHeight(UITableView tableView, NSIndexPath indexPath)
            => 450f;
    }
}
