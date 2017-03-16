using System;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using Tollminder.Core.Converters;
using Tollminder.Touch.Controllers;
using Tollminder.Touch.Controls;
using Tollminder.Touch.Extensions;
using Tollminder.Touch.Interfaces;
using UIKit;
using Tollminder.Core;
using Tollminder.Core.ViewModels.UserProfile;

namespace Tollminder.Touch.Views
{
    public class RegistrationView : BaseViewController<RegistrationViewModel>, ICleanBackStack
    {
        UIButton backHomeView;
        UILabel nameOfPageLabel;
        UILabel informationAboutPageLabel;
        UIView topTextRowView;
        UIView centerTextRowView;

        TextFieldValidationWithImage firstNameTextField;
        TextFieldValidationWithImage lastNameTextField;
        TextFieldValidationWithImage emailTextField;
        TextFieldValidationWithImage passwordTextField;
        TextFieldValidationWithImage confirmPasswordTextField;
        TextFieldValidationWithImage phoneNumberTextField;

        UIButton registrationButton;

        public RegistrationView()
        {
        }

        public RegistrationView(IntPtr handle) : base(handle)
        {
        }

        public RegistrationView(string nibName, Foundation.NSBundle bundle) : base(nibName, bundle)
        {
        }

        protected override void InitializeObjects()
        {
            base.InitializeObjects();

            var topView = new UIView();
            var stackView = new UIStackView();
            var scrollView = new UIScrollView();
            topTextRowView = new UIView();
            centerTextRowView = new UIView();
            var bottomTextRowView = new UIView();
            var bottomView = new UIView();
            var profileNavigationBarBackground = new UIImageView(UIImage.FromBundle(@"Images/navigation_bar_background.png"));

            backHomeView = UIButton.FromType(UIButtonType.Custom);
            backHomeView.SetImage(UIImage.FromFile(@"Images/ic_back.png"), UIControlState.Normal);
            nameOfPageLabel = LabelInformationAboutPage(UIColor.White, "Registration", UIFont.BoldSystemFontOfSize(16f));
            informationAboutPageLabel = LabelInformationAboutPage(UIColor.FromRGB(29, 157, 189), "", UIFont.FromName("Helvetica", 14f));

            // Hide navigation bar
            NavigationController.SetNavigationBarHidden(true, false);
            View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile(@"Images/tab_background.png").Scale(View.Frame.Size));
            profileNavigationBarBackground.Frame = new CGRect(10, 10, profileNavigationBarBackground.Image.CGImage.Width, profileNavigationBarBackground.Image.CGImage.Height);
            var s = UIScreen.MainScreen.Bounds.Width;

            var labelView = new UIView();
            labelView.AddIfNotNull(nameOfPageLabel, informationAboutPageLabel);
            labelView.AddConstraints(
                nameOfPageLabel.AtTopOf(labelView, 20),
                nameOfPageLabel.WithSameCenterX(labelView),
                nameOfPageLabel.WithSameCenterY(labelView),
                nameOfPageLabel.WithSameWidth(labelView),
                nameOfPageLabel.WithRelativeHeight(labelView, 0.3f),

                informationAboutPageLabel.Below(nameOfPageLabel, 5),
                informationAboutPageLabel.WithSameWidth(labelView),
                informationAboutPageLabel.WithSameCenterX(labelView),
                informationAboutPageLabel.WithRelativeHeight(labelView, 0.3f)
            );

            topView.AddIfNotNull(profileNavigationBarBackground, backHomeView, labelView);
            topView.AddConstraints(
                profileNavigationBarBackground.WithSameWidth(topView),
                profileNavigationBarBackground.WithSameHeight(topView),
                profileNavigationBarBackground.AtTopOf(topView),

                backHomeView.WithSameCenterY(topView),
                backHomeView.AtLeftOf(topView, 20),
                backHomeView.WithRelativeWidth(topView, 0.1f),
                backHomeView.WithRelativeHeight(topView, 0.2f),

                labelView.WithSameCenterX(topView),
                labelView.WithSameCenterY(topView),
                labelView.WithRelativeWidth(topView, 0.8f),
                labelView.WithRelativeHeight(topView, 0.6f)
            );

            firstNameTextField = TextFieldInitializer("First Name");
            lastNameTextField = TextFieldInitializer("Last Name");
            emailTextField = TextFieldInitializer("Email");
            passwordTextField = TextFieldInitializer("Password *");
            confirmPasswordTextField = TextFieldInitializer("Confirm Password *");
            phoneNumberTextField = TextFieldInitializer("(000) 000-0000 *");

            registrationButton = ButtonInitializer("Registration", UIControlState.Normal, Theme.BlueDark.ToUIColor(),
                              UIColor.White, UIControlState.Normal, null, UIControlState.Disabled);

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

