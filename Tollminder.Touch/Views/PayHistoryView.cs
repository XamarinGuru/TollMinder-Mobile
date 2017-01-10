using System;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Google.SignIn;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using Tollminder.Core;
using Tollminder.Core.Converters;
using Tollminder.Core.Helpers;
using Tollminder.Core.ViewModels;
using Tollminder.Touch.Controllers;
using Tollminder.Touch.Controls;
using Tollminder.Touch.Extensions;
using Tollminder.Touch.Helpers;
using Tollminder.Touch.Interfaces;
using UIKit;
using Foundation;
using Tollminder.Touch.Converters;
using System.Diagnostics;

namespace Tollminder.Touch.Views
{
    public class PayHistoryView : BaseViewController<PayHistoryViewModel>, ISignInUIDelegate, ICleanBackStack
    {
        UIButton backHomeView;

        TextFieldValidationWithImage firstNameTextField;
        TextFieldValidationWithImage lastNameTextField;
        TextFieldValidationWithImage emailTextField;
        TextFieldValidationWithImage addressTextField;
        TextFieldValidationWithImage cityTextField;
        TextFieldValidationWithImage stateTextField;
        TextFieldValidationWithImage zipCodeTextField;

        ProfileButton dowloadHistoryButton;
        
        public PayHistoryView()
        {
        }

        public PayHistoryView(IntPtr handle) : base(handle)
        {
        }

        public PayHistoryView(string nibName, Foundation.NSBundle bundle) : base(nibName, bundle)
        {
        }

        protected override void InitializeObjects()
        {
            base.InitializeObjects();

            var topView = new UIView();
            var scrollView = new UIScrollView();
            var topTextRowView = new UIView();
            var centerTextRowView = new UIView();
            var bottomTextRowView = new UIView();
            var bottomView = new UIView();
            backHomeView = UIButton.FromType(UIButtonType.Custom);
            backHomeView.SetImage(UIImage.FromFile(@"Images/ic_back.png"), UIControlState.Normal);
            var profileNavigationBarBackground = new UIImageView(UIImage.FromBundle(@"Images/navigation_bar_background.png"));

            // Hide navigation bar
            NavigationController.SetNavigationBarHidden(true, false);
            View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile(@"Images/tab_background.png").Scale(View.Frame.Size));//EnvironmentInfo.CheckDevice().Scale(View.Frame.Size));
            profileNavigationBarBackground.Frame = new CoreGraphics.CGRect(10, 10, profileNavigationBarBackground.Image.CGImage.Width, profileNavigationBarBackground.Image.CGImage.Height);

            topView.AddIfNotNull(profileNavigationBarBackground, backHomeView);
            topView.AddConstraints(
                profileNavigationBarBackground.WithSameWidth(topView),
                profileNavigationBarBackground.WithSameHeight(topView),
                profileNavigationBarBackground.AtTopOf(topView),

                backHomeView.WithSameCenterY(topView),
                backHomeView.AtLeftOf(topView, 20),
                backHomeView.WithRelativeWidth(topView,0.1f),
                backHomeView.WithRelativeHeight(topView, 0.2f)
            );

            firstNameTextField = TextFieldInitializer("First Name");
            lastNameTextField = TextFieldInitializer("Last Name");
            emailTextField = TextFieldInitializer("Email");
            addressTextField = TextFieldInitializer("Address");
            cityTextField = TextFieldInitializer("City");
            stateTextField = TextFieldInitializer("State");
            zipCodeTextField = TextFieldInitializer("Zip Code");
            
            dowloadHistoryButton = ProfileButtonManager.ButtonInitiaziler("Download History", UIImage.FromFile(@"Images/profileView/ic_license.png"));

            topTextRowView.AddIfNotNull(firstNameTextField, lastNameTextField);
            topTextRowView.AddConstraints(
                firstNameTextField.AtTopOf(topTextRowView),
                firstNameTextField.AtLeftOf(topTextRowView),
                firstNameTextField.WithRelativeWidth(topTextRowView, 0.475f),
                firstNameTextField.WithSameHeight(topTextRowView),

                lastNameTextField.AtTopOf(topTextRowView),
                lastNameTextField.AtRightOf(topTextRowView),
                lastNameTextField.WithRelativeWidth(topTextRowView, 0.475f),
                lastNameTextField.WithSameHeight(topTextRowView)
            );

            bottomView.AddIfNotNull(dowloadHistoryButton);
            bottomView.AddConstraints(
                dowloadHistoryButton.AtTopOf(bottomView),
                dowloadHistoryButton.WithSameCenterX(bottomView),
                dowloadHistoryButton.WithSameWidth(bottomView),
                dowloadHistoryButton.WithRelativeHeight(bottomView, 0.4f)
            );

            scrollView.AddIfNotNull(topTextRowView, bottomView);
            scrollView.AddConstraints(
                topTextRowView.AtTopOf(scrollView),
                topTextRowView.WithSameWidth(scrollView),
                topTextRowView.WithSameCenterX(scrollView),
                topTextRowView.WithRelativeHeight(scrollView, 0.12f),

                bottomView.Below(topTextRowView, 10),
                bottomView.WithSameWidth(scrollView),
                bottomView.AtLeftOf(scrollView),
                bottomView.AtRightOf(scrollView),
                bottomView.AtBottomOf(scrollView),
                bottomView.WithRelativeHeight(scrollView, 0.27f)
            );

            View.AddIfNotNull(topView, scrollView);
            View.AddConstraints(
                topView.AtTopOf(View),
                topView.WithSameWidth(View),
                topView.WithRelativeHeight(View, 0.2f),

                scrollView.Below(topView, 30),
                scrollView.AtLeftOf(View, 30),
                scrollView.AtRightOf(View, 30),
                scrollView.WithRelativeHeight(View, 0.8f)
            );

            SignIn.SharedInstance.UIDelegate = this;
            EnableNextKeyForTextFields(firstNameTextField.TextFieldWithValidator.TextField);
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

        protected override void InitializeBindings()
        {
             base.InitializeBindings();

            var set = this.CreateBindingSet<PayHistoryView, PayHistoryViewModel>();
            set.Bind(backHomeView).To(vm => vm.BackHomeCommand);
            set.Bind(dowloadHistoryButton).To(vm => vm.DownloadHistoryCommand);
            //set.Bind(_trackingButton.ButtonText).To(vm => vm.TrackingText);
            //set.Bind(_trackingButton).For(x => x.ButtonImage).To(vm => vm.IsBound).
            //   WithConversion("GetPathToImage");
            //set.Bind(_profileButton).To(vm => vm.ProfileCommand);
            //set.Bind(_callCentergButton.ButtonText).To(vm => vm.SupportText);
            set.Apply();
        }
    }
}
