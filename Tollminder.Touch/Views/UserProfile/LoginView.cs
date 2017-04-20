using System;
using System.Collections.Generic;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Facebook.LoginKit;
using Google.SignIn;
using MvvmCross.Binding.BindingContext;
using Tollminder.Core;
using Tollminder.Core.Converters;
using Tollminder.Core.ViewModels.UserProfile;
using Tollminder.Touch.Controllers;
using Tollminder.Touch.Controls;
using Tollminder.Touch.Extensions;
using Tollminder.Touch.Helpers;
using Tollminder.Touch.Interfaces;
using UIKit;

namespace Tollminder.Touch.Views
{
    public class LoginView : BaseViewController<LoginViewModel>, ISignInUIDelegate, ICleanBackStack
    {
        //UILabel DontHaveAnAccountLabel;
        //UIButton ForgotPasswordButton;
        //UIButton GetStartedButton;
        //UILabel LogInLabel;
        TextFieldValidationWithImage _loginTextField;
        TextFieldValidationWithImage _passwordTextField;

        UIButton _facebookLoginButton;
        ProfileButton _googlePlusLoginButton;
        UIButton _loginButton;
        UIButton forgotPasswordButton;
        UIButton registrationButton;

        UILabel socialNetworkLabel;
        UILabel registrationLabel;

        List<string> readPermissions = new List<string> { "public_profile" };

        public LoginView()
        {
        }

        public LoginView(IntPtr handle) : base(handle)
        {
        }

        public LoginView(string nibName, Foundation.NSBundle bundle) : base(nibName, bundle)
        {
        }


        protected override void InitializeObjects()
        {
            base.InitializeObjects();

            var topView = new UIView();
            var centerView = new UIView();
            var bottomView = new UIView();
            var socialNetworksView = new UIView();
            var applicationLogo = new UIImageView(UIImage.FromBundle(@"Images/logo.png"));

            // Hide navigation bar
            NavigationController.SetNavigationBarHidden(true, false);

            View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile(@"Images/main_background.png").Scale(View.Frame.Size));
            applicationLogo.Frame = new CGRect(10, 10, applicationLogo.Image.CGImage.Width, applicationLogo.Image.CGImage.Height);
            topView.AddIfNotNull(applicationLogo);
            topView.AddConstraints(
                applicationLogo.WithRelativeWidth(topView, 0.5f),
                applicationLogo.WithRelativeHeight(topView, 0.25f),
                applicationLogo.WithSameCenterX(topView),
                applicationLogo.WithSameCenterY(topView)
            );

            _loginTextField = TextFieldInitializer("Login");
            _loginTextField.TextFieldWithValidator.TextField.KeyboardType = UIKeyboardType.EmailAddress;
            socialNetworkLabel = LabelInitializer("Login With:", UIColor.LightGray);
            registrationLabel = LabelInitializer("Don't have an account?", UIColor.LightGray);

            _passwordTextField = TextFieldInitializer("Password");

            _loginButton = ButtonInitializer("Login", UIControlState.Normal, Theme.BlueDark.ToUIColor(),
                              UIColor.White, UIControlState.Normal, null, UIControlState.Disabled);

            _googlePlusLoginButton = CreateGoogleButton();

            var sloginView = new LoginButton(new CGRect(51, 0, 218, 46))
            {
                LoginBehavior = LoginBehavior.Native,
                ReadPermissions = readPermissions.ToArray()
            };

            _facebookLoginButton = ButtonInitializer(sloginView.CurrentTitle, UIControlState.Normal, null, sloginView.CurrentTitleColor,
                                                     UIControlState.Normal, sloginView.CurrentImage, UIControlState.Normal);
            _facebookLoginButton.SetBackgroundImage(sloginView.CurrentBackgroundImage, UIControlState.Normal);
            _facebookLoginButton.Font = UIFont.FromName("Helvetica", 14f);
            _facebookLoginButton.ImageEdgeInsets = new UIEdgeInsets(0, 0, 0, 40);

            forgotPasswordButton = ButtonInitializer("Forgot your password?", UIControlState.Normal,
                              null, UIColor.LightGray, UIControlState.Normal, null, UIControlState.Normal);
            forgotPasswordButton.TitleLabel.Font = UIFont.FromName("Helvetica", 12f);

