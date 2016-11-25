using System;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Google.SignIn;
using MvvmCross.Binding.BindingContext;
using Tollminder.Core;
using Tollminder.Core.Converters;
using Tollminder.Core.ViewModels;
using Tollminder.Touch.Controllers;
using Tollminder.Touch.Controls;
using Tollminder.Touch.Extensions;
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
        UIButton _googlePlusLoginButton;
        UIButton _loginButton;

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
            var imageView = new UIImageView(UIImage.FromBundle(@"Images/home_logo.png"));

            // Hide navigation bar
            NavigationController.SetNavigationBarHidden(true, false);

            View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile(@"Images/home_background.png").
                                           ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal).
                                           ImageWithAlignmentRectInsets(UIEdgeInsets.Zero));
            imageView.Frame = new CGRect(10, 10, imageView.Image.CGImage.Width, imageView.Image.CGImage.Height);
            topView.AddIfNotNull(imageView);
            topView.AddConstraints(
                imageView.WithSameCenterX(topView),
                imageView.WithSameCenterY(topView)
            );

            _loginTextField = new TextFieldValidationWithImage();
            _loginTextField.TextFieldWithValidator.TextField.Placeholder = "Login";
            _loginTextField.TextFieldWithValidator.TextField.KeyboardType = UIKeyboardType.EmailAddress;
                
            _passwordTextField = new TextFieldValidationWithImage();
            _passwordTextField.TextFieldWithValidator.TextField.Placeholder = "Password";
            _passwordTextField.TextFieldWithValidator.TextField.KeyboardType = UIKeyboardType.Default;

            _loginButton = new UIButton();
            ButtonInitializer(_loginButton, "Login", UIControlState.Normal, Theme.BlueDark.ToUIColor(), 
                              UIColor.White, UIControlState.Normal, null, UIControlState.Disabled);

            _googlePlusLoginButton = new UIButton();
            ButtonInitializer(_googlePlusLoginButton, null, UIControlState.Disabled, null, null, 
                              UIControlState.Disabled, @"Images/google-button.png", UIControlState.Normal);

            _facebookLoginButton = new UIButton();
            ButtonInitializer(_facebookLoginButton, null, UIControlState.Disabled, null, null,
                              UIControlState.Disabled,@"Images/facebook-button.png", UIControlState.Normal);

            socialNetworksView.AddIfNotNull(_facebookLoginButton, _googlePlusLoginButton);
            socialNetworksView.AddConstraints(

                _facebookLoginButton.AtTopOf(socialNetworksView, 8),
                _facebookLoginButton.AtLeftOf(socialNetworksView),
                _facebookLoginButton.AtRightOf(_googlePlusLoginButton, 112),

                _googlePlusLoginButton.AtTopOf(socialNetworksView, 8),
                _googlePlusLoginButton.AtLeftOf(_facebookLoginButton, 112),
                _googlePlusLoginButton.AtRightOf(socialNetworksView)
            );

            // Central block with text fields and login buttons
            centerView.Layer.CornerRadius = 5;
            centerView.AddIfNotNull(_loginTextField, _passwordTextField, _loginButton, socialNetworksView);
            centerView.BackgroundColor = UIColor.White;
            centerView.AddConstraints(
                _loginTextField.AtTopOf(centerView, 8),
                _loginTextField.AtLeftOf(centerView, -15),
                _loginTextField.AtRightOf(centerView, 20),

                _passwordTextField.Below(_loginTextField, 8),
                _passwordTextField.AtLeftOf(centerView, -15),
                _passwordTextField.AtRightOf(centerView, 20),

                _loginButton.Below(_passwordTextField, 10),
                _loginButton.AtLeftOf(centerView, 20),
                _loginButton.AtRightOf(centerView, 20),
                // make a fat button
                _loginButton.Height().EqualTo(50),

                socialNetworksView.Below(_loginButton, 20),
                socialNetworksView.AtLeftOf(centerView, 20),
                socialNetworksView.AtRightOf(centerView, 20),
                socialNetworksView.AtBottomOf(centerView, 10)
            );

            // Main view
            View.AddIfNotNull(topView, centerView, bottomView);
            View.AddConstraints(
                topView.AtTopOf(View),
                topView.AtLeftOf(View),
                topView.AtRightOf(View),
                topView.WithRelativeHeight(View, 0.3f),

                centerView.AtLeftOf(View, 30),
                centerView.AtRightOf(View, 30),
                centerView.Below(topView),
                centerView.AtBottomOf(View, 50),

                bottomView.AtBottomOf(View),
                bottomView.AtLeftOf(View),
                bottomView.AtRightOf(View),
                bottomView.Below(centerView),
                bottomView.WithRelativeHeight(View, 0f)
            );

            SignIn.SharedInstance.UIDelegate = this;
            EnableNextKeyForTextFields(_loginTextField.TextFieldWithValidator.TextField, _passwordTextField.TextFieldWithValidator.TextField);
        }

        private UIButton ButtonInitializer(UIButton button, string title, UIControlState titleState, UIColor backgroundColor, 
                                           UIColor titleColor, UIControlState colorTitleState, string imagePath, UIControlState imageState)
        {
            button.SetTitle(title, titleState);
            if(imagePath != null)
                button.SetImage(UIImage.FromFile(imagePath), imageState);
            button.BackgroundColor = backgroundColor;
            button.SetTitleColor(titleColor, colorTitleState);
            button.ClipsToBounds = false;
            button.Layer.CornerRadius = 5;
            button.Layer.ShadowColor = UIColor.Black.CGColor;
            button.Layer.ShadowOpacity = 0.1f;
            button.Layer.ShadowRadius = 1;
            button.Layer.ShadowOffset = new CGSize(1, 1);
            return button;
        }

        protected override void InitializeBindings()
        {
            base.InitializeBindings();

            var set = this.CreateBindingSet<LoginView, LoginViewModel>();
            set.Bind(_loginTextField.TextFieldWithValidator.TextField).To(vm => vm.LoginString);
            set.Bind(_loginTextField.TextFieldWithValidator).For(v => v.ErrorMessageString).To(vm => vm.Errors["Login"]);
            set.Bind(_passwordTextField.TextFieldWithValidator.TextField).To(vm => vm.PasswordString);
            set.Bind(_passwordTextField.TextFieldWithValidator).For(v => v.ErrorMessageString).To(vm => vm.Errors["Password"]);
            set.Bind(_loginButton).To(vm => vm.EmailLoginCommand);
            set.Bind(_loginButton).For(x => x.Enabled).To(vm => vm.IsBusy).WithConversion(new BoolInverseConverter());
            set.Bind(_facebookLoginButton).To(vm => vm.FacebookLoginCommand);
            set.Bind(_facebookLoginButton).For(x => x.Enabled).To(vm => vm.IsBusy).WithConversion(new BoolInverseConverter());
            set.Bind(_googlePlusLoginButton).To(vm => vm.GPlusLoginCommand);
            set.Bind(_googlePlusLoginButton).For(x => x.Enabled).To(vm => vm.IsBusy).WithConversion(new BoolInverseConverter());
            set.Apply();
        }
    }
}
