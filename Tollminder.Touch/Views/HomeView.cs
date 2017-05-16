using System;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using Tollminder.Core.Helpers;
using Tollminder.Core.ViewModels;
using Tollminder.Touch.Controllers;
using Tollminder.Touch.Controls;
using Tollminder.Touch.Extensions;
using Tollminder.Touch.Helpers;
using Tollminder.Touch.Interfaces;
using UIKit;
using Foundation;
using System.Diagnostics;
using MvvmCross.Binding.iOS.Views;
using Tollminder.Core.Converters;

namespace Tollminder.Touch.Views
{
    public class HomeView : BaseViewController<HomeViewModel>, ICleanBackStack
    {
        RoundedButton trackingButton;
        RoundedButton profileButton;
        RoundedButton payButton;
        RoundedButton payHistoryButton;
        RoundedButton callCentergButton;
        RoundedButton logoutButton;

        UIScrollView boardScrollView;
        UIView boardContainerView;
        UIView buttonContainerView;
        UIView roadInformationBoardView;

        // Information board
        BoardField activityLabel;
        BoardField geoLabel;
        BoardField geoLabelData;
        BoardField nextWaypointString;
        BoardField statusLabel;
        BoardField tollRoadString;

        // mock location
        UIButton nextLocatioButton;
        UITextField waypointActionsTextField;
        UIPickerView waypointActionsPicker;
        MvxPickerViewModel waypointActionsPickerViewModel;

        public new HomeViewModel ViewModel { get { return base.ViewModel as HomeViewModel; } }

        public HomeView()
        {
        }

        public HomeView(IntPtr handle) : base(handle)
        {
        }

