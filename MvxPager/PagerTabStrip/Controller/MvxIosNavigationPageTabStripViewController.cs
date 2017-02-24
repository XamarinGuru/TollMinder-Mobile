using System;
using MvvmCross.Core.ViewModels;
using UIKit;
using MvxPagerTabStrip.Views;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using MvvmCross.iOS.Views;
using MvxPagerTabStrip.Models;
using System.Linq;

namespace MvxPagerTabStrip.Controller
{
	public class MvxIosNavigationPageTabStripViewController<TViewModel> : MvxIosPagerTabStripViewController<TViewModel> where TViewModel : class , IMvxViewModel
	{ 

		#region Fields
		const string keyFrame = "frame";

		UIView _navigationView;
		UIScrollView _navigationScrollView;
		TabPageControl _navigationPageControl;
		IList<UILabel> _navigationItemsViews;
		IDisposable _observer;

		UIFont _landscapeTitleFont;
		UIFont _portraitTitleFont;

		#endregion

		#region Constructors

		public MvxIosNavigationPageTabStripViewController () : base ()
		{
			
		}

		#endregion

		#region Properties

		public UIColor TitlesColor { get; set; } = UIColor.White;
		public UIFont TitlesFont { get; set; } = UIFont.FromName("Helvetica-Bold", 18f);

		protected nfloat GetDistanceValue
		{
			get {				
				CGPoint middle = NavigationScrollView.Center;
				var valueOfPoint = middle.X - 53 / 2;
				return valueOfPoint;
			}
		}

		protected UIFont LandScapeTitleFont
		{
			get {
				if (_landscapeTitleFont != null)
					return _landscapeTitleFont;
				_landscapeTitleFont = UIFont.SystemFontOfSize (15);
				return _landscapeTitleFont;
			}
		}

		protected UIFont PortraitTitleFont
		{
			get {
				if (_portraitTitleFont != null)
					return _portraitTitleFont;
				_portraitTitleFont = UIFont.SystemFontOfSize (18);
				return _portraitTitleFont;
			}
		}

		public UIView NavigationView {
			get {
				if (_navigationView != null) 
					return _navigationView;
				_navigationView = new UIView ();
				_navigationView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
				return _navigationView;
			}
		}

		public UIScrollView NavigationScrollView
		{
			get {				
				if (_navigationScrollView != null) 
					return _navigationScrollView;
				_navigationScrollView = new UIScrollView (new CGRect (0, 0, View.Bounds.Width, 34));
				_navigationScrollView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
				_navigationScrollView.Bounces = true;
				_navigationScrollView.ScrollsToTop = false;
				_navigationScrollView.ShowsHorizontalScrollIndicator = false;
				_navigationScrollView.ShowsVerticalScrollIndicator = false;
				_navigationScrollView.PagingEnabled = true;
				_navigationScrollView.UserInteractionEnabled = false;
				_navigationScrollView.AlwaysBounceHorizontal = true;
				_navigationScrollView.AlwaysBounceVertical = true;
				return _navigationScrollView;
			}
		}

		public IList<UILabel> NavigationItemsViews {
			get { 
				if (_navigationItemsViews != null) {
					return _navigationItemsViews;
				}
				_navigationItemsViews = new List<UILabel> ();
				return _navigationItemsViews;
			}
		}

		public TabPageControl NavigationPageControl {
			get { 
				if (_navigationPageControl != null) {
					return _navigationPageControl;
				}
				_navigationPageControl = new TabPageControl ();
				return _navigationPageControl;
			}
		}

		#endregion

		#region Methods

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			if (NavigationView.Superview == null) {
				NavigationItem.TitleView = new UIView (NavigationController.NavigationBar.Frame);
				NavigationItem.TitleView.AddSubview (NavigationView);
			}
			if (_observer == null) {
				_observer = NavigationView.AddObserver (keyFrame, NSKeyValueObservingOptions.OldNew, FrameChanged);	
				
			}
			NavigationView.Frame = new CGRect (0, 0, NavigationController.NavigationBar.Frame.Width, NavigationController.NavigationBar.Frame.Height);

			if (NavigationScrollView.Superview == null) {
				NavigationView.AddSubview (NavigationScrollView);
			}

			if (NavigationPageControl.Superview == null) {
				NavigationView.AddSubview (NavigationPageControl);
			}

			IsProgressiveIndicator = true;
		}



		public override void InitPagerTabStrip (params MvxPagerTab[] mvxViews)
		{
			base.InitPagerTabStrip (mvxViews);
			//ReloadPagerNavigationView ();
		}

		CGRect oldRect;

