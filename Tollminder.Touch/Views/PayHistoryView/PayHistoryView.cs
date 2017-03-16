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

namespace Tollminder.Touch.Views
{
    public class PayHistoryView : BaseViewController<PayHistoryViewModel>, ICleanBackStack
    {
        UIButton backHomeView;
        UITableView tableView;
        ProfileButton dowloadHistoryButton;
        UILabel informationLabel;

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
            var scrollView = new UIView();
            var bottomView = new UIView();

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
            tableView = new UITableView();

            bottomView.AddIfNotNull(dowloadHistoryButton, tableView);
            bottomView.AddConstraints(
                dowloadHistoryButton.AtTopOf(bottomView),
                dowloadHistoryButton.WithSameCenterX(bottomView),
                dowloadHistoryButton.WithSameWidth(bottomView),
                dowloadHistoryButton.WithRelativeHeight(bottomView, 0.1f),

                tableView.Below(dowloadHistoryButton),
                tableView.WithSameCenterX(bottomView),
                tableView.WithSameWidth(bottomView),
                tableView.WithRelativeHeight(bottomView, 1)
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
        }

        protected override void InitializeBindings()
        {
            base.InitializeBindings();

            // choice here:
            //
            //   for original demo use:
            //     var source = new MvxStandardTableViewSource(tableView, "TitleText");
            //
            //   or for prettier cells from XIB file use:
            //     tableView.RowHeight = 88;
            //     var source = new MvxSimpleTableViewSource(tableView, BookCell.Key, BookCell.Key);

            tableView.RowHeight = 44;
            var source = new MvxSimpleTableViewSource(tableView, PayHistoryCell.Key, PayHistoryCell.Key);
            tableView.Source = source;

            var set = this.CreateBindingSet<PayHistoryView, PayHistoryViewModel>();
            set.Bind(backHomeView).To(vm => vm.BackHomeCommand);
            set.Bind(dowloadHistoryButton).To(vm => vm.DownloadHistoryCommand);
            set.Bind(source).To(vm => vm.History);
            set.Apply();
        }
    }
}
