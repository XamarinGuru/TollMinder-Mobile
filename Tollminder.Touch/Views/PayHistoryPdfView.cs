using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using QuickLook;
using Tollminder.Core.ViewModels;
using Tollminder.Touch.Controllers;
using Tollminder.Touch.Controls;
using Tollminder.Touch.Extensions;
using Tollminder.Touch.Helpers;
using UIKit;

namespace Tollminder.Touch.Views
{
    public class PayHistoryPdfView : BaseViewController<PayHistoryPdfViewModel>
    {
        private NSUrl _documentUrl;
        private string pdfPath;
        UIWebView pdfWebView;
        UILabel informationLabel;
        LabelForDataWheel urlLabel;
        UIButton backToPayHistoryViewButton;
        ProfileButton downloadPdfButton;
        ProfileButton openInButton;

        protected override void InitializeObjects()
        {
            base.InitializeObjects();

            View.BackgroundColor = UIColor.LightGray;
            var topView = new UIView();
            var downloadRowView = new UIView();
            var profileNavigationBarBackground = new UIImageView(UIImage.FromBundle(@"Images/navigation_bar_background.png"));
            profileNavigationBarBackground.Frame = new CoreGraphics.CGRect(10, 10, profileNavigationBarBackground.Image.CGImage.Width, profileNavigationBarBackground.Image.CGImage.Height);

            backToPayHistoryViewButton = UIButton.FromType(UIButtonType.Custom);
            backToPayHistoryViewButton.SetImage(UIImage.FromFile(@"Images/ic_back.png"), UIControlState.Normal);

            downloadPdfButton = ProfileButtonManager.ButtonInitiaziler("Download", UIImage.FromFile(@"Images/PayHistoryView/ic_download.png"));
            openInButton = ProfileButtonManager.ButtonInitiaziler("Pdf open in", UIImage.FromFile(@"Images/PayHistoryView/ic_openIn.png"));

            informationLabel = new UILabel();
            informationLabel.TextColor = UIColor.White;
            informationLabel.Text = "Payment History";
            informationLabel.Font = UIFont.BoldSystemFontOfSize(16f);
            informationLabel.TextAlignment = UITextAlignment.Center;
            
            urlLabel = LabelDataWheelInitiaziler("Pdf url");
            pdfWebView = new UIWebView(View.Bounds);

            // Hide navigation bar
            NavigationController.SetNavigationBarHidden(true, false);
            View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile(@"Images/tab_background.png").Scale(View.Frame.Size));

            topView.AddIfNotNull(profileNavigationBarBackground, backToPayHistoryViewButton, informationLabel);
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
                informationLabel.WithRelativeHeight(topView, 0.3f)
            );

            downloadRowView.AddIfNotNull(openInButton); //downloadPdfButton, 
            downloadRowView.AddConstraints(
                //downloadPdfButton.AtTopOf(downloadRowView),
                //downloadPdfButton.AtLeftOf(downloadRowView),
                //downloadPdfButton.WithRelativeWidth(downloadRowView, 0.47f),
                //downloadPdfButton.WithSameHeight(downloadRowView),

                openInButton.AtTopOf(downloadRowView),
                openInButton.WithSameWidth(downloadRowView),
                openInButton.WithSameCenterX(downloadRowView),
                //openInButton.AtRightOf(downloadRowView),
                //openInButton.WithRelativeWidth(downloadRowView, 0.5f),
                openInButton.WithSameHeight(downloadRowView)
            );

            View.AddIfNotNull(topView, downloadRowView, pdfWebView);
            View.AddConstraints(
                topView.AtTopOf(View),
                topView.WithSameWidth(View),
                topView.WithRelativeHeight(View, 0.2f),

                downloadRowView.Below(topView, 10),
                downloadRowView.AtLeftOf(View, 30),
                downloadRowView.AtRightOf(View, 30),
                downloadRowView.WithRelativeHeight(View, 0.06f),

                pdfWebView.Below(downloadRowView, 10),
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
                set.Bind(urlLabel.WheelText).To(vm => vm.PdfUrl);
                set.Bind(backToPayHistoryViewButton).To(vm => vm.BackToPayHistoryCommand);
                //set.Bind(downloadPdfButton).To(vm => vm.DownloadPayHistoryPdfCommand);
                set.Bind(openInButton).To(vm => vm.FileOpenInCommand);
                set.Apply();
                pdfWebView.LoadRequest(new NSUrlRequest(new NSUrl(urlLabel.WheelText.Text)));
        }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, ex.StackTrace);
            }
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
    }
}