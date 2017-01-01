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
    public class ProfileView : BaseViewController<ProfileViewModel>, ISignInUIDelegate, ICleanBackStack
    {
        TextFieldValidationWithImage firstNameTextField;
        TextFieldValidationWithImage lastNameTextField;
        TextFieldValidationWithImage emailTextField;
        TextFieldValidationWithImage addressTextField;
        TextFieldValidationWithImage cityTextField;
        TextFieldValidationWithImage zipCodeTextField;
        ProfileButton addLicenseButton;
        ProfileButton addCreditCardButton;
        
        public ProfileView()
        {
        }

        public ProfileView(IntPtr handle) : base(handle)
        {
        }

        public ProfileView(string nibName, Foundation.NSBundle bundle) : base(nibName, bundle)
        {
        }

        protected override void InitializeObjects()
        {
            base.InitializeObjects();

            var topView = new UIView();
            var scrollView = new UIScrollView();
            var firstRowView = new UIView();
            var bottomView = new UIView();
            var profileNavigationBarBackground = new UIImageView(UIImage.FromBundle(@"Images/navigation_bar_background.png"));

            // Hide navigation bar
            NavigationController.SetNavigationBarHidden(true, false);
            View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile(@"Images/tab_background.png").Scale(View.Frame.Size));//EnvironmentInfo.CheckDevice().Scale(View.Frame.Size));
            profileNavigationBarBackground.Frame = new CoreGraphics.CGRect(10, 10, profileNavigationBarBackground.Image.CGImage.Width, profileNavigationBarBackground.Image.CGImage.Height);
            topView.AddIfNotNull(profileNavigationBarBackground);
            topView.AddConstraints(
                profileNavigationBarBackground.WithSameWidth(topView),
                profileNavigationBarBackground.WithSameHeight(topView),
                profileNavigationBarBackground.AtTopOf(topView)
            );

            firstNameTextField = TextFieldInitializer("First Name");
            lastNameTextField = TextFieldInitializer("Last Name");
            emailTextField = TextFieldInitializer("Email");
            addressTextField = TextFieldInitializer("Address");
            cityTextField = TextFieldInitializer("City");
            zipCodeTextField = TextFieldInitializer("Zip Code");
            
            addLicenseButton = ProfileButtonManager.ButtonInitiaziler("Add License Plate", UIImage.FromFile(@"Images/profileView/ic_license.png"));
            addCreditCardButton = ProfileButtonManager.ButtonInitiaziler("Add Credit Card", UIImage.FromFile(@"Images/profileView/ic_card.png"));

            firstRowView.BackgroundColor = UIColor.Brown;
            firstRowView.AddIfNotNull(firstNameTextField, lastNameTextField);
            firstRowView.AddConstraints(
                firstNameTextField.AtTopOf(firstRowView, 10),
                firstNameTextField.AtLeftOf(firstRowView),
                firstNameTextField.WithRelativeWidth(firstRowView, 0.475f),
                firstNameTextField.WithRelativeHeight(firstRowView, 0.25f),

                lastNameTextField.AtTopOf(firstRowView, 10),
                lastNameTextField.AtRightOf(firstRowView),
                lastNameTextField.WithRelativeWidth(firstRowView, 0.475f),
                lastNameTextField.WithRelativeHeight(firstRowView, 0.25f)
                //    _payButton.AtTopOf(centerView, 10),
                //    _payButton.AtRightOf(centerView, 8),
                //    _payButton.WithRelativeWidth(centerView, 0.4f),
                //    _payButton.WithRelativeHeight(centerView, 0.47f)
                );
                bottomView.BackgroundColor = UIColor.Black;
                bottomView.AddIfNotNull(addLicenseButton, addCreditCardButton);
                bottomView.AddConstraints(
                addLicenseButton.AtTopOf(bottomView),
                addLicenseButton.WithSameCenterX(bottomView),
                addLicenseButton.WithSameWidth(bottomView),
                addLicenseButton.WithRelativeHeight(bottomView, 0.4f), 

                addCreditCardButton.Below(addLicenseButton, 10),
                addCreditCardButton.WithSameCenterX(bottomView),
                addCreditCardButton.WithSameWidth(bottomView),
                addCreditCardButton.WithRelativeHeight(bottomView, 0.4f)
            );

            scrollView.AddIfNotNull(firstRowView, , emailTextField, addressTextField, cityTextField, bottomView);
            scrollView.AddConstraints(
                firstRowView.AtTopOf(scrollView),
                firstRowView.AtLeftOf(scrollView, 25),
                firstRowView.AtRightOf(scrollView, 80),
                firstRowView.WithRelativeHeight(scrollView, 0.43f),

                bottomView.Below(firstRowView),
                bottomView.AtLeftOf(scrollView, 25),
                bottomView.AtRightOf(scrollView, 80),
                bottomView.AtBottomOf(scrollView, 30),
                bottomView.WithRelativeHeight(scrollView, 0.27f)
            );

            View.AddIfNotNull(topView, scrollView);
            View.AddConstraints(
                topView.AtTopOf(View),
                topView.WithSameWidth(View),
                topView.WithRelativeHeight(View, 0.2f),

                scrollView.Below(topView),
                scrollView.WithSameCenterX(View),
                scrollView.WithSameWidth(View),
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
            // base.InitializeBindings();

            //var set = this.CreateBindingSet<ProfileView, ProfileViewModel>();
            //set.Bind(_trackingButton).To(vm => vm.TrackingCommand);
            //set.Bind(_trackingButton.ButtonText).To(vm => vm.TrackingText);
            //set.Bind(_trackingButton).For(x => x.ButtonImage).To(vm => vm.IsBound).
            //   WithConversion("GetPathToImage");
            //set.Bind(_profileButton).To(vm => vm.ProfileCommand);
            //set.Bind(_callCentergButton.ButtonText).To(vm => vm.SupportText);
            //set.Apply();

        }
    }
}