            registrationButton = ButtonInitializer("Get Started!", UIControlState.Normal,
                              null, UIColor.Cyan, UIControlState.Normal, null, UIControlState.Normal);
            registrationButton.TitleLabel.Font = UIFont.FromName("Helvetica", 12f);

            socialNetworksView.AddIfNotNull(socialNetworkLabel, _facebookLoginButton, _googlePlusLoginButton);
            socialNetworksView.AddConstraints(
                socialNetworkLabel.AtTopOf(socialNetworksView),
                socialNetworkLabel.AtLeftOf(socialNetworksView),
                socialNetworkLabel.WithSameWidth(socialNetworksView),
                socialNetworkLabel.WithRelativeHeight(socialNetworksView, 0.2f),
                socialNetworkLabel.AtBottomOf(socialNetworksView, 10),

                _facebookLoginButton.Below(socialNetworkLabel),
                _facebookLoginButton.AtLeftOf(socialNetworksView),
                _facebookLoginButton.WithRelativeWidth(socialNetworksView, 0.48f),
                _facebookLoginButton.WithRelativeHeight(socialNetworksView, 0.4f),

                _googlePlusLoginButton.Below(socialNetworkLabel),
                _googlePlusLoginButton.WithRelativeWidth(socialNetworksView, 0.48f),
                _googlePlusLoginButton.WithRelativeHeight(socialNetworksView, 0.4f),
                _googlePlusLoginButton.AtRightOf(socialNetworksView)
            );

            // Central block with text fields and login buttons
            centerView.Layer.CornerRadius = 10;
            centerView.AddIfNotNull(_loginTextField, _passwordTextField, forgotPasswordButton, _loginButton, socialNetworksView,
                                   registrationLabel, registrationButton);
            centerView.BackgroundColor = UIColor.White;
            centerView.AddConstraints(
                _loginTextField.AtTopOf(centerView, 10),
                _loginTextField.WithSameCenterX(centerView),
                _loginTextField.WithSameWidth(centerView),
                _loginTextField.WithRelativeHeight(centerView, 0.2f),

                _passwordTextField.Below(_loginTextField),
                _passwordTextField.WithSameCenterX(centerView),
                _passwordTextField.WithSameWidth(centerView),
                _passwordTextField.WithRelativeHeight(centerView, 0.2f),

                forgotPasswordButton.Below(_passwordTextField, -10),
                forgotPasswordButton.AtLeftOf(centerView, 20),
                forgotPasswordButton.WithRelativeWidth(centerView, 0.5f),
                forgotPasswordButton.WithRelativeHeight(centerView, 0.07f),

                _loginButton.Below(forgotPasswordButton, 10),
                _loginButton.AtLeftOf(centerView, 20),
                _loginButton.AtRightOf(centerView, 20),
                // make a fat login button
                _loginButton.WithRelativeHeight(centerView, 0.15f),

                socialNetworksView.Below(_loginButton, 20),
                socialNetworksView.AtLeftOf(centerView, 20),
                socialNetworksView.AtRightOf(centerView, 20),
                socialNetworksView.WithRelativeHeight(centerView, 0.2f),

                registrationLabel.Below(socialNetworksView),
                registrationLabel.AtLeftOf(centerView, 25),
                registrationLabel.WithRelativeWidth(centerView, 0.5f),
                registrationLabel.WithRelativeHeight(centerView, 0.07f),

                registrationButton.Below(socialNetworksView),
                registrationButton.AtRightOf(centerView, 10),
                registrationButton.WithRelativeWidth(centerView, 0.5f),
                registrationButton.WithRelativeHeight(centerView, 0.07f)
            );


            // Main view
            View.AddIfNotNull(topView, centerView, bottomView);
            View.AddConstraints(
                topView.AtTopOf(View, 10),
                topView.AtLeftOf(View),
                topView.AtRightOf(View),
                topView.WithRelativeHeight(View, 0.15f),

                centerView.AtLeftOf(View, 30),
                centerView.AtRightOf(View, 30),
                centerView.Below(topView),
                centerView.AtBottomOf(View, 70),

                bottomView.AtBottomOf(View),
                bottomView.AtLeftOf(View),
                bottomView.AtRightOf(View),
                bottomView.Below(centerView),
                bottomView.WithRelativeHeight(View, 0f)
            );

