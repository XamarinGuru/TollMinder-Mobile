using System;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Google.SignIn;
using MvvmCross.Binding.BindingContext;
using Tollminder.Core;
using Tollminder.Core.Converters;
using Tollminder.Core.ViewModels;
using Tollminder.Touch.Controllers;
using Tollminder.Touch.Controls;
using Tollminder.Touch.Extensions;
using Tollminder.Touch.Interfaces;
using UIKit;

namespace Tollminder.Touch.Views
{
    public class HomeView : BaseViewController<HomeViewModel>, ISignInUIDelegate, ICleanBackStack
    {
        RoundedButton _trackingButton;
        RoundedButton _profileButton;
        RoundedButton _payButton;
        RoundedButton _payHistoryButton;
        RoundedButton _callCentergButton;

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
            var centerView = new UIView();
            var bottomView = new UIView();
            var applicationLogo = new UIImageView(UIImage.FromBundle(@"Images/home_logo.png"));
            var callCenterLabel = new UILabel();

            // Hide navigation bar
            NavigationController.SetNavigationBarHidden(true, false);
            View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile(@"Images/home_background.png"));
            applicationLogo.Frame = new CoreGraphics.CGRect(10, 10, applicationLogo.Image.CGImage.Width, applicationLogo.Image.CGImage.Height);
            topView.AddIfNotNull(applicationLogo);
            topView.AddConstraints(
                applicationLogo.WithSameCenterX(topView),
                applicationLogo.WithSameCenterY(topView)
            );

            _trackingButton = ButtonInitiaziler("Tracking is Off", UIImage.FromFile(@"Images/ic_home_tracking_default.png"));
            _profileButton = ButtonInitiaziler("Profile", UIImage.FromFile(@"Images/ic_home_profile.png"));
            _payButton = ButtonInitiaziler("Pay", UIImage.FromFile(@"Images/ic_home_pay.png"));
            _payHistoryButton = ButtonInitiaziler("Pay History", UIImage.FromFile(@"Images/ic_home_pay_history.png"));
            _callCentergButton = ButtonInitiaziler(null, UIImage.FromFile(@"Images/ic_home_support.png"));
            //_callCentergButton.ButtonTextColor = UIColor.LightGray;
            callCenterLabel.Text = "Call Center:\n+(1)305 335 85 08";
            callCenterLabel.TextColor = UIColor.LightGray;
            _callCentergButton.ButtonBackgroundColor = null;

            centerView.AddIfNotNull(_trackingButton, _profileButton, _profileButton, _payButton, _payHistoryButton);
            centerView.AddConstraints(
                _trackingButton.AtTopOf(centerView),
                _trackingButton.AtLeftOf(centerView, 5),
                _trackingButton.AtRightOf(_profileButton, 130),
                _trackingButton.Height().EqualTo(120),
                _trackingButton.Width().EqualTo(120),

                _profileButton.AtTopOf(centerView),
                _profileButton.AtLeftOf(_trackingButton, 130),
                _profileButton.AtRightOf(centerView, 15),
                _profileButton.Height().EqualTo(120),
                _profileButton.Width().EqualTo(120),

                _payButton.Below(_trackingButton, 10),
                _payButton.AtLeftOf(centerView, 5),
                _payButton.AtRightOf(_payHistoryButton, 130),
                _payButton.Height().EqualTo(120),
                _payButton.Width().EqualTo(120),

                _payHistoryButton.Below(_profileButton, 10),
                _payHistoryButton.AtLeftOf(_payButton, 130),
                _payHistoryButton.AtRightOf(centerView, 15),
                _payHistoryButton.Height().EqualTo(120),
                _payHistoryButton.Width().EqualTo(120)
            );

            bottomView.AddIfNotNull(_callCentergButton, callCenterLabel);
            bottomView.AddConstraints(
                _callCentergButton.AtTopOf(bottomView),
                _callCentergButton.AtLeftOf(bottomView, 20),
                _callCentergButton.AtRightOf(bottomView, 20),
                _callCentergButton.Height().EqualTo(100),
                _callCentergButton.Width().EqualTo(120),

                callCenterLabel.Below(_callCentergButton),
                callCenterLabel.AtLeftOf(bottomView, 40),
                callCenterLabel.AtRightOf(bottomView, 40),
                callCenterLabel.Height().EqualTo(40)
            );

            View.AddIfNotNull(topView, centerView, bottomView);
            View.AddConstraints(
                topView.AtTopOf(View, -20),
                topView.AtLeftOf(View),
                topView.AtRightOf(View),
                topView.WithRelativeHeight(View, 0.3f),

                centerView.Below(topView, -20),
                centerView.AtLeftOf(View, 30),
                centerView.AtRightOf(View, 30),

                bottomView.Below(centerView, 280),
                bottomView.AtLeftOf(View, 80),
                bottomView.AtRightOf(View, 30)
            );

            SignIn.SharedInstance.UIDelegate = this;
        }

        private RoundedButton ButtonInitiaziler(string buttonText, UIImage buttonImage)
        {
            RoundedButton newButton = new RoundedButton();
            newButton.ButtonText = buttonText;
            newButton.ButtonImage = buttonImage;
            newButton.BackgroundColor = UIColor.White;
            return newButton;
        }

        protected override void InitializeBindings()
        {
            base.InitializeBindings();

            var set = this.CreateBindingSet<HomeView, HomeViewModel>();
            set.Bind(_trackingButton).To(vm => vm.TrackingCommand);
            set.Bind(_profileButton).To(vm => vm.ProfileCommand);
            set.Apply();
        }
    }
}
