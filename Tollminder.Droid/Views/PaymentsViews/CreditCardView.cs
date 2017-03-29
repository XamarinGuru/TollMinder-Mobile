using System.Collections.Generic;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Droid.Views;
using Tollminder.Core.ViewModels.Payments;
using Tollminder.Droid.TemplateSelectors;
using Android.App;
using Android.Content.PM;
using Android.OS;
using System;

namespace Tollminder.Droid.Views
{
    [Activity(Label = "CreditCard", Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class CreditCardView : MvxActivity<CreditCardsViewModel>
    {
        private MvxRecyclerView recyclerView;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.credit_cards_list_view);

            recyclerView = FindViewById<MvxRecyclerView>(Resource.Id.credit_cards_list_view_recycler_view);
            recyclerView.ItemTemplateSelector = new TypeTemplateSelector(new Dictionary<Type, int> {
                { typeof(CreditCardAuthorizeDotNetViewModel), Resource.Layout.credit_card_item }
            });
        }
    }
}
