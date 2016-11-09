using System;
using Tollminder.Core.ViewModels;
using Tollminder.Touch.Controllers;
using Tollminder.Touch.Controls;
using UIKit;

namespace Tollminder.Touch.Views
{
    public class LoginView : BaseViewController<LoginViewModel>
    {
        UILabel DontHaveAnAccountLabel { get; set; }
        UIButton ForgotPasswordButton { get; set; }
        UIButton GetStartedButton { get; set; }
        UIButton LoginButton { get; set; }
        UILabel LogInLabel { get; set; }
        TextFieldValidatorView LoginTxt { get; set; }
        TextFieldValidatorView PasswordTxt { get; set; }

        public LoginView()
        {
        }
    }
}