		public virtual void FrameChanged(NSObservedChange change)
		{
			if (change.Change == NSKeyValueChange.Setting) {
				if (oldRect == CGRect.Empty) {
					oldRect = ((NSValue)change.OldValue).CGRectValue;					
				}
				CGRect newRect = ((NSValue)change.NewValue).CGRectValue;
				if (oldRect != newRect) {
					NavigationScrollView.Frame = new CGRect (0, 0, NavigationView.Frame.Width, NavigationScrollView.Frame.Height);
					SetNavigationViewItemsPosition ();
				}
			}
		}

		public virtual void ReloadPagerNavigationView ()
		{
			if (IsViewLoaded) {
				ReloadNavigationViewItems ();
				SetNavigationViewItemsPosition ();
			}
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			SetNavigationViewItemsPosition ();
		}

		public override void ReloadPagerTabStripView ()
		{
			base.ReloadPagerTabStripView ();
			ReloadPagerNavigationView ();
		}

		public override void UpdatePage (nint fromIndex, nint toIndex)
		{
			//Do nothing
		}

		public override void UpdatePage (nint fromIndex, nint toIndex, nfloat withProgressPercentage)
		{
			nfloat distnace = GetDistanceValue;
			nfloat xOffset = fromIndex < toIndex ? distnace * fromIndex + distnace * withProgressPercentage : distnace * fromIndex - distnace * withProgressPercentage;
			NavigationScrollView.ContentOffset = new CGPoint (xOffset, 0);
			SetAlphaWithOffset (xOffset);
			NavigationPageControl.CurrentPage = CurrentIndex;
		}

		protected void ReloadNavigationViewItems ()
		{
			foreach (var item in NavigationItemsViews) {
				item.RemoveFromSuperview ();
			}
			NavigationItemsViews.Clear ();

			for (int i = 0; i < Titles.Count; i++) {
				UILabel navTitleLabel = CreateNewLabelWithText (Titles [i]);
				navTitleLabel.Alpha = 1;
				navTitleLabel.TextColor = TitlesColor;
				NavigationScrollView.AddSubview (navTitleLabel);
				NavigationItemsViews.Add (navTitleLabel);
			}
		}

		protected void SetNavigationViewItemsPosition ()
		{
			nfloat distance = GetDistanceValue;
			bool isPortrait = UIDeviceOrientationExtensions.IsPortrait (UIDevice.CurrentDevice.Orientation);
			nfloat labelHeightSpace = isPortrait ? 34 : 25;
			for (int i = 0; i < NavigationItemsViews.Count; i++) {
				UILabel label = NavigationItemsViews [i];
				label.Alpha = CurrentIndex == i ? 1 : 0;
				label.Font = TitlesFont;
				CGSize viewSize = GetLabelSize (label);
				nfloat originX = (distance - viewSize.Width / 2) + i * distance;
				nfloat originY = (labelHeightSpace - viewSize.Height) / 2;
				label.Frame = new CGRect (originX, originY + 2, viewSize.Width, viewSize.Height);
				Console.WriteLine (label);
				label.Tag = i;
			}

			nfloat xOffset = distance * CurrentIndex;
			NavigationScrollView.ContentOffset = new CGPoint (xOffset + NavigationScrollView.ContentOffset.X, 0);
			NavigationPageControl.NumberOfPages = NavigationItemsViews.Count;
			NavigationPageControl.CurrentPage = CurrentIndex;
			CGSize viewSizeForPageControl = NavigationPageControl.SizeForNumbaerOfPager();
			nfloat originXPageControl = (distance - viewSizeForPageControl.Width / 2);
			NavigationPageControl.Frame = new CGRect (originXPageControl, labelHeightSpace, viewSizeForPageControl.Width, viewSizeForPageControl.Height);
		}

		protected void SetAlphaWithOffset(nfloat xOffset)
		{
			nfloat disance = GetDistanceValue;
			for (int i = 0; i < NavigationItemsViews.Count; i++) {
				nfloat alpha = xOffset < disance * i ? (xOffset - disance * (i - 1)) / disance : 1 - ((xOffset - disance * i) / disance);
				NavigationItemsViews [i].Alpha = alpha;
			}
		}

		protected UILabel CreateNewLabelWithText(string text)
		{
			UILabel navTitleLabel = new UILabel();
			navTitleLabel.Text = text;
			navTitleLabel.Font = TitlesFont;
			navTitleLabel.TextColor = TitlesColor;
			navTitleLabel.Alpha = 0;
			return navTitleLabel;
		}

		protected CGSize GetLabelSize (UILabel label)
		{
			NSString labelText = new NSString (label.Text);
			return labelText.StringSize (label.Font);
		}

		bool _isDisposed;
		protected override void Dispose (bool disposing)
		{
			if (disposing && !_isDisposed) {
				_observer.Dispose ();
				_observer = null;
				_isDisposed = true;
			}
			base.Dispose (disposing);
		}

		#endregion
	}
}