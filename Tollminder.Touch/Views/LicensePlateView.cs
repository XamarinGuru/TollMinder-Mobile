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
        UILabel nameOfPageLabel;
        UILabel informationAboutPageLabel;

        TextFieldValidationWithImage licensePlateTextField;
        TextFieldValidationWithImage stateTextField;
        TextFieldValidationWithImage vehicleClassTextField;

        LabelForDataWheel stateLabel;
        UIPickerView statesPicker;
        MvxPickerViewModel statesPickerViewModel;

        LabelForDataWheel vehicleClassLabel;
        UIPickerView vehicleClassesPicker;
        MvxPickerViewModel vehicleClassesPickerViewModel;
        
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
            nameOfPageLabel = LabelInformationAboutPage(UIColor.White, "License Information", UIFont.BoldSystemFontOfSize(16f));
            informationAboutPageLabel = LabelInformationAboutPage(UIColor.FromRGB(29, 157, 189), "Please, Enter the License Plate Number and Other Information for Your Vehicle.", UIFont.FromName("Helvetica", 14f));

            // Hide navigation bar
            NavigationController.SetNavigationBarHidden(true, false);
            View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile(@"Images/tab_background.png").Scale(View.Frame.Size));//EnvironmentInfo.CheckDevice().Scale(View.Frame.Size));
            profileNavigationBarBackground.Frame = new CoreGraphics.CGRect(10, 10, profileNavigationBarBackground.Image.CGImage.Width, profileNavigationBarBackground.Image.CGImage.Height);

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
                informationAboutPageLabel.WithRelativeHeight(labelView, 0.6f)
            );

            topView.AddIfNotNull(profileNavigationBarBackground, backHomeView, labelView);
            topView.AddConstraints(
                profileNavigationBarBackground.WithSameWidth(topView),
                profileNavigationBarBackground.WithSameHeight(topView),
                profileNavigationBarBackground.AtTopOf(topView),

                backHomeView.WithSameCenterY(topView),
                backHomeView.AtLeftOf(topView, 20),
                backHomeView.WithRelativeWidth(topView,0.1f),
                backHomeView.WithRelativeHeight(topView, 0.2f),

                labelView.WithSameCenterX(topView),
                labelView.WithSameCenterY(topView),
                labelView.WithRelativeWidth(topView, 0.8f),
                labelView.WithRelativeHeight(topView, 0.6f)
            );

            licensePlateTextField = TextFieldInitializer("LicensePlate");
            stateTextField = TextFieldInitializer("State");
            vehicleClassTextField = TextFieldInitializer("Vehicle Class");

            stateLabel = LabelDataWheelInitiaziler("State");
            statesPicker = PickerInitializer();
            statesPickerViewModel = new MvxPickerViewModel(statesPicker);
            statesPicker.Model = statesPickerViewModel;

            vehicleClassLabel = LabelDataWheelInitiaziler("Vehicle Class");
            vehicleClassesPicker = PickerInitializer();
            vehicleClassesPickerViewModel = new MvxPickerViewModel(vehicleClassesPicker);
            vehicleClassesPicker.Model = vehicleClassesPickerViewModel;

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

        private UILabel LabelInformationAboutPage(UIColor color, string text, UIFont font)
        {
            var labelInformation = new UILabel();
            labelInformation.TextColor = color;
            labelInformation.Text = text;
            labelInformation.Font = font;
            labelInformation.TextAlignment = UITextAlignment.Center;
            labelInformation.LineBreakMode = UILineBreakMode.WordWrap;
            labelInformation.Lines = 0;
            return labelInformation;
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

        private UIPickerView PickerInitializer()
        {
            var picker = new UIPickerView();
            picker.Hidden = true;
            picker.ShowSelectionIndicator = true;
            picker.BackgroundColor = UIColor.White;
            return picker;
        }

        protected override void InitializeBindings()
        {
            base.InitializeBindings();
            try
            {
                var set = this.CreateBindingSet<LicensePlateView, LicenseViewModel>();
                set.Bind(backHomeView).To(vm => vm.BackToProfileCommand);

                set.Bind(licensePlateTextField.TextFieldWithValidator.TextField).To(vm => vm.DriverLicense.LicensePlate);
                set.Bind(statesPickerViewModel).For(p => p.ItemsSource).To(vm => vm.States);
                set.Bind(statesPickerViewModel).For(p => p.SelectedItem).To(vm => vm.SelectedState);
                set.Bind(stateLabel.WheelText).To(vm => vm.SelectedState);
                set.Bind(statesPicker).For(x => x.Hidden).To(vm => vm.IsStateWheelHidden).WithConversion(new BoolInverseConverter());
                set.Bind(stateLabel).To(vm => vm.StatesWheelCommand);

                set.Bind(vehicleClassesPickerViewModel).For(p => p.ItemsSource).To(vm => vm.VehicleClasses);
                set.Bind(vehicleClassesPickerViewModel).For(p => p.SelectedItem).To(vm => vm.SelectedVehicleClass);
                set.Bind(vehicleClassLabel.WheelText).To(vm => vm.SelectedVehicleClass);
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
