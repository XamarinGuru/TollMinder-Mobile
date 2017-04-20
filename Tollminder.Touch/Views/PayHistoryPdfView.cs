using System;
using System.Diagnostics;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using Tollminder.Core.ViewModels.Payments;
using Tollminder.Touch.Controllers;
using Tollminder.Touch.Extensions;
using UIKit;

namespace Tollminder.Touch.Views
{
    public class PayHistoryPdfView : BaseViewController<PayHistoryPdfViewModel>
    {
        UIWebView pdfWebView;
        UILabel informationLabel;
        UILabel urlLabel;

        UIButton backToPayHistoryViewButton;
        UIButton openInButton;

        protected override void InitializeObjects()
        {
            base.InitializeObjects();

            View.BackgroundColor = UIColor.LightGray;
            var topView = new UIView();
            var profileNavigationBarBackground = new UIImageView(UIImage.FromBundle(@"Images/navigation_bar_background.png"));
            profileNavigationBarBackground.Frame = new CoreGraphics.CGRect(10, 10, profileNavigationBarBackground.Image.CGImage.Width, profileNavigationBarBackground.Image.CGImage.Height);

            backToPayHistoryViewButton = UIButton.FromType(UIButtonType.Custom);
            backToPayHistoryViewButton.SetImage(UIImage.FromFile(@"Images/ic_back.png"), UIControlState.Normal);

            openInButton = UIButton.FromType(UIButtonType.Custom);
            openInButton.SetImage(UIImage.FromFile(@"Images/PayHistoryView/ic_openIn.png"), UIControlState.Normal);

            informationLabel = new UILabel();
            informationLabel.TextColor = UIColor.White;
            informationLabel.Text = "Payment History";
            informationLabel.Font = UIFont.BoldSystemFontOfSize(16f);
            informationLabel.TextAlignment = UITextAlignment.Center;

            urlLabel = new UILabel();
            pdfWebView = new UIWebView(View.Bounds);

            // Hide navigation bar
            NavigationController.SetNavigationBarHidden(true, false);
            View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile(@"Images/tab_background.png").Scale(View.Frame.Size));

            topView.AddIfNotNull(profileNavigationBarBackground, backToPayHistoryViewButton, informationLabel, openInButton);
            topView.AddConstraints(
                profileNavigationBarBackground.WithSameWidth(topView),
                profileNavigationBarBackground.WithSameHeight(topView),
                profileNavigationBarBackground.AtTopOf(topView),

                backToPayHistoryViewButton.WithSameCenterY(topView),
                backToPayHistoryViewButton.AtLeftOf(topView, 20),
                backToPayHistoryViewButton.WithRelativeWidth(topView, 0.1f),
                backToPayHistoryViewButton.WithRelativeHeight(topView, 0.2f),

                informationLabel.WithSameCenterY(topView),
                informationLabel.WithSameCenterX(topView),
                informationLabel.WithSameWidth(topView),
                informationLabel.WithRelativeHeight(topView, 0.3f),

                openInButton.WithSameCenterY(topView),
                openInButton.AtRightOf(topView, 20),
                openInButton.WithRelativeWidth(topView, 0.1f),
                openInButton.WithRelativeHeight(topView, 0.2f)
            );

            View.AddIfNotNull(topView, pdfWebView);
            View.AddConstraints(
                topView.AtTopOf(View),
                topView.WithSameWidth(View),
                topView.WithRelativeHeight(View, 0.2f),

                pdfWebView.Below(topView, 10),
                pdfWebView.AtLeftOf(View, 30),
                pdfWebView.AtRightOf(View, 30),
                pdfWebView.WithRelativeHeight(View, 0.8f)
            );
        }

        protected override void InitializeBindings()
        {
            base.InitializeBindings();
            try
            {
                var set = this.CreateBindingSet<PayHistoryPdfView, PayHistoryPdfViewModel>();
                set.Bind(urlLabel).To(vm => vm.PdfUrl);
                set.Bind(backToPayHistoryViewButton).To(vm => vm.BackToPayHistoryCommand);
                set.Bind(openInButton).To(vm => vm.FileOpenInCommand);
                set.Apply();
                pdfWebView.LoadRequest(new NSUrlRequest(new NSUrl(urlLabel.Text)));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, ex.StackTrace);
            }
        }
    }
}