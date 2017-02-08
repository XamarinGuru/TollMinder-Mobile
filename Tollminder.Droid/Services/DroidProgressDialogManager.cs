using System;
using Android.App;
using Android.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Plugin.CurrentActivity;
using Tollminder.Core.Services;

namespace Tollminder.Droid.Services
{
    public class DroidProgressDialogManager : IProgressDialogManager
    {
        ProgressDialog progressDialog;

        public DroidProgressDialogManager()
        {
            progressDialog = new ProgressDialog(CrossCurrentActivity.Current.Activity);
        }

        public void CloseAndShowMessage(string title, string message)
        {
            if (progressDialog != null)
            {
                if (progressDialog.IsShowing)
                    progressDialog.Dismiss();

                NotificationView(title, message).Show();
            }
        }

        private AlertDialog NotificationView(string title, string message)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(CrossCurrentActivity.Current.Activity);
            AlertDialog dialog = null;

            builder.SetTitle(title);
            builder.SetMessage(message);
            builder.SetCancelable(true);
            builder.SetPositiveButton("Ok", delegate
            {
                dialog.Dismiss();
            });
            dialog = builder.Create();
            return dialog;
        }

        public void ShowSmsConfirmation()
        {
            View view = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity.LayoutInflater.Inflate(Resource.Layout.calendar_fragment, null);
            AlertDialog dialog = NotificationView("Please input code from SMS", null);
            dialog.SetView(view);
            dialog.Show();


        }

        public void CloseProgressDialog()
        {
            if (progressDialog != null)
            {
                if (progressDialog.IsShowing)
                    progressDialog.Dismiss();
            }
        }

        public void ShowProgressDialog(string title, string message)
        {
            if (progressDialog != null)
            {
                if (!progressDialog.IsShowing)
                {
                    progressDialog.SetTitle(title);
                    progressDialog.SetMessage(message);
                    progressDialog.Show();
                }
            }
        }
    }
}
