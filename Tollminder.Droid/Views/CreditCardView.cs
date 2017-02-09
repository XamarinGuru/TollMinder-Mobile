using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Droid.Views;
using Tollminder.Core.ViewModels;
using Tollminder.Droid.AndroidServices;

namespace Tollminder.Droid.Views
{
    [Activity(Label = "CreditCard", Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class CreditCardView : MvxActivity<CreditCardViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.credit_card_view);

            // hide keyboard when last editText lost focus
            FieldsService.LostFocusFromField(Resource.Id.zip_code_editText);
        }
    }
}
