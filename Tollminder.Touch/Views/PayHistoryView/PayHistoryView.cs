using System;
using Cirrious.FluentLayouts.Touch;
using MvvmCross.Binding.BindingContext;
using Tollminder.Touch.Controllers;
using Tollminder.Touch.Controls;
using Tollminder.Touch.Extensions;
using Tollminder.Touch.Helpers;
using Tollminder.Touch.Interfaces;
using UIKit;
using MvvmCross.Binding.iOS.Views;
using Tollminder.Core.ViewModels.Payments;
using CoreGraphics;
using Tollminder.Core.Converters;
using Tollminder.Touch.Views.PaymentViews;

namespace Tollminder.Touch.Views
{
    public class PayHistoryView : BaseViewController<PayHistoryViewModel>, ICleanBackStack
    {
        UIButton backHomeView;
        UITableView payHistoryTableView;
        ProfileButton dowloadHistoryButton;
        UILabel informationLabel;
        UIView scrollView;
        UIActivityIndicatorView activityIndicatorView;
        private MvxSimpleTableViewSource payHistoryViewSource;

        public PayHistoryView()
        {
        }

        public PayHistoryView(IntPtr handle) : base(handle)
        {
        }

        public PayHistoryView(string nibName, Foundation.NSBundle bundle) : base(nibName, bundle)
        {
        }

        protected override void InitializeObjects()
        {
            base.InitializeObjects();

            var topView = new UIView();
            scrollView = new UIView();
            var bottomView = new UIView();
            payHistoryTableView = new UITableView();

            informationLabel = new UILabel();
            informationLabel.TextColor = UIColor.White;
            informationLabel.Text = "Payment History";
            informationLabel.Font = UIFont.BoldSystemFontOfSize(16f);
            informationLabel.TextAlignment = UITextAlignment.Center;

            backHomeView = UIButton.FromType(UIButtonType.Custom);
            backHomeView.SetImage(UIImage.FromFile(@"Images/ic_back.png"), UIControlState.Normal);
            var profileNavigationBarBackground = new UIImageView(UIImage.FromBundle(@"Images/navigation_bar_background.png"));

            // Hide navigation bar
            NavigationController.SetNavigationBarHidden(true, false);
            View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile(@"Images/tab_background.png").Scale(View.Frame.Size));//EnvironmentInfo.CheckDevice().Scale(View.Frame.Size));
            profileNavigationBarBackground.Frame = new CoreGraphics.CGRect(10, 10, profileNavigationBarBackground.Image.CGImage.Width, profileNavigationBarBackground.Image.CGImage.Height);

            topView.AddIfNotNull(profileNavigationBarBackground, backHomeView, informationLabel);
            topView.AddConstraints(
                profileNavigationBarBackground.WithSameWidth(topView),
                profileNavigationBarBackground.WithSameHeight(topView),
                profileNavigationBarBackground.AtTopOf(topView),

                backHomeView.WithSameCenterY(topView),
                backHomeView.AtLeftOf(topView, 20),
                backHomeView.WithRelativeWidth(topView, 0.1f),
                backHomeView.WithRelativeHeight(topView, 0.2f),

                informationLabel.WithSameCenterY(topView),
                informationLabel.WithSameCenterX(topView),
                informationLabel.WithSameWidth(topView),
                informationLabel.WithRelativeHeight(topView, 0.3f)
            );

            dowloadHistoryButton = ProfileButtonManager.ButtonInitiaziler("Download History", UIImage.FromFile(@"Images/ProfileView/ic_license.png"));

            payHistoryViewSource = new MvxSimpleTableViewSource(payHistoryTableView, NotPayedTripsTableViewCell.Key, NotPayedTripsTableViewCell.Key);
            payHistoryTableView.Source = payHistoryViewSource;
            payHistoryTableView.BackgroundColor = UIColor.Clear;
            payHistoryTableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            payHistoryTableView.EstimatedRowHeight = 90f;
            payHistoryTableView.RowHeight = UITableView.AutomaticDimension;

            bottomView.AddIfNotNull(dowloadHistoryButton, payHistoryTableView);
            bottomView.AddConstraints(
                dowloadHistoryButton.AtTopOf(bottomView),
                dowloadHistoryButton.WithSameCenterX(bottomView),
                dowloadHistoryButton.WithSameWidth(bottomView),
                dowloadHistoryButton.WithRelativeHeight(bottomView, 0.1f),

                payHistoryTableView.Below(dowloadHistoryButton),
                payHistoryTableView.WithSameCenterX(bottomView),
                payHistoryTableView.WithSameWidth(bottomView),
                payHistoryTableView.WithRelativeHeight(bottomView, 1)
            );

            scrollView.AddIfNotNull(bottomView);
            scrollView.AddConstraints(
                bottomView.AtTopOf(scrollView),
                bottomView.WithSameWidth(scrollView),
                bottomView.AtLeftOf(scrollView),
                bottomView.AtRightOf(scrollView),
                bottomView.AtBottomOf(scrollView),
                bottomView.WithRelativeHeight(scrollView, 0.9f)
            );

            View.AddIfNotNull(topView, scrollView);
            View.AddConstraints(
                topView.AtTopOf(View),
                topView.WithSameWidth(View),
                topView.WithRelativeHeight(View, 0.2f),

                scrollView.Below(topView, 1),
                scrollView.AtLeftOf(View, 30),
                scrollView.AtRightOf(View, 30),
                scrollView.WithRelativeHeight(View, 0.8f)
            );
            AddLoader();
        }

        protected override void InitializeBindings()
        {
            base.InitializeBindings();

            var set = this.CreateBindingSet<PayHistoryView, PayHistoryViewModel>();
            set.Bind(backHomeView).To(vm => vm.BackHomeCommand);
            set.Bind(dowloadHistoryButton).To(vm => vm.DownloadHistoryCommand);
            set.Bind(activityIndicatorView).For(x => x.Hidden).To(vm => vm.IsBusy).WithConversion(new BoolInverseConverter());
            set.Bind(payHistoryViewSource).To(vm => vm.History);
            set.Apply();
            payHistoryTableView.ReloadData();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        private void AddLoader()
        {
            activityIndicatorView = (UIActivityIndicatorView)View.ViewWithTag(1000);

            // show busy indicator. create it first if it doesn't already exists
            if (activityIndicatorView == null)
            {
                var s = scrollView.Bounds;
                activityIndicatorView = new UIActivityIndicatorView()
                {
                    ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.Gray,
                    Tag = 1000
                };
                activityIndicatorView.BackgroundColor = UIColor.White;
                activityIndicatorView.Alpha = 0.7f;
                scrollView.AddIfNotNull(activityIndicatorView);
                scrollView.AddConstraints(
                    activityIndicatorView.AtTopOf(scrollView),
                    activityIndicatorView.AtLeftOf(scrollView),
                    activityIndicatorView.AtRightOf(scrollView),
                    activityIndicatorView.AtBottomOf(scrollView)
                );
                scrollView.BringSubviewToFront(activityIndicatorView);
                activityIndicatorView.StartAnimating();
            }
        }
    }
}
