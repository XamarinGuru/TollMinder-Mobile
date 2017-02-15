using System;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Google.SignIn;
using MvvmCross.Binding.BindingContext;
using Tollminder.Core.Converters;
using Tollminder.Core.ViewModels;
using Tollminder.Touch.Controllers;
using Tollminder.Touch.Controls;
using Tollminder.Touch.Extensions;
using Tollminder.Touch.Helpers;
using Tollminder.Touch.Interfaces;
using UIKit;
using System.Diagnostics;
using MvvmCross.Binding.iOS.Views;
using Tollminder.Core;
using System.Drawing;
using Foundation;
using System.Threading.Tasks;
using Tollminder.Touch.Helpers.MvxAlertActionHelpers;

namespace Tollminder.Touch.Views
{
    public class RegistrationView : BaseViewController<RegistrationViewModel>, ICleanBackStack
    {
        UIButton backHomeView;
        UILabel nameOfPageLabel;
        UILabel informationAboutPageLabel;
        UIView topTextRowView;
        UIView centerTextRowView;
        UITextField smsInputTextField;
        UIAlertController smsConfirmationAlertController;
        TaskCompletionSource<string> smsConfirmationTask;
        MvxAlertAction smsValidateButton;

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

            scrollView.AddIfNotNull(topTextRowView, centerTextRowView, bottomView);
            scrollView.AddConstraints(
                topTextRowView.AtTopOf(scrollView, 30),
                topTextRowView.WithSameWidth(scrollView),
                topTextRowView.WithSameCenterX(scrollView),
                topTextRowView.WithRelativeHeight(scrollView, 0.12f),

                centerTextRowView.Below(topTextRowView, 10),
                centerTextRowView.WithSameWidth(scrollView),
                centerTextRowView.AtLeftOf(scrollView),
                centerTextRowView.AtRightOf(scrollView),
                centerTextRowView.WithRelativeHeight(scrollView, 0.4f),

                bottomView.Below(centerTextRowView, 10),
                bottomView.WithSameWidth(scrollView),
                bottomView.AtLeftOf(scrollView),
                bottomView.AtRightOf(scrollView),
                bottomView.AtBottomOf(scrollView, 100),
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

            EnableNextKeyForTextFields(firstNameTextField.TextFieldWithValidator.TextField, lastNameTextField.TextFieldWithValidator.TextField, emailTextField.TextFieldWithValidator.TextField,
                                       passwordTextField.TextFieldWithValidator.TextField, confirmPasswordTextField.TextFieldWithValidator.TextField, phoneNumberTextField.TextFieldWithValidator.TextField);

            registrationButton.TouchDown += (object sender, EventArgs e) => 
            {
                //SmsConfirmation("Please input code from SMS", "");
                //return smsConfirmationTask.Task;
            };
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
            smsConfirmationAlertController.AddAction(smsValidateButton.AlertAction);

            // Display the alert
            this.PresentViewController(smsConfirmationAlertController, true, null);
        }
       
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //smsConfirmationTask = new TaskCompletionSource<string>();
            //smsValidateButton = new MvxAlertAction("Validate", UIAlertActionStyle.Default);
            
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

            UnregisterKeyboardNotifications();
        }

        NSObject _keyboardObserverWillShow;
        NSObject _keyboardObserverWillHide;

        protected virtual void RegisterForKeyboardNotifications()
        {
            _keyboardObserverWillShow = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, KeyboardWillShowNotification);
            _keyboardObserverWillHide = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, KeyboardWillHideNotification);
        }

        protected virtual void UnregisterKeyboardNotifications()
        {
            NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardObserverWillShow);
            NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardObserverWillHide);
        }

        /// <summary>
        /// Gets the UIView that represents the &quot;active&quot; user input control (e.g. textfield, or button under a text field)
        /// </summary>
        /// <returns>The get active view.</returns>
        protected virtual UIView KeyboardGetActiveView()
        {
            return this.View.FindFirstResponder();
        }

        protected virtual void KeyboardWillShowNotification(NSNotification notification)
        {
            UIView activeView = KeyboardGetActiveView();
            if (activeView == null)
                return;

            UIScrollView scrollView = activeView.FindSuperviewOfType(this.View, typeof(UIScrollView)) as UIScrollView;
            if (scrollView == null)
                return;

            RectangleF keyboardBounds = (System.Drawing.RectangleF)UIKeyboard.FrameBeginFromNotification(notification);

            UIEdgeInsets contentInsets = new UIEdgeInsets(0.0f, 0.0f, keyboardBounds.Size.Height, 0.0f);
            scrollView.ContentInset = contentInsets;
            scrollView.ScrollIndicatorInsets = contentInsets;

            // If activeField is hidden by keyboard, scroll it so it’s visible
            RectangleF viewRectAboveKeyboard = new RectangleF((System.Drawing.PointF)this.View.Frame.Location, new SizeF((float)this.View.Frame.Width, (float)(this.View.Frame.Size.Height - keyboardBounds.Size.Height * 0.5f)));

            RectangleF activeFieldAbsoluteFrame = (System.Drawing.RectangleF)activeView.Superview.ConvertRectToView(activeView.Frame, this.View);
            // activeFieldAbsoluteFrame is relative to this.View so does not include any scrollView.ContentOffset

            // Check if the activeField will be partially or entirely covered by the keyboard
            if (!viewRectAboveKeyboard.Contains(activeFieldAbsoluteFrame))
            {
                // Scroll to the activeField Y position + activeField.Height + current scrollView.ContentOffset.Y – the keyboard Height
                PointF scrollPoint = new PointF(0.0f, activeFieldAbsoluteFrame.Location.Y - viewRectAboveKeyboard.Height * 1f);
                scrollView.SetContentOffset(scrollPoint, true);
            }
        }

        protected virtual void KeyboardWillHideNotification(NSNotification notification)
        {
            UIView activeView = KeyboardGetActiveView();
            if (activeView == null)
                return;

            UIScrollView scrollView = activeView.FindSuperviewOfType(this.View, typeof(UIScrollView)) as UIScrollView;
            if (scrollView == null)
                return;

            // Reset the content inset of the scrollView and animate using the current keyboard animation duration
            double animationDuration = UIKeyboard.AnimationDurationFromNotification(notification);
            UIEdgeInsets contentInsets = new UIEdgeInsets(0.0f, 0.0f, 0.0f, 0.0f);
            UIView.Animate(animationDuration, delegate
            {
                scrollView.ContentInset = contentInsets;
                scrollView.ScrollIndicatorInsets = contentInsets;
            });
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
           
            set.Bind(smsInputTextField).To(vm => vm.SmsCode);
            set.Bind(smsValidateButton).For("Click").To(vm => vm.ValidateCommand);
            set.Apply();
        }
    }
}
