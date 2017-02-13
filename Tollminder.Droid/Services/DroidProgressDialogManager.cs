using System;
using Android.App;
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

        public void ShowMessage(string title, string message)
        {
            CloseAndShowMessage(title, message);
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
            if (!CrossCurrentActivity.Current.Activity.IsFinishing)
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
}
