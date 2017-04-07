using System;
using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Droid.Views;
using Tollminder.Core.ViewModels.Payments;
using Tollminder.Droid.TemplateSelectors;
using Tollminder.Core.Models.PaymentData;
using System.Collections.Specialized;
using Tollminder.Core.Services.Settings;
using MvvmCross.Binding.Droid.BindingContext;

namespace Tollminder.Droid.Views.PaymentsViews
{
    [Activity(Label = "PaymentView", Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class PaymentView : MvxActivity<PayViewModel>
    {
        private MvxRecyclerView recyclerView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.pay_view);

            recyclerView = FindViewById<MvxRecyclerView>(Resource.Id.pay_recycler_view);
            recyclerView.Adapter = new MvxRecyclerAdapter((IMvxAndroidBindingContext)BindingContext);
            recyclerView.ItemTemplateSelector = new TypeTemplateSelector(new Dictionary<Type, int> {
                { typeof(Trip), Resource.Layout.pay_item },
                { typeof(CreditCardsForPayViewModel), Resource.Layout.credit_cards_for_pay }
            });
            SubscribeForHeaderInsertion();
        }

        private void SubscribeForHeaderInsertion()
        {
            var notifyCollectionChanged = recyclerView.Adapter.ItemsSource as INotifyCollectionChanged;
            if (notifyCollectionChanged != null)
                notifyCollectionChanged.CollectionChanged += (sender, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add && e.NewStartingIndex == 0
                    && (e.NewItems[0] as IQueueItem)?.Priority == ItemPriority.FirstAlways)
                    recyclerView.SmoothScrollToPosition(0);
            };
        }
    }
}
