using System;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using Tollminder.Touch.Controllers;
using Tollminder.Touch.Controls;
using Tollminder.Touch.Extensions;
using Tollminder.Touch.Helpers;
using Tollminder.Touch.Interfaces;
using UIKit;
using System.Diagnostics;
using MvvmCross.Binding.iOS.Views;
using Tollminder.Core.ViewModels.UserProfile;

namespace Tollminder.Touch.Views
{
    public class ProfileView : BaseViewController<ProfileViewModel>, ICleanBackStack
    {
        UIButton backHomeView;
        UILabel nameOfPageLabel;
        UILabel informationAboutPageLabel;

        TextFieldValidationWithImage firstNameTextField;
        TextFieldValidationWithImage lastNameTextField;
        TextFieldValidationWithImage emailTextField;
        TextFieldValidationWithImage addressTextField;
        TextFieldValidationWithImage cityTextField;
        TextFieldValidationWithImage stateTextField;
        TextFieldValidationWithImage zipCodeTextField;

        UIPickerView statesPicker;
        MvxPickerViewModel statesPickerViewModel;

        ProfileButton addLicenseButton;
        ProfileButton showCreditCardButton;

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
            var topTextRowView = new UIView();
            var centerTextRowView = new UIView();
            var bottomTextRowView = new UIView();
            var bottomView = new UIView();
            var profileNavigationBarBackground = new UIImageView(UIImage.FromBundle(@"Images/navigation_bar_background.png"));

            backHomeView = UIButton.FromType(UIButtonType.Custom);
            backHomeView.SetImage(UIImage.FromFile(@"Images/ic_back.png"), UIControlState.Normal);
            nameOfPageLabel = LabelInformationAboutPage(UIColor.White, "Profile", UIFont.BoldSystemFontOfSize(16f));
            informationAboutPageLabel = LabelInformationAboutPage(UIColor.FromRGB(29, 157, 189), "Please, Enter Your Personal Information.", UIFont.FromName("Helvetica", 14f));

            // Hide navigation bar
            NavigationController.SetNavigationBarHidden(true, false);
            View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile(@"Images/tab_background.png").Scale(View.Frame.Size));
            profileNavigationBarBackground.Frame = new CGRect(10, 10, profileNavigationBarBackground.Image.CGImage.Width, profileNavigationBarBackground.Image.CGImage.Height);

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
            addressTextField = TextFieldInitializer("Address");
            cityTextField = TextFieldInitializer("City");
            zipCodeTextField = TextFieldInitializer("Zip Code");
            zipCodeTextField.TextFieldWithValidator.TextField.SecureTextEntry = true;

            stateTextField = TextFieldInitializer("State");
            statesPicker = new UIPickerView();
            statesPickerViewModel = new MvxPickerViewModel(statesPicker);
            statesPicker.Model = statesPickerViewModel;
            statesPicker.ShowSelectionIndicator = true;
            statesPicker.BackgroundColor = UIColor.White;

            addLicenseButton = ProfileButtonManager.ButtonInitiaziler("Add License Plate", UIImage.FromFile(@"Images/ProfileView/ic_license.png"));
            showCreditCardButton = ProfileButtonManager.ButtonInitiaziler("Credit Cards", UIImage.FromFile(@"Images/ProfileView/ic_card.png"));

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

            centerTextRowView.AddIfNotNull(emailTextField, addressTextField, cityTextField);
            centerTextRowView.AddConstraints(
                emailTextField.AtTopOf(centerTextRowView),
                emailTextField.WithSameCenterX(centerTextRowView),
                emailTextField.WithSameWidth(centerTextRowView),
                emailTextField.WithRelativeHeight(centerTextRowView, 0.3f),

                addressTextField.Below(emailTextField, 10),
                addressTextField.WithSameCenterX(centerTextRowView),
                addressTextField.WithSameWidth(centerTextRowView),
                addressTextField.WithRelativeHeight(centerTextRowView, 0.3f),

                cityTextField.Below(addressTextField, 10),
                cityTextField.WithSameCenterX(centerTextRowView),
                cityTextField.WithSameWidth(centerTextRowView),
                cityTextField.WithRelativeHeight(centerTextRowView, 0.3f)
            );

            bottomTextRowView.AddIfNotNull(stateTextField, zipCodeTextField);
            bottomTextRowView.AddConstraints(
                stateTextField.AtTopOf(bottomTextRowView),
                stateTextField.AtLeftOf(bottomTextRowView),
                stateTextField.WithRelativeWidth(bottomTextRowView, 0.475f),
                stateTextField.WithSameHeight(bottomTextRowView),

                zipCodeTextField.AtTopOf(bottomTextRowView),
                zipCodeTextField.AtRightOf(bottomTextRowView),
                zipCodeTextField.WithRelativeWidth(bottomTextRowView, 0.475f),
                zipCodeTextField.WithSameHeight(bottomTextRowView)
            );

            bottomView.AddIfNotNull(addLicenseButton, showCreditCardButton);
            bottomView.AddConstraints(
                addLicenseButton.AtTopOf(bottomView),
                addLicenseButton.WithSameCenterX(bottomView),
                addLicenseButton.WithSameWidth(bottomView),
                addLicenseButton.WithRelativeHeight(bottomView, 0.4f),

                showCreditCardButton.Below(addLicenseButton, 10),
                showCreditCardButton.WithSameCenterX(bottomView),
                showCreditCardButton.WithSameWidth(bottomView),
                showCreditCardButton.WithRelativeHeight(bottomView, 0.4f)
            );

