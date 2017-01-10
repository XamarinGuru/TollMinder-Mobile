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
using System.Collections.Generic;
using Tollminder.Core.Models;

namespace Tollminder.Touch.Views
{
    public class LicensePlateView : BaseViewController<LicenseViewModel>, ISignInUIDelegate, ICleanBackStack
    {
        UIButton backHomeView;

        TextFieldValidationWithImage licensePlateTextField;
        TextFieldValidationWithImage stateTextField;
        UIPickerView picker;
        TextFieldValidationWithImage vehicleClassTextField;
        List<StatesData> states;

        public LicensePlateView()
        {
        }

        public LicensePlateView(IntPtr handle) : base(handle)
        {
        }

        public LicensePlateView(string nibName, Foundation.NSBundle bundle) : base(nibName, bundle)
        {
        }

        protected override void InitializeObjects()
        {
            base.InitializeObjects();

            var topView = new UIView();
            var scrollView = new UIScrollView();
            var topTextRowView = new UIView();
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

            licensePlateTextField = TextFieldInitializer("LicensePlate");
            stateTextField = TextFieldInitializer("State");//new UILabel();
            var tap = new UITapGestureRecognizer(() => {
                if (picker.Hidden)
                {
                    picker.Hidden = false;
                    picker.WithRelativeHeight(View, 0.6f);
                }
                else
                    picker.WithRelativeHeight(View, 0.3f);
            });
            UILabel touchLabel = new UILabel();
            touchLabel.UserInteractionEnabled = true;
            touchLabel.AddGestureRecognizer(tap);
            picker = new UIPickerView();
            //picker.Model = new PickerModel(stateTextField, );//TextFieldInitializer("State");
            picker.BackgroundColor = UIColor.White;
            vehicleClassTextField = TextFieldInitializer("Vehicle CLass");

            topTextRowView.AddIfNotNull(licensePlateTextField, touchLabel, stateTextField, vehicleClassTextField);
            topTextRowView.AddConstraints(
                licensePlateTextField.AtTopOf(topTextRowView),
                licensePlateTextField.WithSameCenterX(topTextRowView),
                licensePlateTextField.WithSameWidth(topTextRowView),
                licensePlateTextField.WithRelativeHeight(topTextRowView, 0.3f),

                stateTextField.Below(touchLabel, 10),
                stateTextField.WithSameCenterX(topTextRowView),
                stateTextField.WithSameWidth(topTextRowView),
                stateTextField.WithRelativeHeight(topTextRowView, 0.3f),

                touchLabel.Below(licensePlateTextField, 10),
                touchLabel.WithSameCenterX(topTextRowView),
                touchLabel.WithSameWidth(topTextRowView),
                touchLabel.WithRelativeHeight(topTextRowView, 0.3f),

                vehicleClassTextField.Below(touchLabel, 10),
                vehicleClassTextField.WithSameCenterX(topTextRowView),
                vehicleClassTextField.WithSameWidth(topTextRowView),
                vehicleClassTextField.WithRelativeHeight(topTextRowView, 0.3f)
            );

            scrollView.AddIfNotNull(topTextRowView);
            scrollView.AddConstraints(
                topTextRowView.AtTopOf(scrollView),
                topTextRowView.WithSameWidth(scrollView),
                topTextRowView.AtLeftOf(scrollView),
                topTextRowView.AtRightOf(scrollView),
                topTextRowView.WithRelativeHeight(scrollView, 0.4f)
            );

            View.AddIfNotNull(topView, scrollView, picker);
            View.AddConstraints(
                topView.AtTopOf(View),
                topView.WithSameWidth(View),
                topView.WithRelativeHeight(View, 0.2f),

                scrollView.Below(topView, 30),
                scrollView.AtLeftOf(View, 30),
                scrollView.AtRightOf(View, 30),
                scrollView.WithRelativeHeight(View, 0.8f),

                picker.AtBottomOf(View),
                picker.AtLeftOf(View),
                picker.AtRightOf(View),
                picker.WithSameWidth(View),
                picker.WithRelativeHeight(View, 0.2f)
            );

            SignIn.SharedInstance.UIDelegate = this;
            EnableNextKeyForTextFields(licensePlateTextField.TextFieldWithValidator.TextField);
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

            var set = this.CreateBindingSet<LicensePlateView, LicenseViewModel>();
            set.Bind(backHomeView).To(vm => vm.BackToProfileCommand);
            set.Bind(states).To(vm => vm.States);
            //set.Bind(_trackingButton).For(x => x.ButtonImage).To(vm => vm.IsBound).
            //   WithConversion("GetPathToImage");
            //set.Bind(_profileButton).To(vm => vm.ProfileCommand);
            //set.Bind(_callCentergButton.ButtonText).To(vm => vm.SupportText);
            set.Apply();
        }
    }
}
