using Tollminder.Core.Services.Notifications;
using Tollminder.Touch.Controls;
using UIKit;

namespace Tollminder.Touch.Services
{
    public class TouchProgressDialogManager : IProgressDialogManager
    {
        UIViewController currentViewController;
        LoadingOverlay loadingOverlay;

        public TouchProgressDialogManager()
        {
            UIWindow topViewController = UIApplication.SharedApplication.KeyWindow;
            currentViewController = topViewController.RootViewController;

            var bounds = UIScreen.MainScreen.Bounds;
            // show the loading overlay on the UI thread using the correct orientation sizing
            loadingOverlay = new LoadingOverlay(bounds);
        }

        public void CloseAndShowMessage(string title, string message)
        {
            loadingOverlay.Hide();
            new UIAlertView(title, message, null, "OK", null).Show();
        }

        public void CloseProgressDialog()
        {
            loadingOverlay.Hide();
        }

        public void ShowMessage(string title, string message)
        {
            new UIAlertView(title, message, null, "OK", null).Show();
        }

        public void ShowProgressDialog(string title, string message)
        {
            currentViewController.Add(loadingOverlay);
        }
    }
}