            SignIn.SharedInstance.UIDelegate = this;
            EnableNextKeyForTextFields(_loginTextField.TextFieldWithValidator.TextField, _passwordTextField.TextFieldWithValidator.TextField);
        }

        private TextFieldValidationWithImage TextFieldInitializer(string placeholder)
        {
            TextFieldValidationWithImage textField = new TextFieldValidationWithImage();
            textField.TextFieldWithValidator.TextField.Placeholder = placeholder;
            textField.BackgroundColor = UIColor.White;
            textField.TextFieldWithValidator.TextField.TextColor = UIColor.LightGray;
            textField.TextFieldWithValidator.SeparatorView.BackgroundColor = UIColor.Clear;
            textField.Layer.CornerRadius = 10;
            textField.TextFieldWithValidator.TopLabelColor = UIColor.Black;
            textField.TextFieldWithValidator.TextField.KeyboardType = UIKeyboardType.Default;

            return textField;
        }

        private ProfileButton CreateGoogleButton()
        {
            var socialNetworkButton = ProfileButtonManager.ButtonInitiaziler(EnvironmentInfo.GetGoogleButtonDistanceBetweenTextAndIcon, 0.1f, 0.6f, 0.3f, 0.5f, "Sign in",
                                                                            UIImage.FromFile(@"Images/LoginView/ic_google.png"), UIColor.DarkGray, UIColor.White);
            socialNetworkButton.Layer.CornerRadius = 5;
            socialNetworkButton.Layer.ShadowColor = UIColor.Black.CGColor;
            socialNetworkButton.Layer.ShadowOpacity = 0.1f;
            socialNetworkButton.Layer.ShadowRadius = 1;
            socialNetworkButton.Layer.ShadowOffset = new CGSize(1, 1);
            return socialNetworkButton;
        }

        private UIButton ButtonInitializer(string title, UIControlState titleState, UIColor backgroundColor,
                                           UIColor titleColor, UIControlState colorTitleState, UIImage image, UIControlState imageState)
        {
            UIButton button = new UIButton();
            button.SetTitle(title, titleState);
            if (image != null)
                button.SetImage(image, imageState);
            button.BackgroundColor = backgroundColor;
            button.SetTitleColor(titleColor, colorTitleState);
            button.ClipsToBounds = false;
            button.Layer.CornerRadius = 10;
            button.Layer.ShadowColor = UIColor.Black.CGColor;
            button.Layer.ShadowOpacity = 0.1f;
            button.Layer.ShadowRadius = 1;
            button.Layer.ShadowOffset = new CGSize(1, 1);
            return button;
        }

        private UILabel LabelInitializer(string text, UIColor textColor)
        {
            UILabel label = new UILabel();
            label.Text = text;
            label.TextColor = textColor;
            label.Font = UIFont.FromName("Helvetica", 12f);
            return label;
        }

        protected override void InitializeBindings()
        {
            base.InitializeBindings();

            var set = this.CreateBindingSet<LoginView, LoginViewModel>();
            set.Bind(_loginTextField.TextFieldWithValidator.TextField).To(vm => vm.LoginString);
            set.Bind(_loginTextField.TextFieldWithValidator).For(v => v.ErrorMessageString).To(vm => vm.Errors["Login"]);
            set.Bind(_passwordTextField.TextFieldWithValidator.TextField).To(vm => vm.PasswordString);
            set.Bind(_passwordTextField.TextFieldWithValidator).For(v => v.ErrorMessageString).To(vm => vm.Errors["Password"]);

            set.Bind(forgotPasswordButton).To(vm => vm.ForgotPasswordCommand);
            set.Bind(_loginButton).To(vm => vm.EmailLoginCommand);
            set.Bind(_loginButton).For(x => x.Enabled).To(vm => vm.IsBusy).WithConversion(new BoolInverseConverter());
            set.Bind(_facebookLoginButton).To(vm => vm.FacebookLoginCommand);
            set.Bind(_facebookLoginButton).For(x => x.Enabled).To(vm => vm.IsBusy).WithConversion(new BoolInverseConverter());
            set.Bind(_googlePlusLoginButton).To(vm => vm.GPlusLoginCommand);
            set.Bind(_googlePlusLoginButton).For(x => x.Enabled).To(vm => vm.IsBusy).WithConversion(new BoolInverseConverter());
            set.Bind(registrationButton).To(vm => vm.RegistrationCommand);
            set.Apply();
        }
    }
}