        public HomeView(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        protected override void InitializeObjects()
        {
            base.InitializeObjects();

            // Navigation bar
            var applicationLogo = new UIImageView(UIImage.FromBundle(@"Images/logo.png"));
            View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile(@"Images/main_background.png").Scale(View.Frame.Size));
            applicationLogo.Frame = new CGRect(10, 10, applicationLogo.Image.CGImage.Width, applicationLogo.Image.CGImage.Height);
            logoutButton = RoundedButtonManager.ButtonInitiaziler("", UIImage.FromFile(@"Images/HomeView/ic_logout.png"));
            // Hide navigation bar
            NavigationController.SetNavigationBarHidden(true, false);

            var topView = new UIView();
            topView.AddIfNotNull(applicationLogo, logoutButton);
            topView.AddConstraints(
                applicationLogo.WithRelativeWidth(topView, 0.5f),
                applicationLogo.WithRelativeHeight(topView, 0.18f),
                applicationLogo.WithSameCenterX(topView),
                applicationLogo.WithSameCenterY(topView),

                logoutButton.AtTopOf(topView, 10),
                logoutButton.AtRightOf(topView),
                logoutButton.WithRelativeWidth(topView, 0.2f),
                logoutButton.WithRelativeHeight(topView, 0.4f)
            );

            // Central Board View
            boardScrollView = CreateSliderBoard(true);
            buttonContainerView = new UIView();
            roadInformationBoardView = new UIView();

            buttonContainerView.Frame = new CGRect(0, 0, (boardScrollView.Bounds.Width * EnvironmentInfo.GetValueForGettingWidthButtonsContainer),
                                                   (boardScrollView.Bounds.Height * 3));
            roadInformationBoardView.Frame = new CGRect((boardScrollView.Bounds.Width * EnvironmentInfo.GetValueForGettingWidthRoadInformationContainer),
                                                        EnvironmentInfo.GetValueForFirstSliderPositionY, (boardScrollView.Bounds.Width * 0.8), (boardScrollView.Bounds.Height * 2.5));
            boardScrollView.ContentSize = new CGSize((buttonContainerView.Bounds.Width + roadInformationBoardView.Bounds.Width), boardScrollView.Frame.Height);

            // Board View - Button Container
            profileButton = RoundedButtonManager.ButtonInitiaziler("PROFILE", UIImage.FromFile(@"Images/HomeView/ic_home_profile.png"), UIImage.FromFile(@"Images/HomeView/InformationBoard/ic_pointer.png"));
            payButton = RoundedButtonManager.ButtonInitiaziler("PAY", UIImage.FromFile(@"Images/HomeView/ic_home_pay.png"), UIImage.FromFile(@"Images/HomeView/InformationBoard/ic_pointer.png"));
            payHistoryButton = RoundedButtonManager.ButtonInitiaziler("PAY HISTORY", UIImage.FromFile(@"Images/HomeView/ic_home_pay_history.png"), UIImage.FromFile(@"Images/HomeView/InformationBoard/ic_pointer.png"));

            buttonContainerView.AddIfNotNull(profileButton, payButton, payHistoryButton);
            buttonContainerView.AddConstraints(
                profileButton.AtTopOf(buttonContainerView, EnvironmentInfo.GetMarginTopButtonsContainer),
                profileButton.AtLeftOf(buttonContainerView),
                profileButton.WithRelativeWidth(buttonContainerView, 0.4f),
                profileButton.WithRelativeHeight(buttonContainerView, 0.6f),

                payHistoryButton.AtTopOf(buttonContainerView, EnvironmentInfo.GetMarginTopButtonsContainer),
                payHistoryButton.WithSameCenterX(buttonContainerView),
                payHistoryButton.WithRelativeWidth(buttonContainerView, 0.4f),
                payHistoryButton.WithRelativeHeight(buttonContainerView, 0.6f),

                payButton.AtTopOf(buttonContainerView, EnvironmentInfo.GetMarginTopButtonsContainer),
                payButton.AtRightOf(buttonContainerView),
                payButton.WithRelativeWidth(buttonContainerView, 0.4f),
                payButton.WithRelativeHeight(buttonContainerView, 0.6f)
            );

            // Board View - Road Information Container
            nextWaypointString = BoardFieldInitializer(UIImage.FromFile(@"Images/HomeView/InformationBoard/ic_nearest_point.png"), "Nearest point in(ml):",
                                                       (roadInformationBoardView.Bounds.Width * EnvironmentInfo.GetDistanceBetweenLabelAndTextNearestPoint));
            geoLabelData = BoardFieldInitializer(UIImage.FromFile(@"Images/HomeView/InformationBoard/ic_location.png"), "Geolocation:",
                                                 (roadInformationBoardView.Bounds.Width * EnvironmentInfo.GetDistanceBetweenLabelAndTextGeolocation));
            tollRoadString = BoardFieldInitializer(UIImage.FromFile(@"Images/HomeView/InformationBoard/ic_tollroad.png"), "Tollroad:",
                                                   (roadInformationBoardView.Bounds.Width * EnvironmentInfo.GetDistanceBetweenLabelAndTextTollroad));
            statusLabel = BoardFieldInitializer(UIImage.FromFile(@"Images/HomeView/InformationBoard/ic_status.png"), "Status:",
                                                (roadInformationBoardView.Bounds.Width * EnvironmentInfo.GetDistanceBetweenLabelAndTextStatus));

            roadInformationBoardView.AddIfNotNull(nextWaypointString, geoLabelData, tollRoadString, statusLabel);
            roadInformationBoardView.AddConstraints(
                nextWaypointString.AtTopOf(roadInformationBoardView, 10),
                nextWaypointString.AtLeftOf(roadInformationBoardView, 10),
                nextWaypointString.WithSameWidth(roadInformationBoardView),
                nextWaypointString.WithRelativeHeight(roadInformationBoardView, 0.2f),

                geoLabelData.Below(nextWaypointString),
                geoLabelData.AtLeftOf(roadInformationBoardView, 10),
                geoLabelData.WithSameWidth(roadInformationBoardView),
                geoLabelData.WithRelativeHeight(roadInformationBoardView, 0.2f),

                tollRoadString.Below(geoLabelData),
                tollRoadString.AtLeftOf(roadInformationBoardView, 10),
                tollRoadString.WithSameWidth(roadInformationBoardView),
                tollRoadString.WithRelativeHeight(roadInformationBoardView, 0.2f),

                statusLabel.Below(tollRoadString),
                statusLabel.AtLeftOf(roadInformationBoardView, 10),
                statusLabel.WithSameWidth(roadInformationBoardView),
                statusLabel.WithRelativeHeight(roadInformationBoardView, 0.2f)
            );

            boardScrollView.AddSubviews(buttonContainerView, roadInformationBoardView);
            boardScrollView.Scrolled += (sender, e) =>
            {
                Debug.WriteLine(((UIScrollView)sender).ContentOffset.X);
            };

            // Slider container
            var applicationBoard = new UIImageView(UIImage.FromBundle(@"Images/HomeView/home_board.png"));
            applicationBoard.Frame = new CGRect(10, 10, applicationBoard.Image.CGImage.Width, applicationBoard.Image.CGImage.Height);
            boardContainerView = new UIView();
            boardContainerView.AddIfNotNull(applicationBoard, boardScrollView);
            boardContainerView.AddConstraints(
                applicationBoard.WithSameHeight(boardContainerView),
                applicationBoard.WithSameWidth(boardContainerView),
                applicationBoard.WithSameCenterX(boardContainerView),
                applicationBoard.WithSameCenterY(boardContainerView),

                boardScrollView.AtTopOf(boardContainerView, 10),
                boardScrollView.AtLeftOf(boardContainerView, 25),
                boardScrollView.AtRightOf(boardContainerView, 25),
                boardScrollView.WithRelativeHeight(boardContainerView, 0.55f)
            );

            // Bottom View
            trackingButton = RoundedButtonManager.ButtonInitiaziler(EnvironmentInfo.GetTrackingButtonDistanceBetweenTextAndImage);
            this.AddLinqBinding(ViewModel, vm => vm.TrackingCommand, (value) =>
            {
                trackingButton.BackgroundColor = UIColor.White;
                trackingButton.Alpha = 0.7f;
                trackingButton.ButtonTextColor = UIColor.FromRGB(3, 117, 27);
            });

            //var callCenterLabel = new UILabel();
            //_callCentergButton = ButtonInitiaziler(null, UIImage.FromFile(@"Images/ic_home_support.png"));
            //_callCentergButton.ButtonText.TextColor = UIColor.LightGray;
            //_callCentergButton.ButtonBackgroundColor = null;
            //callCenterLabel.Text = "+(1)305 335 85 08";
            //callCenterLabel.TextColor = UIColor.LightGray;

            var bottomView = new UIView();
            bottomView.AddIfNotNull(trackingButton);
            bottomView.AddConstraints(
                trackingButton.AtTopOf(bottomView),
                trackingButton.AtLeftOf(bottomView, 20),
                trackingButton.AtRightOf(bottomView, 20),
                trackingButton.WithRelativeHeight(bottomView, EnvironmentInfo.GetTrackingButtonHeight),
                trackingButton.WithRelativeWidth(bottomView, EnvironmentInfo.GetTrackingButtonWidth)
            );

            // View Initialising
            View.AddIfNotNull(topView, boardContainerView, bottomView);
            View.AddConstraints(
                topView.AtTopOf(View),
                topView.AtLeftOf(View),
                topView.AtRightOf(View),
                topView.WithRelativeHeight(View, 0.2f),

                boardContainerView.Below(topView),
                boardContainerView.AtLeftOf(View, 15),
                boardContainerView.AtRightOf(View, 15),
                boardContainerView.WithRelativeHeight(View, 0.43f),

                bottomView.Below(boardContainerView),
                bottomView.WithSameCenterX(topView),
                bottomView.WithRelativeHeight(View, 0.27f),
                bottomView.AtBottomOf(View, 30)
            );
            //#if DEBUG
            //            CreateWaypointPicker();
            //            nextLocatioButton = ButtonInitializer("Next Geo Location", UIControlState.Normal, UIColor.Black, UIColor.White, UIControlState.Normal, null);

            //            View.AddIfNotNull(waypointActionsTextField, nextLocatioButton);
            //            View.AddConstraints(
            //                waypointActionsTextField.WithSameCenterY(View),
            //                waypointActionsTextField.AtLeftOf(View, 10),
            //                waypointActionsTextField.Height().EqualTo(50),
            //                waypointActionsTextField.Width().EqualTo(100),

            //                nextLocatioButton.AtRightOf(View, 5),
            //                nextLocatioButton.WithSameCenterY(View),
            //                nextLocatioButton.Height().EqualTo(50),
            //                nextLocatioButton.Width().EqualTo(200)
            //            );
            //#endif
        }

