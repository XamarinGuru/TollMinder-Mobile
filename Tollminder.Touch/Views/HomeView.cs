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
            View.BackgroundColor = UIColor.FromPatternImage(EnvironmentInfo.CheckDevice().Scale(View.Frame.Size));
            applicationLogo.Frame = new CoreGraphics.CGRect(10, 10, applicationLogo.Image.CGImage.Width, applicationLogo.Image.CGImage.Height);
            topView.AddIfNotNull(applicationLogo);
            topView.AddConstraints(
                applicationLogo.WithSameCenterX(topView),
                applicationLogo.WithSameCenterY(topView)
            );

            _trackingButton = ButtonInitiaziler();
            _profileButton = ButtonInitiaziler("Profile", UIImage.FromFile(@"Images/ic_home_profile.png"));
            _payButton = ButtonInitiaziler("Pay", UIImage.FromFile(@"Images/ic_home_pay.png"));
            _payHistoryButton = ButtonInitiaziler("Pay History", UIImage.FromFile(@"Images/ic_home_pay_history.png"));

            _callCentergButton = ButtonInitiaziler(null, UIImage.FromFile(@"Images/ic_home_support.png"));
            _callCentergButton.ButtonText.TextColor = UIColor.LightGray;
            _callCentergButton.ButtonBackgroundColor = null;
            callCenterLabel.Text = "+(1)305 335 85 08";
            callCenterLabel.TextColor = UIColor.LightGray;

            centerView.AddIfNotNull(_trackingButton, _profileButton, _payButton, _payHistoryButton);
            centerView.AddConstraints(
                _trackingButton.AtTopOf(centerView),
                _trackingButton.AtLeftOf(centerView, 8),
                _trackingButton.WithRelativeWidth(centerView, 0.45f),
                _trackingButton.WithRelativeHeight(centerView, 0.43f),

                _profileButton.AtTopOf(centerView),
                _profileButton.AtRightOf(centerView, 8),
                _profileButton.WithRelativeWidth(centerView, 0.45f),
                _profileButton.WithRelativeHeight(centerView, 0.43f),

                _payButton.Below(_trackingButton, 15),
                _payButton.AtLeftOf(centerView, 8),
                _payButton.WithRelativeWidth(centerView, 0.45f),
                _payButton.WithRelativeHeight(centerView, 0.43f),

                _payHistoryButton.Below(_profileButton, 15),
                _payHistoryButton.AtRightOf(centerView, 8),
                _payHistoryButton.WithRelativeWidth(centerView, 0.45f),
                _payHistoryButton.WithRelativeHeight(centerView, 0.43f)
            );

            bottomView.AddIfNotNull(_callCentergButton, callCenterLabel);
            bottomView.AddConstraints(
                _callCentergButton.AtTopOf(bottomView),
                _callCentergButton.AtLeftOf(bottomView, 20),
                _callCentergButton.AtRightOf(bottomView, 20),
                _callCentergButton.WithRelativeHeight(bottomView, 0.8f),
                _callCentergButton.WithRelativeWidth(bottomView, 0.78f),

                callCenterLabel.Below(_callCentergButton, -10),
                callCenterLabel.WithSameCenterX(bottomView),
                callCenterLabel.AtBottomOf(bottomView, 15)
            );

            View.AddIfNotNull(topView, centerView, bottomView);
            View.AddConstraints(
                topView.AtTopOf(View),
                topView.AtLeftOf(View),
                topView.AtRightOf(View),
                topView.WithRelativeHeight(View, 0.2f),

                centerView.Below(topView),
                centerView.AtLeftOf(View, 30),
                centerView.AtRightOf(View, 30),
                centerView.WithRelativeHeight(View, 0.5f),

                bottomView.Below(centerView),
                bottomView.WithSameCenterX(topView),
                bottomView.WithRelativeHeight(View, 0.25f),
                bottomView.AtBottomOf(View, 20)
            );

            SignIn.SharedInstance.UIDelegate = this;
        }

        private RoundedButton ButtonInitiaziler(string buttonText = null, UIImage buttonImage = null, int linesNumber = 0)
        {
            RoundedButton newButton = new RoundedButton();
            newButton.ButtonText.Text = buttonText;
            if(buttonImage !=null)
                newButton.ButtonImage = buttonImage;
            newButton.BackgroundColor = UIColor.White;
            return newButton;
        }

        protected override void InitializeBindings()
        {
            base.InitializeBindings();

            var set = this.CreateBindingSet<HomeView, HomeViewModel>();
            set.Bind(_trackingButton).To(vm => vm.TrackingCommand);
            set.Bind(_trackingButton.ButtonText).To(vm => vm.TrackingText);             set.Bind(_trackingButton).For(x => x.ButtonImage).To(vm => vm.IsBound).                WithConversion("GetPathToImage");
            set.Bind(_profileButton).To(vm => vm.ProfileCommand);
            set.Bind(_callCentergButton.ButtonText).To(vm => vm.SupportText);
            set.Apply();
        }
    }
}
