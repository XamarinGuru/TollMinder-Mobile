using System;
using MvvmCross.iOS.Views;
using UIKit;
using Tollminder.Core.ViewModels.UserProfile;
using MvvmCross.Binding.BindingContext;
using Tollminder.Touch.Controls;
using Tollminder.Core.Converters;
using Tollminder.Touch.Controllers;

namespace Tollminder.Touch.Views.UserProfile
{
    public partial class RegistrationViewController : BaseViewController<RegistrationViewModel>
    {
        public RegistrationViewController() : base("RegistrationViewController", null) { }

        protected override void InitializeObjects()
        {
            base.InitializeObjects();

            SetBackground(@"Images/tab_background.png");
            RegistrationNavigationItem.Title = "Registration";
            RegistrationNavigationBar.TitleTextAttributes = new UIStringAttributes() { ForegroundColor = UIColor.White };
            RegistrationNavigationBar.TintColor = UIColor.White;
            RegistrationNavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIImage.FromFile("Images/ic_back.png"), UIBarButtonItemStyle.Plain, null);
            RegistrationNavigationItem.RightBarButtonItem = new UIBarButtonItem("Go", UIBarButtonItemStyle.Plain, null);

            AddDoneButtonOnKeyBoard();
            GestureGecognizer();
        }

        protected override void InitializeBindings()
        {
            base.InitializeBindings();

            var set = this.CreateBindingSet<RegistrationViewController, RegistrationViewModel>();

            set.Bind(RegistrationNavigationItem.LeftBarButtonItem).To(vm => vm.BackToLoginViewCommand);
            set.Bind(RegistrationNavigationItem.RightBarButtonItem).To(vm => vm.RegistrationCommand);
            set.Bind(RegistrationNavigationItem.RightBarButtonItem).For(x => x.Enabled).To(vm => vm.IsBusy).WithConversion(new BoolInverseConverter());
            set.Bind(PhoneNumberTextField).To(vm => vm.Profile.Phone);

            set.Bind(FirstNameTextField).To(vm => vm.Profile.FirstName);
            set.Bind(FirstNameTextField).For(x => x.Hidden).To(vm => vm.IsSocialRegistrationHidden).WithConversion(new BoolInverseConverter());
            set.Bind(LastNameTextField).To(vm => vm.Profile.LastName);
            set.Bind(LastNameTextField).For(x => x.Hidden).To(vm => vm.IsSocialRegistrationHidden).WithConversion(new BoolInverseConverter());
            set.Bind(EmailTextField).To(vm => vm.Profile.Email);
            set.Bind(EmailTextField).For(x => x.Hidden).To(vm => vm.IsSocialRegistrationHidden).WithConversion(new BoolInverseConverter());
            set.Bind(PasswordTextField).To(vm => vm.Profile.Password);
            set.Bind(PasswordTextField).For(x => x.Hidden).To(vm => vm.IsSocialRegistrationHidden).WithConversion(new BoolInverseConverter());
            set.Bind(ConfirmPasswordTextField).To(vm => vm.ConfirmPassword);
            set.Bind(ConfirmPasswordTextField).For(x => x.Hidden).To(vm => vm.IsSocialRegistrationHidden).WithConversion(new BoolInverseConverter());

            set.Apply();
        }

        private void GestureGecognizer()
        {
            var gestureRecognizer = new UITapGestureRecognizer(() =>
            {
                FirstNameTextField.ResignFirstResponder();
                LastNameTextField.ResignFirstResponder();
                EmailTextField.ResignFirstResponder();
                PasswordTextField.ResignFirstResponder();
                ConfirmPasswordTextField.ResignFirstResponder();
                PhoneNumberTextField.ResignFirstResponder();
            });

            View.AddGestureRecognizer(gestureRecognizer);
        }

        void AddDoneButtonOnKeyBoard()
        {
            PhoneNumberTextField.InputAccessoryView = new EnhancedToolbar(PhoneNumberTextField, null, FirstNameTextField);
            PhoneNumberTextField.KeyboardType = UIKeyboardType.NumberPad;
            PhoneNumberTextField.ShouldChangeCharacters = (textField, range, replacementString) =>
            {
                var newLength = textField.Text.Length + replacementString.Length - range.Length;
                return newLength <= 10;
            };

            FirstNameTextField.InputAccessoryView = new EnhancedToolbar(FirstNameTextField, PhoneNumberTextField, LastNameTextField);
            LastNameTextField.InputAccessoryView = new EnhancedToolbar(LastNameTextField, FirstNameTextField, EmailTextField);
            EmailTextField.InputAccessoryView = new EnhancedToolbar(EmailTextField, LastNameTextField, PasswordTextField);
            PasswordTextField.InputAccessoryView = new EnhancedToolbar(PasswordTextField, EmailTextField, ConfirmPasswordTextField);
            ConfirmPasswordTextField.InputAccessoryView = new EnhancedToolbar(ConfirmPasswordTextField, PasswordTextField, null);
        }
    }
}

