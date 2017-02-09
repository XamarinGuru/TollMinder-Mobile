using System;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Android.Content;
using Android.Content.PM;

namespace Tollminder.Droid.AndroidServices
{
    public class FieldsService
    {
        public static void LostFocusFromField(int fieldId)
        {
            View lastField = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity.FindViewById(fieldId);
            lastField.FocusChange += (object sender, View.FocusChangeEventArgs e) =>
            {
                if (!lastField.HasFocus)
                {
                    InputMethodManager inputManager = (InputMethodManager)Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity.GetSystemService(Context.InputMethodService);

                    inputManager.HideSoftInputFromWindow(lastField.WindowToken, 0);
                }
            };
        } 
    }
}
