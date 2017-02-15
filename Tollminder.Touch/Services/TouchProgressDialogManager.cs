using System;
using System.Threading.Tasks;
using Tollminder.Core.Services;
using Tollminder.Core.ViewModels;
using Tollminder.Touch.Controls;
using Tollminder.Touch.Helpers.MvxAlertActionHelpers;
using UIKit;

namespace Tollminder.Touch.Services
{
    public class TouchProgressDialogManager : IProgressDialogManager
    {
        UIViewController currentViewController;
        LoadingOverlay loadingOverlay;
        UITextField smsInputTextField;
        UIAlertController smsConfirmationAlertController;
        TaskCompletionSource<string> smsConfirmationTask;
        MvxAlertAction smsValidateButton;
        RegistrationViewModel registrationViewModel;

        public TouchProgressDialogManager()
        {
            UIWindow topViewController = UIApplication.SharedApplication.KeyWindow;
            currentViewController = topViewController.RootViewController;
            registrationViewModel = new RegistrationViewModel();
            smsValidateButton = new MvxAlertAction("Validate", UIAlertActionStyle.Default);
            var binding = new MvxAlertActionBinding(smsValidateButton, registrationViewModel.ValidateCommand);

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

        public void SmsConfirmation(string title, string message)
        {
            smsConfirmationAlertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);

            smsConfirmationAlertController.AddTextField((textField) =>
            {
                smsInputTextField = textField;
                smsInputTextField.Placeholder = "XXXX";
                smsInputTextField.SecureTextEntry = true;
            });
            // Add validate button
            smsConfirmationAlertController.AddAction(smsValidateButton.AlertAction);

            // Display the alert
            currentViewController.PresentViewController(smsConfirmationAlertController, true, null);
        }
    }
}