            centerTextRowView.AddIfNotNull(emailTextField, passwordTextField, confirmPasswordTextField);
            centerTextRowView.AddConstraints(
                emailTextField.AtTopOf(centerTextRowView),
                emailTextField.WithSameCenterX(centerTextRowView),
                emailTextField.WithSameWidth(centerTextRowView),
                emailTextField.WithRelativeHeight(centerTextRowView, 0.3f),

                passwordTextField.Below(emailTextField, 10),
                passwordTextField.WithSameCenterX(centerTextRowView),
                passwordTextField.WithSameWidth(centerTextRowView),
                passwordTextField.WithRelativeHeight(centerTextRowView, 0.3f),

                confirmPasswordTextField.Below(passwordTextField, 10),
                confirmPasswordTextField.WithSameCenterX(centerTextRowView),
                confirmPasswordTextField.WithSameWidth(centerTextRowView),
                confirmPasswordTextField.WithRelativeHeight(centerTextRowView, 0.3f)
            );

            bottomView.AddIfNotNull(registrationButton, phoneNumberTextField);
            bottomView.AddConstraints(
                phoneNumberTextField.AtTopOf(bottomView),
                phoneNumberTextField.WithSameCenterX(bottomView),
                phoneNumberTextField.WithSameWidth(bottomView),
                phoneNumberTextField.WithRelativeHeight(bottomView, 0.4f),

                registrationButton.Below(phoneNumberTextField, 10),
                registrationButton.WithSameCenterX(bottomView),
                registrationButton.WithSameWidth(bottomView),
                registrationButton.WithRelativeHeight(bottomView, 0.4f)
            );

            stackView.AddIfNotNull(topTextRowView, centerTextRowView, bottomView);
            stackView.AddConstraints(
                topTextRowView.AtTopOf(stackView, 30),
                topTextRowView.WithSameWidth(stackView),
                topTextRowView.WithSameCenterX(stackView),
                topTextRowView.WithRelativeHeight(stackView, 0.10f),

                centerTextRowView.Below(topTextRowView, 10),
                centerTextRowView.WithSameWidth(stackView),
                centerTextRowView.AtLeftOf(stackView),
                centerTextRowView.AtRightOf(stackView),
                centerTextRowView.WithRelativeHeight(stackView, 0.35f),

                bottomView.Below(centerTextRowView, 10),
                bottomView.WithSameWidth(stackView),
                bottomView.AtLeftOf(stackView),
                bottomView.AtRightOf(stackView),
                bottomView.WithRelativeHeight(stackView, 0.25f)
            );

            View.AddIfNotNull(topView, stackView);
            View.AddConstraints(
                topView.AtTopOf(View),
                topView.WithSameWidth(View),
                topView.WithRelativeHeight(View, 0.2f),

                stackView.Below(topView, 30),
                stackView.AtLeftOf(View, 30),
                stackView.AtRightOf(View, 30),
                stackView.WithRelativeHeight(View, 0.8f)
            );

            EnableNextKeyForTextFields(firstNameTextField.TextFieldWithValidator.TextField, lastNameTextField.TextFieldWithValidator.TextField, emailTextField.TextFieldWithValidator.TextField,
                                       passwordTextField.TextFieldWithValidator.TextField, confirmPasswordTextField.TextFieldWithValidator.TextField, phoneNumberTextField.TextFieldWithValidator.TextField);
        }

