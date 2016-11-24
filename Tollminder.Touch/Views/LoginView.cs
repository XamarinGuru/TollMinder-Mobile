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
        UILabel DontHaveAnAccountLabel;
        UIButton ForgotPasswordButton;
        UIButton GetStartedButton;
        UIButton LoginButton;
        UILabel LogInLabel;
        TextFieldValidationWithImage LoginTxt;
        TextFieldValidationWithImage PasswordTxt;

        UIButton FacebookLoginButton;
        UIButton GPlusLoginButton;

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
            imageView.Frame = new CoreGraphics.CGRect(10, 10, imageView.Image.CGImage.Width, imageView.Image.CGImage.Height);
            topView.AddIfNotNull(imageView);
            topView.AddConstraints(
                imageView.WithSameCenterX(topView),
                imageView.WithSameCenterY(topView)
            );

            LoginTxt = new TextFieldValidationWithImage();
            LoginTxt.TextFieldWithValidator.TextField.Placeholder = "Login";
            LoginTxt.TextFieldWithValidator.TextField.KeyboardType = UIKeyboardType.EmailAddress;
                
            PasswordTxt = new TextFieldValidationWithImage();
            PasswordTxt.TextFieldWithValidator.TextField.Placeholder = "Password";
            PasswordTxt.TextFieldWithValidator.TextField.KeyboardType = UIKeyboardType.Default;

            LoginButton = new UIButton();
            ButtonInitializer(LoginButton, "Login", UIControlState.Normal, Theme.BlueDark.ToUIColor(), 
                              UIColor.White, UIControlState.Normal, null, UIControlState.Disabled);

            GPlusLoginButton = new UIButton();
            ButtonInitializer(GPlusLoginButton, null, UIControlState.Disabled, null, null, 
                              UIControlState.Disabled, @"Images/google-button.png", UIControlState.Normal);

            FacebookLoginButton = new UIButton();
            ButtonInitializer(FacebookLoginButton, null, UIControlState.Disabled, null, null,
                              UIControlState.Disabled,@"Images/facebook-button.png", UIControlState.Normal);

            socialNetworksView.AddIfNotNull(FacebookLoginButton, GPlusLoginButton);
            //socialNetworksView.AddIfNotNull(GPlusLoginButton);
            socialNetworksView.AddConstraints(

                FacebookLoginButton.AtTopOf(socialNetworksView, 8),
                FacebookLoginButton.AtLeftOf(socialNetworksView),
                FacebookLoginButton.AtRightOf(GPlusLoginButton, 112),

                GPlusLoginButton.AtTopOf(socialNetworksView, 8),
                GPlusLoginButton.AtLeftOf(FacebookLoginButton, 112),
                GPlusLoginButton.AtRightOf(socialNetworksView)
            );

            centerView.Layer.CornerRadius = 5;
            centerView.AddIfNotNull(LoginTxt, PasswordTxt, LoginButton, socialNetworksView);
            //centerView.AddIfNotNull(PasswordTxt);
            //centerView.AddIfNotNull(LoginButton);
            //centerView.AddIfNotNull(socialNetworksView);
            centerView.BackgroundColor = UIColor.White;
            centerView.AddConstraints(
                LoginTxt.AtTopOf(centerView, 8),
                LoginTxt.AtLeftOf(centerView, -15),
                LoginTxt.AtRightOf(centerView, 20),

                PasswordTxt.Below(LoginTxt, 8),
                PasswordTxt.AtLeftOf(centerView, -15),
                PasswordTxt.AtRightOf(centerView, 20),

                LoginButton.Below(PasswordTxt, 10),
                LoginButton.AtLeftOf(centerView, 20),
                LoginButton.AtRightOf(centerView, 20),
                // make a fat button
                LoginButton.Height().EqualTo(50),

                socialNetworksView.Below(LoginButton, 20),
                socialNetworksView.AtLeftOf(centerView, 20),
                socialNetworksView.AtRightOf(centerView, 20),
                socialNetworksView.AtBottomOf(centerView, 10)
            );

            View.AddChildInParentView(topView, centerView, bottomView);
            //View.AddIfNotNull(centerView);
            //View.AddIfNotNull(bottomView);
            
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
            EnableNextKeyForTextFields(LoginTxt.TextFieldWithValidator.TextField, PasswordTxt.TextFieldWithValidator.TextField);
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
            set.Bind(LoginTxt.TextFieldWithValidator.TextField).To(vm => vm.LoginString);
            set.Bind(LoginTxt.TextFieldWithValidator).For(v => v.ErrorMessageString).To(vm => vm.Errors["Login"]);
            set.Bind(PasswordTxt.TextFieldWithValidator.TextField).To(vm => vm.PasswordString);
            set.Bind(PasswordTxt.TextFieldWithValidator).For(v => v.ErrorMessageString).To(vm => vm.Errors["Password"]);
            set.Bind(LoginButton).To(vm => vm.EmailLoginCommand);
            set.Bind(LoginButton).For(x => x.Enabled).To(vm => vm.IsBusy).WithConversion(new BoolInverseConverter());
            set.Bind(FacebookLoginButton).To(vm => vm.FacebookLoginCommand);
            set.Bind(FacebookLoginButton).For(x => x.Enabled).To(vm => vm.IsBusy).WithConversion(new BoolInverseConverter());
            set.Bind(GPlusLoginButton).To(vm => vm.GPlusLoginCommand);
            set.Bind(GPlusLoginButton).For(x => x.Enabled).To(vm => vm.IsBusy).WithConversion(new BoolInverseConverter());
            set.Apply();
        }
    }
}