        private void CreateWaypointPicker()
        {
            waypointActionsPicker = new UIPickerView();
            waypointActionsPickerViewModel = new MvxPickerViewModel(waypointActionsPicker);
            waypointActionsPicker.Model = waypointActionsPickerViewModel;
            waypointActionsPicker.ShowSelectionIndicator = true;
            waypointActionsPicker.BackgroundColor = UIColor.White;

            CreatePickerLabel();
            var gestureRecognizer = new UITapGestureRecognizer(() =>
            {
                waypointActionsTextField.ResignFirstResponder();
            });
            View.AddGestureRecognizer(gestureRecognizer);
            waypointActionsTextField.InputAccessoryView = new EnhancedToolbar(waypointActionsTextField, null, null);
            waypointActionsTextField.InputView = waypointActionsPicker;
        }

        private void CreatePickerLabel()
        {
            waypointActionsTextField = new UITextField();
            waypointActionsTextField.BackgroundColor = UIColor.White;
            waypointActionsTextField.TextColor = UIColor.Blue;
            waypointActionsTextField.Layer.CornerRadius = 10;
        }

        private UIButton ButtonInitializer(string title, UIControlState titleState, UIColor backgroundColor,
                                           UIColor titleColor, UIControlState colorTitleState, UIImage image, UIControlState imageState = UIControlState.Normal)
        {
            UIButton button = new UIButton();
            button.SetTitle(title, titleState);
            if (image != null)
                button.SetImage(image, imageState);
            button.BackgroundColor = backgroundColor;
            button.SetTitleColor(titleColor, colorTitleState);
            button.ClipsToBounds = false;
            button.Layer.CornerRadius = 10;
            button.Layer.ShadowColor = UIColor.Black.CGColor;
            button.Layer.ShadowOpacity = 0.1f;
            button.Layer.ShadowRadius = 1;
            button.Layer.ShadowOffset = new CGSize(1, 1);
            button.UserInteractionEnabled = true;
            return button;
        }

