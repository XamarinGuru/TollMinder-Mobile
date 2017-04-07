// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Tollminder.Touch.Views.UserProfile
{
	[Register ("RegistrationViewController")]
	partial class RegistrationViewController
	{
		[Outlet]
		UIKit.UITextField ConfirmPasswordTextField { get; set; }

		[Outlet]
		UIKit.UITextField EmailTextField { get; set; }

		[Outlet]
		UIKit.UITextField FirstNameTextField { get; set; }

		[Outlet]
		UIKit.UITextField LastNameTextField { get; set; }

		[Outlet]
		UIKit.UIView NotSocialRegistrationView { get; set; }

		[Outlet]
		UIKit.UITextField PasswordTextField { get; set; }

		[Outlet]
		UIKit.UITextField PhoneNumberTextField { get; set; }

		[Outlet]
		UIKit.UINavigationBar RegistrationNavigationBar { get; set; }

		[Outlet]
		UIKit.UINavigationItem RegistrationNavigationItem { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ConfirmPasswordTextField != null) {
				ConfirmPasswordTextField.Dispose ();
				ConfirmPasswordTextField = null;
			}

			if (EmailTextField != null) {
				EmailTextField.Dispose ();
				EmailTextField = null;
			}

			if (FirstNameTextField != null) {
				FirstNameTextField.Dispose ();
				FirstNameTextField = null;
			}

			if (LastNameTextField != null) {
				LastNameTextField.Dispose ();
				LastNameTextField = null;
			}

			if (NotSocialRegistrationView != null) {
				NotSocialRegistrationView.Dispose ();
				NotSocialRegistrationView = null;
			}

			if (PasswordTextField != null) {
				PasswordTextField.Dispose ();
				PasswordTextField = null;
			}

			if (PhoneNumberTextField != null) {
				PhoneNumberTextField.Dispose ();
				PhoneNumberTextField = null;
			}

			if (RegistrationNavigationBar != null) {
				RegistrationNavigationBar.Dispose ();
				RegistrationNavigationBar = null;
			}

			if (RegistrationNavigationItem != null) {
				RegistrationNavigationItem.Dispose ();
				RegistrationNavigationItem = null;
			}
		}
	}
}