        protected override void InitializeBindings()
        {
            base.InitializeBindings();

            var set = this.CreateBindingSet<RegistrationView, RegistrationViewModel>();
            set.Bind(informationAboutPageLabel).To(vm => vm.ViewInformation);
            set.Bind(backHomeView).To(vm => vm.BackToLoginViewCommand);

            set.Bind(topTextRowView).For(x => x.Hidden).To(vm => vm.IsSocialRegistrationHidden).WithConversion(new BoolInverseConverter());
            set.Bind(centerTextRowView).For(x => x.Hidden).To(vm => vm.IsSocialRegistrationHidden).WithConversion(new BoolInverseConverter());
            set.Bind(firstNameTextField.TextFieldWithValidator.TextField).To(vm => vm.Profile.FirstName);
            set.Bind(lastNameTextField.TextFieldWithValidator.TextField).To(vm => vm.Profile.LastName);
            set.Bind(emailTextField.TextFieldWithValidator.TextField).To(vm => vm.Profile.Email);
            set.Bind(passwordTextField.TextFieldWithValidator.TextField).To(vm => vm.Profile.Password);
            set.Bind(confirmPasswordTextField.TextFieldWithValidator.TextField).To(vm => vm.ConfirmPassword);

            set.Bind(phoneNumberTextField.TextFieldWithValidator.TextField).To(vm => vm.Profile.Phone);
            set.Bind(registrationButton).To(vm => vm.RegistrationCommand);
            set.Bind(registrationButton).For(x => x.Enabled).To(vm => vm.IsBusy).WithConversion(new BoolInverseConverter());

            set.Apply();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var gestureRecognizer = new UITapGestureRecognizer(() =>
            {
                firstNameTextField.TextFieldWithValidator.TextField.ResignFirstResponder();
                lastNameTextField.TextFieldWithValidator.TextField.ResignFirstResponder();
                emailTextField.TextFieldWithValidator.TextField.ResignFirstResponder();
                passwordTextField.TextFieldWithValidator.TextField.ResignFirstResponder();
                confirmPasswordTextField.TextFieldWithValidator.TextField.ResignFirstResponder();
                phoneNumberTextField.TextFieldWithValidator.TextField.ResignFirstResponder();
            });
            View.AddGestureRecognizer(gestureRecognizer);
            AddDoneButtonOnKeyBoard();
            RegisterForKeyboardNotifications();
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();

            UnregisterForKeyboardNotifications();
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

        private UILabel LabelInformationAboutPage(UIColor color, string text, UIFont font)
        {
            var labelInformation = new UILabel();
            labelInformation.TextColor = color;
            labelInformation.Text = text;
            labelInformation.Font = font;
            labelInformation.TextAlignment = UITextAlignment.Center;
            return labelInformation;
        }

        private UIButton ButtonInitializer(string title, UIControlState titleState, UIColor backgroundColor,
                                           UIColor titleColor, UIControlState colorTitleState, string imagePath, UIControlState imageState)
        {
            UIButton button = new UIButton();
            button.SetTitle(title, titleState);
            if (imagePath != null)
            {
                button.SetImage(UIImage.FromFile(imagePath), imageState);
                button.HorizontalAlignment = UIControlContentHorizontalAlignment.Fill;
                button.VerticalAlignment = UIControlContentVerticalAlignment.Fill;
            }
            button.BackgroundColor = backgroundColor;
            button.SetTitleColor(titleColor, colorTitleState);
            button.ImageView.ContentMode = UIViewContentMode.ScaleToFill;
            button.ClipsToBounds = false;
            button.Layer.CornerRadius = 10;
            button.Layer.ShadowColor = UIColor.Black.CGColor;
            button.Layer.ShadowOpacity = 0.1f;
            button.Layer.ShadowRadius = 1;
            button.Layer.ShadowOffset = new CGSize(1, 1);
            return button;
        }

        void AddDoneButtonOnKeyBoard()
        {
            firstNameTextField.TextFieldWithValidator.TextField.InputAccessoryView = new EnhancedToolbar(firstNameTextField.TextFieldWithValidator.TextField,
                                                                                                         null, lastNameTextField.TextFieldWithValidator.TextField);
            lastNameTextField.TextFieldWithValidator.TextField.InputAccessoryView = new EnhancedToolbar(lastNameTextField.TextFieldWithValidator.TextField,
                                                                                                         firstNameTextField.TextFieldWithValidator.TextField,
                                                                                                        emailTextField.TextFieldWithValidator.TextField);
            emailTextField.TextFieldWithValidator.TextField.InputAccessoryView = new EnhancedToolbar(emailTextField.TextFieldWithValidator.TextField,
                                                                                                     lastNameTextField.TextFieldWithValidator.TextField,
                                                                                                     passwordTextField.TextFieldWithValidator.TextField);
            passwordTextField.TextFieldWithValidator.TextField.InputAccessoryView = new EnhancedToolbar(passwordTextField.TextFieldWithValidator.TextField,
                                                                                                        emailTextField.TextFieldWithValidator.TextField,
                                                                                                        confirmPasswordTextField.TextFieldWithValidator.TextField);
            confirmPasswordTextField.TextFieldWithValidator.TextField.InputAccessoryView = new EnhancedToolbar(confirmPasswordTextField.TextFieldWithValidator.TextField,
                                                                                                               passwordTextField.TextFieldWithValidator.TextField,
                                                                                                               phoneNumberTextField.TextFieldWithValidator.TextField);
            phoneNumberTextField.TextFieldWithValidator.TextField.InputAccessoryView = new EnhancedToolbar(phoneNumberTextField.TextFieldWithValidator.TextField,
                                                                                                           confirmPasswordTextField.TextFieldWithValidator.TextField,
                                                                                                            null);
            phoneNumberTextField.TextFieldWithValidator.TextField.KeyboardType = UIKeyboardType.NumberPad;
            phoneNumberTextField.TextFieldWithValidator.TextField.ShouldChangeCharacters = (textField, range, replacementString) =>
            {
                var newLength = textField.Text.Length + replacementString.Length - range.Length;
                return newLength <= 10;
            };
        }
    }
}