            scrollView.AddIfNotNull(topTextRowView, centerTextRowView, bottomTextRowView, bottomView);
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

                bottomTextRowView.Below(centerTextRowView, 10),
                bottomTextRowView.WithSameWidth(scrollView),
                bottomTextRowView.WithSameCenterX(scrollView),
                bottomTextRowView.WithRelativeHeight(scrollView, 0.12f),

                bottomView.Below(bottomTextRowView, 10),
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
                                       addressTextField.TextFieldWithValidator.TextField, cityTextField.TextFieldWithValidator.TextField, stateTextField.TextFieldWithValidator.TextField,
                                       zipCodeTextField.TextFieldWithValidator.TextField);
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

        protected override void InitializeBindings()
        {
            base.InitializeBindings();

            var set = this.CreateBindingSet<ProfileView, ProfileViewModel>();
            set.Bind(backHomeView).To(vm => vm.BackHomeCommand);
            set.Bind(addLicenseButton).To(vm => vm.AddLicenseCommand);
            set.Bind(showCreditCardButton).To(vm => vm.ShowCreditCardsCommand);

            set.Bind(firstNameTextField.TextFieldWithValidator.TextField).To(vm => vm.Profile.FirstName);
            set.Bind(lastNameTextField.TextFieldWithValidator.TextField).To(vm => vm.Profile.LastName);
            set.Bind(emailTextField.TextFieldWithValidator.TextField).To(vm => vm.Profile.Email);
            set.Bind(addressTextField.TextFieldWithValidator.TextField).To(vm => vm.Profile.Address);
            set.Bind(cityTextField.TextFieldWithValidator.TextField).To(vm => vm.Profile.City);
            set.Bind(zipCodeTextField.TextFieldWithValidator.TextField).To(vm => vm.Profile.ZipCode);

            set.Bind(statesPickerViewModel).For(p => p.ItemsSource).To(vm => vm.States);
            set.Bind(statesPickerViewModel).For(p => p.SelectedItem).To(vm => vm.SelectedState);
            set.Bind(stateTextField.TextFieldWithValidator.TextField).To(vm => vm.SelectedState);

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
                addressTextField.TextFieldWithValidator.TextField.ResignFirstResponder();
                cityTextField.TextFieldWithValidator.TextField.ResignFirstResponder();
                zipCodeTextField.TextFieldWithValidator.TextField.ResignFirstResponder();
                stateTextField.TextFieldWithValidator.TextField.ResignFirstResponder();
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

        void AddDoneButtonOnKeyBoard()
        {
            firstNameTextField.TextFieldWithValidator.TextField.InputAccessoryView = new EnhancedToolbar(firstNameTextField.TextFieldWithValidator.TextField,
                                                                                                         null, lastNameTextField.TextFieldWithValidator.TextField);
            lastNameTextField.TextFieldWithValidator.TextField.InputAccessoryView = new EnhancedToolbar(lastNameTextField.TextFieldWithValidator.TextField,
                                                                                                         firstNameTextField.TextFieldWithValidator.TextField,
                                                                                                        emailTextField.TextFieldWithValidator.TextField);
            emailTextField.TextFieldWithValidator.TextField.InputAccessoryView = new EnhancedToolbar(emailTextField.TextFieldWithValidator.TextField,
                                                                                                     lastNameTextField.TextFieldWithValidator.TextField,
                                                                                                     addressTextField.TextFieldWithValidator.TextField);
            addressTextField.TextFieldWithValidator.TextField.InputAccessoryView = new EnhancedToolbar(addressTextField.TextFieldWithValidator.TextField,
                                                                                                        emailTextField.TextFieldWithValidator.TextField,
                                                                                                       cityTextField.TextFieldWithValidator.TextField);
            cityTextField.TextFieldWithValidator.TextField.InputAccessoryView = new EnhancedToolbar(cityTextField.TextFieldWithValidator.TextField,
                                                                                                    addressTextField.TextFieldWithValidator.TextField,
                                                                                                    stateTextField.TextFieldWithValidator.TextField);
            stateTextField.TextFieldWithValidator.TextField.InputAccessoryView = new EnhancedToolbar(stateTextField.TextFieldWithValidator.TextField,
                                                                                                           cityTextField.TextFieldWithValidator.TextField,
                                                                                                            zipCodeTextField.TextFieldWithValidator.TextField);
            stateTextField.TextFieldWithValidator.TextField.InputView = statesPicker;

            zipCodeTextField.TextFieldWithValidator.TextField.InputAccessoryView = new EnhancedToolbar(zipCodeTextField.TextFieldWithValidator.TextField,
                                                                                                           stateTextField.TextFieldWithValidator.TextField,
                                                                                                            null);
            zipCodeTextField.TextFieldWithValidator.TextField.KeyboardType = UIKeyboardType.NumberPad;
            zipCodeTextField.TextFieldWithValidator.TextField.ShouldChangeCharacters = (textField, range, replacementString) =>
            {
                var newLength = textField.Text.Length + replacementString.Length - range.Length;
                return newLength <= 10;
            };
        }
    }
}
