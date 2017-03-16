using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Droid.Support.V4;
using Tollminder.Core.ViewModels;
using Tollminder.Core.ViewModels.Payments;

namespace Tollminder.Droid.Views
{
    [Activity(Label = "PaytHistoryPdfView", Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class PayHistoryPdfView : MvxFragmentActivity<PayHistoryPdfViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.pay_history_pdf_view);
        }
    }
}
