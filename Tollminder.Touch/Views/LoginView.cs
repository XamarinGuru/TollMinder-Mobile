﻿using System;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using Tollminder.Core;
using Tollminder.Core.ViewModels;
using Tollminder.Touch.Controllers;
using Tollminder.Touch.Controls;
using Tollminder.Touch.Extensions;
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
        TextFieldValidationWithImage LoginTxt { get; set; }
        TextFieldValidationWithImage PasswordTxt { get; set; }

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

            View.BackgroundColor = UIColor.White;

            var topView = new UIView();
            var centerView = new UIView();
            var bottomView = new UIView();

            LoginTxt = new TextFieldValidationWithImage();
            LoginTxt.TextFieldWithValidator.TextField.Placeholder = "Login";
            LoginTxt.TextFieldWithValidator.TextField.KeyboardType = UIKeyboardType.EmailAddress;
            PasswordTxt = new TextFieldValidationWithImage();
            PasswordTxt.TextFieldWithValidator.TextField.Placeholder = "Password";
            PasswordTxt.TextFieldWithValidator.TextField.KeyboardType = UIKeyboardType.Default;
            LoginButton = new UIButton();
            LoginButton.SetTitle("Login", UIControlState.Normal);
            LoginButton.BackgroundColor = Theme.Green.ToUIColor();
            LoginButton.SetTitleColor(UIColor.White, UIControlState.Normal);
            LoginButton.ClipsToBounds = false;
            LoginButton.Layer.CornerRadius = 5;
            LoginButton.Layer.ShadowColor = UIColor.Black.CGColor;
            LoginButton.Layer.ShadowOpacity = 0.1f;
            LoginButton.Layer.ShadowRadius = 1;
            LoginButton.Layer.ShadowOffset = new CGSize(1, 1);

            centerView.AddIfNotNull(LoginTxt);
            centerView.AddIfNotNull(PasswordTxt);
            centerView.AddIfNotNull(LoginButton);

            centerView.AddConstraints(
                LoginTxt.AtTopOf(centerView, 8),
                LoginTxt.AtLeftOf(centerView, 8),
                LoginTxt.AtRightOf(centerView, 8),

                PasswordTxt.Below(LoginTxt, 8),
                PasswordTxt.AtLeftOf(centerView, 8),
                PasswordTxt.AtRightOf(centerView, 8),

                LoginButton.Below(PasswordTxt, 8),
                LoginButton.AtLeftOf(centerView, 50),
                LoginButton.AtRightOf(centerView, 50)
            );

            View.AddIfNotNull(topView);
            View.AddIfNotNull(centerView);
            View.AddIfNotNull(bottomView);

            View.AddConstraints(
                topView.AtTopOf(View),
                topView.AtLeftOf(View),
                topView.AtRightOf(View),
                topView.WithRelativeHeight(View, 0.3f),

                centerView.AtLeftOf(View),
                centerView.AtRightOf(View),
                centerView.Below(topView),
                centerView.WithRelativeHeight(View, 0.4f),

                bottomView.AtBottomOf(View),
                bottomView.AtLeftOf(View),
                bottomView.AtRightOf(View),
                bottomView.Below(centerView),
                bottomView.WithRelativeHeight(View, 0.3f)
            );
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
            set.Apply();
        }
    }
}
