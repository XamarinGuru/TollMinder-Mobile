using System;
using Cirrious.FluentLayouts.Touch;
using MvvmCross.Binding.BindingContext;
using Tollminder.Touch.Controllers;
using Tollminder.Touch.Controls;
using Tollminder.Touch.Extensions;
using Tollminder.Touch.Interfaces;
using UIKit;
using System.Diagnostics;
using MvvmCross.Binding.iOS.Views;
using Tollminder.Core.ViewModels.Vehicles;

namespace Tollminder.Touch.Views
{
    public class LicensePlateView : BaseViewController<LicenseViewModel>, ICleanBackStack
    {
        UIButton backHomeView;
        UILabel nameOfPageLabel;
        UILabel informationAboutPageLabel;

        TextFieldValidationWithImage licensePlateTextField;

        TextFieldValidationWithImage stateTextField;
        UIPickerView statesPicker;
        MvxPickerViewModel statesPickerViewModel;

        TextFieldValidationWithImage vehicleClassTextField;
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
                backHomeView.WithRelativeWidth(topView, 0.1f),
                backHomeView.WithRelativeHeight(topView, 0.2f),

                labelView.WithSameCenterX(topView),
                labelView.WithSameCenterY(topView),
                labelView.WithRelativeWidth(topView, 0.8f),
                labelView.WithRelativeHeight(topView, 0.6f)
            );

            licensePlateTextField = TextFieldInitializer("LicensePlate");

            stateTextField = TextFieldInitializer("State");
            statesPicker = PickerInitializer();
            statesPickerViewModel = new MvxPickerViewModel(statesPicker);
            statesPicker.Model = statesPickerViewModel;

            vehicleClassTextField = TextFieldInitializer("Vehicle Class");
            vehicleClassesPicker = PickerInitializer();
            vehicleClassesPickerViewModel = new MvxPickerViewModel(vehicleClassesPicker);
            vehicleClassesPicker.Model = vehicleClassesPickerViewModel;

            topTextRowView.AddIfNotNull(licensePlateTextField, stateTextField, vehicleClassTextField);
            topTextRowView.AddConstraints(
                licensePlateTextField.AtTopOf(topTextRowView),
                licensePlateTextField.WithSameCenterX(topTextRowView),
                licensePlateTextField.WithSameWidth(topTextRowView),
                licensePlateTextField.WithRelativeHeight(topTextRowView, 0.3f),

                stateTextField.Below(licensePlateTextField, 10),
                stateTextField.WithSameCenterX(topTextRowView),
                stateTextField.WithSameWidth(topTextRowView),
                stateTextField.WithRelativeHeight(topTextRowView, 0.3f),

                vehicleClassTextField.Below(stateTextField, 10),
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

            EnableNextKeyForTextFields(licensePlateTextField.TextFieldWithValidator.TextField, stateTextField.TextFieldWithValidator.TextField, vehicleClassTextField.TextFieldWithValidator.TextField);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var gestureRecognizer = new UITapGestureRecognizer(() =>
            {
                licensePlateTextField.TextFieldWithValidator.TextField.ResignFirstResponder();
                stateTextField.TextFieldWithValidator.TextField.ResignFirstResponder();
                vehicleClassTextField.TextFieldWithValidator.TextField.ResignFirstResponder();
            });
            View.AddGestureRecognizer(gestureRecognizer);
            AddDoneButtonOnKeyBoard();
            RegisterForKeyboardNotifications();
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

        private UIPickerView PickerInitializer()
        {
            var picker = new UIPickerView();
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
                set.Bind(stateTextField.TextFieldWithValidator.TextField).To(vm => vm.SelectedState);

                set.Bind(vehicleClassesPickerViewModel).For(p => p.ItemsSource).To(vm => vm.VehicleClasses);
                set.Bind(vehicleClassesPickerViewModel).For(p => p.SelectedItem).To(vm => vm.SelectedVehicleClass);
                set.Bind(vehicleClassTextField.TextFieldWithValidator.TextField).To(vm => vm.SelectedVehicleClass);

                set.Apply();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, ex.StackTrace);
            }
        }

        void AddDoneButtonOnKeyBoard()
        {
            licensePlateTextField.TextFieldWithValidator.TextField.InputAccessoryView = new EnhancedToolbar(licensePlateTextField.TextFieldWithValidator.TextField,
                                                                                                            null, stateTextField.TextFieldWithValidator.TextField);
            stateTextField.TextFieldWithValidator.TextField.InputAccessoryView = new EnhancedToolbar(stateTextField.TextFieldWithValidator.TextField,
                                                                                                     licensePlateTextField.TextFieldWithValidator.TextField,
                                                                                                     vehicleClassTextField.TextFieldWithValidator.TextField);
            stateTextField.TextFieldWithValidator.TextField.InputView = statesPicker;

            vehicleClassTextField.TextFieldWithValidator.TextField.InputAccessoryView = new EnhancedToolbar(vehicleClassTextField.TextFieldWithValidator.TextField,
                                                                                                            stateTextField.TextFieldWithValidator.TextField, null);

            vehicleClassTextField.TextFieldWithValidator.TextField.InputView = vehicleClassesPicker;
        }
    }
}
