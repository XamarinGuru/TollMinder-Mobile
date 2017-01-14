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
using MvvmCross.Binding.iOS.Views;

namespace Tollminder.Touch.Views
{
    public class LicensePlateView : BaseViewController<LicenseViewModel>, ISignInUIDelegate, ICleanBackStack
    {
        UIButton backHomeView;

        TextFieldValidationWithImage licensePlateTextField;
        TextFieldValidationWithImage stateTextField;
        TextFieldValidationWithImage vehicleClassTextField;

        LabelForDataWheel stateLabel;
        UIPickerView statesPicker;
        MvxPickerViewModel statesPickerViewModel;

        LabelForDataWheel vehicleClassLabel;
        UIPickerView vehicleClassesPicker;
        MvxPickerViewModel vehicleClassesPickerViewModel;
        UITextField text = new UITextField();
        
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
            stateTextField = TextFieldInitializer("State");
            vehicleClassTextField = TextFieldInitializer("Vehicle Class");

            stateLabel = LabelDataWheelInitiaziler("State");
            statesPicker = new UIPickerView();
            statesPickerViewModel = new MvxPickerViewModel(statesPicker);
            statesPicker.Model = statesPickerViewModel;
            statesPicker.Hidden = true;
            statesPicker.ShowSelectionIndicator = true;
            //statesPicker.Model = new PickerModel<StatesData>(stateLabel, statesList);
            statesPicker.BackgroundColor = UIColor.White;

            vehicleClassLabel = LabelDataWheelInitiaziler("Vehicle Class");
            vehicleClassesPicker = new UIPickerView();
            vehicleClassesPicker.Hidden = true;
            vehicleClassesPickerViewModel = new MvxPickerViewModel(vehicleClassesPicker);
            vehicleClassesPicker.Model = vehicleClassesPickerViewModel;
            vehicleClassesPicker.ShowSelectionIndicator = true;
            //vehicleClassesPicker.Model = new PickerModel<string>(vehicleClassLabel, vehicleClassList);
            vehicleClassesPicker.BackgroundColor = UIColor.White;

            topTextRowView.AddIfNotNull(licensePlateTextField, stateLabel, vehicleClassLabel);
            topTextRowView.AddConstraints(
                licensePlateTextField.AtTopOf(topTextRowView),
                licensePlateTextField.WithSameCenterX(topTextRowView),
                licensePlateTextField.WithSameWidth(topTextRowView),
                licensePlateTextField.WithRelativeHeight(topTextRowView, 0.3f),

                stateLabel.Below(licensePlateTextField, 10),
                stateLabel.WithSameCenterX(topTextRowView),
                stateLabel.WithSameWidth(topTextRowView),
                stateLabel.WithRelativeHeight(topTextRowView, 0.3f),

                vehicleClassLabel.Below(stateLabel, 10),
                vehicleClassLabel.WithSameCenterX(topTextRowView),
                vehicleClassLabel.WithSameWidth(topTextRowView),
                vehicleClassLabel.WithRelativeHeight(topTextRowView, 0.3f)
            );

            scrollView.AddIfNotNull(topTextRowView);
            scrollView.AddConstraints(
                topTextRowView.AtTopOf(scrollView),
                topTextRowView.WithSameWidth(scrollView),
                topTextRowView.AtLeftOf(scrollView),
                topTextRowView.AtRightOf(scrollView),
                topTextRowView.WithRelativeHeight(scrollView, 0.4f)
            );

            View.AddIfNotNull(topView, scrollView, statesPicker, vehicleClassesPicker);
            View.AddConstraints(
                topView.AtTopOf(View),
                topView.WithSameWidth(View),
                topView.WithRelativeHeight(View, 0.2f),

                scrollView.Below(topView, 30),
                scrollView.AtLeftOf(View, 30),
                scrollView.AtRightOf(View, 30),
                scrollView.WithRelativeHeight(View, 0.8f),

                statesPicker.AtBottomOf(View),
                statesPicker.AtLeftOf(View),
                statesPicker.AtRightOf(View),
                statesPicker.WithSameWidth(View),
                statesPicker.WithRelativeHeight(View, 0.2f),

                vehicleClassesPicker.AtBottomOf(View),
                vehicleClassesPicker.AtLeftOf(View),
                vehicleClassesPicker.AtRightOf(View),
                vehicleClassesPicker.WithSameWidth(View),
                vehicleClassesPicker.WithRelativeHeight(View, 0.2f)
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

        private LabelForDataWheel LabelDataWheelInitiaziler(string fieldName)
        {
            LabelForDataWheel labelWheel = new LabelForDataWheel();
            labelWheel.PlaceHolderText = fieldName;
            labelWheel.LabelTextColor = UIColor.Black;
            labelWheel.WheelTextColor = UIColor.Cyan;
            labelWheel.BackgroundColor = UIColor.White;
            labelWheel.Layer.CornerRadius = 10;
            return labelWheel;
        }

        protected override void InitializeBindings()
        {
            base.InitializeBindings();
            try
            {
                //stateLabel.WheelText.InputView = statesPicker;

                var set = this.CreateBindingSet<LicensePlateView, LicenseViewModel>();
                set.Bind(backHomeView).To(vm => vm.BackToProfileCommand);

                set.Bind(licensePlateTextField.TextFieldWithValidator.TextField).To(vm => vm.LicensePlate);
                set.Bind(statesPickerViewModel).For(p => p.ItemsSource).To(vm => vm.States);
                set.Bind(statesPickerViewModel).For(p => p.SelectedItem).To(vm => vm.SelectedState);
                set.Bind(stateLabel.WheelText).To(vm => vm.SelectedState);
                //set.Bind(stateLabel).For(x => x.Enabled).To(vm => vm.IsBusy).WithConversion(new BoolInverseConverter());
                set.Bind(statesPicker).For(x => x.Hidden).To(vm => vm.IsStateWheelHidden).WithConversion(new BoolInverseConverter());
                set.Bind(stateLabel).To(vm => vm.StatesWheelCommand);

                set.Bind(vehicleClassesPickerViewModel).For(p => p.ItemsSource).To(vm => vm.VehicleClasses);
                set.Bind(vehicleClassesPickerViewModel).For(p => p.SelectedItem).To(vm => vm.SelectedVehicleClass);
                set.Bind(vehicleClassLabel.WheelText).To(vm => vm.SelectedVehicleClass);
                set.Bind(vehicleClassLabel).For(x => x.Enabled).To(vm => vm.IsBusy).WithConversion(new BoolInverseConverter());
                set.Bind(vehicleClassesPicker).For(x => x.Hidden).To(vm => vm.IsVehicleClassWheelHidden).WithConversion(new BoolInverseConverter());
                set.Bind(vehicleClassLabel).To(vm => vm.VehicleClassesWheelCommand);
                
                set.Apply();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, ex.StackTrace);
            }
        }
    }
}
