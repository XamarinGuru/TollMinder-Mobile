using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using Tollminder.Core.ViewModels;
using Tollminder.Core.Helpers;
using Foundation;
using Tollminder.Touch.Interfaces;
using Tollminder.Core.Converters;
using Tollminder.Touch.Controls;
using Google.SignIn;
using UIKit;
using Cirrious.FluentLayouts.Touch;
using Tollminder.Touch.Helpers;
using Tollminder.Touch.Extensions;
using Tollminder.Touch.Controllers;
using System;
using System.Diagnostics;

namespace Tollminder.Touch.Views
{
	public partial class HomeTestView : MvxViewController, ICleanBackStack, ISignInUIDelegate
	{	
        RoundedButton _trackingButton;
        RoundedButton _profileButton;
        RoundedButton _payButton;
        RoundedButton _payHistoryButton;
        RoundedButton _callCentergButton;

        //HomeTestViewModel _viewModel;
        #pragma warning disable 108     
        public new HomeTestViewModel ViewModel { get { return base.ViewModel as HomeTestViewModel; } }
#pragma warning restore 108

        public HomeTestView()
        {
        }

        //public HomeTestView(IntPtr handle) : base(handle)
        //{
        //}

        //public HomeTestView(string nibName, Foundation.NSBundle bundle) : base(nibName, bundle)
        //{
        //}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var topView = new UIView();
            var centerView = new UIView();
            var bottomView = new UIView();
            var applicationLogo = new UIImageView(UIImage.FromBundle(@"Images/home_logo.png"));
            var callCenterLabel = new UILabel();
            var applicationBoard = new UIImageView(UIImage.FromBundle(@"Images/home_board.png"));
         
            // Hide navigation bar
            NavigationController.SetNavigationBarHidden(true, false);
            View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile(@"Images/home_background.png").Scale(View.Frame.Size));//EnvironmentInfo.CheckDevice().Scale(View.Frame.Size));
            applicationLogo.Frame = new CoreGraphics.CGRect(10, 10, applicationLogo.Image.CGImage.Width, applicationLogo.Image.CGImage.Height);
            topView.AddIfNotNull(applicationLogo);
            topView.AddConstraints(
                applicationLogo.WithRelativeWidth(topView, 0.5f),
                applicationLogo.WithRelativeHeight(topView, 0.18f),
                applicationLogo.WithSameCenterX(topView),
                applicationLogo.WithSameCenterY(topView)
            );
            //var cell = new UITableViewCell(UITableViewCellStyle.Value2, cellIdentifier);
          
            _profileButton = RoundedButtonManager.ButtonInitiaziler("PROFILE", UIImage.FromFile(@"Images/ic_home_profile.png"));
            _payButton = RoundedButtonManager.ButtonInitiaziler("PAY", UIImage.FromFile(@"Images/ic_home_pay.png"));
            _payHistoryButton = RoundedButtonManager.ButtonInitiaziler("PAY HISTORY", UIImage.FromFile(@"Images/ic_home_pay_history.png"));
            _trackingButton = RoundedButtonManager.ButtonInitiaziler(EnvironmentInfo.GetTrackingButtonDistanceBetweenTextAndImage);

            applicationBoard.Frame = new CoreGraphics.CGRect(10, 10, applicationBoard.Image.CGImage.Width, applicationBoard.Image.CGImage.Height);
            centerView.AddIfNotNull(applicationBoard, _profileButton, _payButton, _payHistoryButton);
            centerView.AddConstraints(
                applicationBoard.WithSameHeight(centerView),
                applicationBoard.WithSameWidth(centerView),
                applicationBoard.WithSameCenterX(centerView),
                applicationBoard.WithSameCenterY(centerView),

                _profileButton.AtTopOf(centerView, 10),
                _profileButton.AtLeftOf(centerView, 8),
                _profileButton.WithRelativeWidth(centerView, 0.4f),
                _profileButton.WithRelativeHeight(centerView, 0.47f),

                _payHistoryButton.AtTopOf(centerView, 10),
                _payHistoryButton.WithSameCenterX(centerView),
                _payHistoryButton.WithRelativeWidth(centerView, 0.4f),
                _payHistoryButton.WithRelativeHeight(centerView, 0.47f),

                _payButton.AtTopOf(centerView, 10),
                _payButton.AtRightOf(centerView, 8),
                _payButton.WithRelativeWidth(centerView, 0.4f),
                _payButton.WithRelativeHeight(centerView, 0.47f)
            );

            bottomView.AddIfNotNull(_trackingButton);
            bottomView.AddConstraints(
                _trackingButton.AtTopOf(bottomView),
                _trackingButton.AtLeftOf(bottomView, 20),
                _trackingButton.AtRightOf(bottomView, 20),
                _trackingButton.WithRelativeHeight(bottomView, EnvironmentInfo.GetTrackingButtonHeight),
                _trackingButton.WithRelativeWidth(bottomView, EnvironmentInfo.GetTrackingButtonWidth)
            );

            View.AddIfNotNull(topView, centerView, bottomView);
            View.AddConstraints(
                topView.AtTopOf(View),
                topView.AtLeftOf(View),
                topView.AtRightOf(View),
                topView.WithRelativeHeight(View, 0.2f),

                centerView.Below(topView),
                centerView.AtLeftOf(View, 15),
                centerView.AtRightOf(View, 15),
                centerView.WithRelativeHeight(View, 0.43f),

                bottomView.Below(centerView),
                bottomView.WithSameCenterX(topView),
                bottomView.WithRelativeHeight(View, 0.27f),
                bottomView.AtBottomOf(View, 30)
            );

            SignIn.SharedInstance.UIDelegate = this;
            InitializeBindings();
        }

        protected  void InitializeBindings()
        {
            //base.InitializeBindings();

            NavigationController.NavigationBar.Translucent = false;
            //LogArea.Font = UIKit.UIFont.FromName ("Helvetica", 12f);
            AutomaticallyAdjustsScrollViewInsets = true;

            var set = this.CreateBindingSet<HomeTestView, HomeTestViewModel>();
            set.Bind(_trackingButton).To(vm => vm.TrackingCommand);
            set.Bind(_trackingButton.ButtonText).To(vm => vm.TrackingText);
            set.Bind(_trackingButton).For(x => x.ButtonImage).To(vm => vm.IsBound).
               WithConversion("GetPathToImage");
            set.Bind(_profileButton).To(vm => vm.ProfileCommand);
            set.Bind(GeoLabelData).To(v => v.LocationString);
            set.Bind(ActivityLabel).To(v => v.MotionTypeString);
            set.Bind(StartButton).For(x => x.Enabled).To(x => x.IsBound).WithConversion(new BoolInverseConverter());
            set.Bind(StopButton).For(x => x.Enabled).To(x => x.IsBound);
            set.Bind(DistanceToNearestPoint).To(v => v.Distance);
            set.Bind(StatusLabel).To(v => v.StatusString);
            set.Bind(TollRoadString).To(v => v.TollRoadString);
            set.Bind(NextTollpointString).To(v => v.NearestTollpointsString);
            set.Apply();

   //         try
   //         {
   //             this.AddLinqBinding(ViewModel, vm => vm.NearestTollpointsString, (value) =>
   //             {
   //                 NSRange bottom = new NSRange(0, value?.Length ?? 0);
   //                 NextTollpointString.ScrollRangeToVisible(bottom);
			//});
   //         }
   //         catch(Exception ex)
   //         {
   //             Debug.WriteLine("Message: {0} StackTrace: {1}", ex.Message, ex.StackTrace);
   //         }
        }
	}
}