        private UIScrollView CreateSliderBoard(bool showWithPaging)
        {
            nfloat height = 50.0f;
            nfloat width = 50.0f;
            nfloat padding = 10.0f;
            nint n = 25;

            var scrollView = new UIScrollView
            {
                Frame = new CGRect(0, 100, View.Frame.Width, height + 2 * padding),
                ContentSize = new CGSize((width + padding) * n, height),
                AutoresizingMask = UIViewAutoresizing.FlexibleWidth
            };
            scrollView.PagingEnabled = showWithPaging;
            return scrollView;
        }

        private BoardField BoardFieldInitializer(UIImage icon, string labelText, nfloat distanceBetweenLabelAndValue, string valueText = null)
        {
            BoardField boardField = new BoardField(30, distanceBetweenLabelAndValue);
            boardField.FieldIcon = icon;
            boardField.LabelText.Text = labelText;
            boardField.LabelTextColor = UIColor.White;
            boardField.ValueText.Text = "dfgh";
            boardField.ValueText.Font = UIFont.BoldSystemFontOfSize(14f);
            boardField.ValueTextColor = UIColor.FromRGB(1, 94, 76);
            return boardField;
        }

        protected override void InitializeBindings()
        {
            base.InitializeBindings();
            //boardScrollView.ContentOffset = new CGPoint((boardScrollView.Bounds.Width * 0.76), 0);
            var set = this.CreateBindingSet<HomeView, HomeViewModel>();
            set.Bind(trackingButton).To(vm => vm.TrackingCommand);
            set.Bind(trackingButton.ButtonText).To(vm => vm.TrackingText);
            set.Bind(trackingButton).For(b => b.Enabled).To(vm => vm.IsBusy).WithConversion(new BoolInverseConverter());
            set.Bind(trackingButton).For(x => x.ButtonImage).To(vm => vm.IsBound).WithConversion("GetPathToImage");

            set.Bind(boardScrollView).For(x => x.ContentOffset).To(vm => vm.IsBound).WithConversion("GetSliderPage", boardScrollView);
            set.Bind(profileButton).To(vm => vm.ProfileCommand);
            set.Bind(payHistoryButton).To(vm => vm.PayHistoryCommand);
            set.Bind(payButton).To(vm => vm.PayCommand);
            set.Bind(logoutButton).To(vm => vm.LogoutCommand);

            // Information board
            set.Bind(geoLabelData.ValueText).To(v => v.Location).WithConversion("GeoLocation");
            set.Bind(statusLabel.ValueText).To(v => v.StatusString);
            set.Bind(tollRoadString.ValueText).To(v => v.TollRoadString);
            set.Bind(nextWaypointString.ValueText).To(v => v.WaypointChecker.DistanceToNearestTollpoint);
            //#if DEBUG       
            //            set.Bind(waypointActionsPickerViewModel).For(p => p.ItemsSource).To(vm => vm.WaypointActions);
            //            set.Bind(waypointActionsPickerViewModel).For(p => p.SelectedItem).To(vm => vm.SelectedWaypointAction);
            //            set.Bind(waypointActionsTextField).To(vm => vm.SelectedWaypointAction);
            //            set.Bind(nextLocatioButton).To(vm => vm.NextGeoLocationCommand);
            //            set.Bind(nextLocatioButton).For(b => b.Enabled).To(vm => vm.IsBusy).WithConversion(new BoolInverseConverter());
            //#endif
            set.Apply();
        }
    }
}
