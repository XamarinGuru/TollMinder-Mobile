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
using Tollminder.Touch.Services;
using System.Drawing;

namespace Tollminder.Touch.Views
{
    public class HomeView : BaseViewController<HomeViewModel>, ISignInUIDelegate, ICleanBackStack
    {
        RoundedButton trackingButton;
        RoundedButton profileButton;
        RoundedButton payButton;
        RoundedButton payHistoryButton;
        RoundedButton callCentergButton;
        RoundedButton logoutButton;

        UIView buttonContainerView;
        UIView roadInformationContainerView;

        public new HomeViewModel ViewModel { get { return base.ViewModel as HomeViewModel; } }
        
        public HomeView()
        {
        }

        public HomeView(IntPtr handle) : base(handle)
        {
        }

        public HomeView(string nibName, Foundation.NSBundle bundle) : base(nibName, bundle)
        {
        }

        protected override void InitializeObjects()
        {
            base.InitializeObjects();

            var topView = new UIView();
            buttonContainerView = new UIView();
            roadInformationContainerView = new UIView();
            var bottomView = new UIView();
            var applicationLogo = new UIImageView(UIImage.FromBundle(@"Images/logo.png"));
            var callCenterLabel = new UILabel();
            var applicationBoard = new UIImageView(UIImage.FromBundle(@"Images/homeView/home_board.png"));

            // Hide navigation bar
            NavigationController.SetNavigationBarHidden(true, false);
            View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile(@"Images/main_background.png").Scale(View.Frame.Size));
            applicationLogo.Frame = new CoreGraphics.CGRect(10, 10, applicationLogo.Image.CGImage.Width, applicationLogo.Image.CGImage.Height);
            logoutButton = RoundedButtonManager.ButtonInitiaziler("", UIImage.FromFile(@"Images/homeView/ic_logout.png"));
            
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

            profileButton = RoundedButtonManager.ButtonInitiaziler("PROFILE", UIImage.FromFile(@"Images/homeView/ic_home_profile.png"));
            payButton = RoundedButtonManager.ButtonInitiaziler("PAY", UIImage.FromFile(@"Images/homeView/ic_home_pay.png"));
            payHistoryButton = RoundedButtonManager.ButtonInitiaziler("PAY HISTORY", UIImage.FromFile(@"Images/homeView/ic_home_pay_history.png"));
            trackingButton = RoundedButtonManager.ButtonInitiaziler(EnvironmentInfo.GetTrackingButtonDistanceBetweenTextAndImage);
            
            this.AddLinqBinding(ViewModel, vm => vm.TrackingCommand, (value) =>
            {
                trackingButton.BackgroundColor = UIColor.White;
                trackingButton.Alpha = 0.7f;
                trackingButton.ButtonTextColor = UIColor.FromRGB(3, 117, 27);
            });
            //_callCentergButton = ButtonInitiaziler(null, UIImage.FromFile(@"Images/ic_home_support.png"));
            //_callCentergButton.ButtonText.TextColor = UIColor.LightGray;
            //_callCentergButton.ButtonBackgroundColor = null;
            //callCenterLabel.Text = "+(1)305 335 85 08";
            //callCenterLabel.TextColor = UIColor.LightGray;

            buttonContainerView.Alpha = 1.0f;
            applicationBoard.Frame = new CoreGraphics.CGRect(10, 10, applicationBoard.Image.CGImage.Width, applicationBoard.Image.CGImage.Height);
            buttonContainerView.AddIfNotNull(applicationBoard, profileButton, payButton, payHistoryButton);
            buttonContainerView.AddConstraints(
                applicationBoard.WithSameHeight(buttonContainerView),
                applicationBoard.WithSameWidth(buttonContainerView),
                applicationBoard.WithSameCenterX(buttonContainerView),
                applicationBoard.WithSameCenterY(buttonContainerView),

                profileButton.AtTopOf(buttonContainerView, 10),
                profileButton.AtLeftOf(buttonContainerView, 8),
                profileButton.WithRelativeWidth(buttonContainerView, 0.4f),
                profileButton.WithRelativeHeight(buttonContainerView, 0.47f),

                payHistoryButton.AtTopOf(buttonContainerView, 10),
                payHistoryButton.WithSameCenterX(buttonContainerView),
                payHistoryButton.WithRelativeWidth(buttonContainerView, 0.4f),
                payHistoryButton.WithRelativeHeight(buttonContainerView, 0.47f),

                payButton.AtTopOf(buttonContainerView, 10),
                payButton.AtRightOf(buttonContainerView, 8),
                payButton.WithRelativeWidth(buttonContainerView, 0.4f),
                payButton.WithRelativeHeight(buttonContainerView, 0.47f)
            );
            
            bottomView.AddIfNotNull(trackingButton);
            bottomView.AddConstraints(
                trackingButton.AtTopOf(bottomView),
                trackingButton.AtLeftOf(bottomView, 20),
                trackingButton.AtRightOf(bottomView, 20),
                trackingButton.WithRelativeHeight(bottomView, EnvironmentInfo.GetTrackingButtonHeight),
                trackingButton.WithRelativeWidth(bottomView, EnvironmentInfo.GetTrackingButtonWidth)
            );
            var width = View.Frame.Width-30;
            buttonContainerView.AddConstraint(NSLayoutConstraint.Create(buttonContainerView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, 1f, width));

            View.AddIfNotNull(topView, buttonContainerView, bottomView);
            View.AddConstraints(
                topView.AtTopOf(View),
                topView.AtLeftOf(View),
                topView.AtRightOf(View),
                topView.WithRelativeHeight(View, 0.2f),

                buttonContainerView.Below(topView),
                buttonContainerView.AtLeftOf(View, 15),
                buttonContainerView.WithRelativeHeight(View, 0.43f),

                bottomView.Below(buttonContainerView),
                bottomView.WithSameCenterX(topView),
                bottomView.WithRelativeHeight(View, 0.27f),
                bottomView.AtBottomOf(View, 30)
            );
            SignIn.SharedInstance.UIDelegate = this;
        }

        protected override void InitializeBindings()
        {
             base.InitializeBindings();

            var set = this.CreateBindingSet<HomeView, HomeViewModel>();
            set.Bind(trackingButton).To(vm => vm.TrackingCommand);
            set.Bind(trackingButton.ButtonText).To(vm => vm.TrackingText);
            set.Bind(trackingButton).For(x => x.ButtonImage).To(vm => vm.IsBound).WithConversion("GetPathToImage");
            set.Bind(profileButton).To(vm => vm.ProfileCommand);
            set.Bind(payHistoryButton).To(vm => vm.PayHistoryCommand);
            set.Bind(logoutButton).To(vm => vm.LogoutCommand);

            set.Apply();
        }
    }
}
