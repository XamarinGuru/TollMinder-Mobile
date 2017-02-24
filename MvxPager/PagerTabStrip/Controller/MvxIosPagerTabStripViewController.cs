using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using MvxPagerTabStrip.Models;
using UIKit;

namespace MvxPagerTabStrip.Controller
{
	public abstract class MvxIosPagerTabStripViewController<TViewModel> : MvxViewController<TViewModel> , IUIScrollViewDelegate where TViewModel : class , IMvxViewModel
	{
		#region Private Fields

		#pragma warning disable 414 169
		nint _lastPageNumber;
		nint _pageBeforeRotate;
		#pragma warning restore 414 169
		nfloat _lastContentOffset;
		IList<IMvxIosView> _originalPagerTabStripChildViewControllers;
		CGSize _lastSize;
		
		#endregion



		protected MvxIosPagerTabStripViewController ()
		{
			PagerTabStripViewControllerInit ();
		}

		#region Properties
		public nint CurrentIndex { get; protected set; }

		IList<IMvxIosView> _pagerTabStripChildViewControllers;
		public virtual IList<IMvxIosView> PagerTabStripChildViewControllers {
			get { return _pagerTabStripChildViewControllers; }
			protected set { _pagerTabStripChildViewControllers = value; }
		}

		UIScrollView  _containerView;
		public UIScrollView ContainerView {
			get {		
				InitScrollView ();
				return _containerView;
			}
		}

		private IList<string> _titles = new List<string> ();
		public virtual IList<string> Titles {
			get { return _titles; }
		}

		public bool SkipIntermediateViewControllers { get; set; }
		public bool IsProgressiveIndicator { get; set; }
		public bool IsElasticIndicatorLimit { get; set; }

		#endregion

		protected void PagerTabStripViewControllerInit()
		{
			CurrentIndex = 0;
			_lastContentOffset = 0.0f;
			IsElasticIndicatorLimit = false;
			SkipIntermediateViewControllers = true;
			IsProgressiveIndicator = false;
		}

		#region Public Methods

		public virtual void InitPagerTabStrip(params MvxPagerTabStrip.Models.MvxPagerTab[] mvxViews)
		{			
			var mvxList = new List<IMvxIosView> ();
			var mvxTitles = new List<string> ();
			foreach (var item in mvxViews) {
				mvxList.Add (item.View);
				mvxTitles.Add (item.Title);
			}
			_titles = mvxTitles;
			PagerTabStripChildViewControllers = mvxList;
			ReloadPagerTabStripView ();
		}

		public virtual void MoveToViewControllerAtIndex (int index)
		{
			MoveToViewControllerAtIndex (index, true);
		}

		public virtual void MoveToViewControllerAtIndex (int index, bool animated)
		{
			if (!IsViewLoaded) {
				CurrentIndex = index;
			} else {
				if (SkipIntermediateViewControllers && NMath.Abs (CurrentIndex - index) > 1) {
					var originalPagerTabStripChildViewControllers = _pagerTabStripChildViewControllers;
					var tempChildControllerList = _pagerTabStripChildViewControllers.ToList();
					var currentChildVC = originalPagerTabStripChildViewControllers [(int)CurrentIndex];
					nint fromIndex = (CurrentIndex < index) ? index - 1 : index + 1;
					tempChildControllerList [(int)CurrentIndex] = originalPagerTabStripChildViewControllers [(int)fromIndex];
					tempChildControllerList [(int)fromIndex] = currentChildVC;
					_pagerTabStripChildViewControllers = tempChildControllerList;
					ContainerView.SetContentOffset (new CGPoint (PageOffsetForChildIndex (fromIndex), 0), false);
					if (NavigationController != null) {
						NavigationController.View.UserInteractionEnabled = false;
					} else {
						View.UserInteractionEnabled = false;
					}				
					_originalPagerTabStripChildViewControllers = originalPagerTabStripChildViewControllers;
					ContainerView.SetContentOffset (new CGPoint (PageOffsetForChildIndex (index), 0), true);
				} else {
					ContainerView.SetContentOffset (new CGPoint (PageOffsetForChildIndex (index), 0), animated);
				}
			}
		}

		public virtual void MoveToViewController (MvxViewController viewController)
		{
			MoveToViewControllerAtIndex (_pagerTabStripChildViewControllers.IndexOf (viewController));
		}

		public virtual void ReloadPagerTabStripView ()
		{
			if (IsViewLoaded) {
				foreach (UIViewController item in _pagerTabStripChildViewControllers) {
					if (item.ParentViewController != null) {
						item.View.RemoveFromSuperview ();
						item.WillMoveToParentViewController (null);
						item.RemoveFromParentViewController ();
					}
				}
				ContainerView.ContentSize = new CGSize (ContainerView.Bounds.Width * _pagerTabStripChildViewControllers.Count, ContainerView.ContentSize.Height);
				if (CurrentIndex >= _pagerTabStripChildViewControllers.Count) {
					CurrentIndex = _pagerTabStripChildViewControllers.Count - 1;
				}
				ContainerView.SetContentOffset (new CGPoint (PageOffsetForChildIndex (CurrentIndex), 0), false);
				UpdateContent ();
			}
		}

		#endregion

		#region Abstract Membors
		public abstract void UpdatePage(nint fromIndex, nint toIndex);
		public abstract void UpdatePage(nint fromIndex, nint toIndex, nfloat withProgressPercentage);
		#endregion

		#region UIScrollView methods

		[Foundation.Export ("scrollViewDidScroll:")]
		public virtual void Scrolled (UIKit.UIScrollView scrollView)
		{
			if (ContainerView == scrollView){
				UpdateContent ();
			}
		}

		[Foundation.Export ("scrollViewWillBeginDragging:")]
		public virtual void DraggingStarted (UIKit.UIScrollView scrollView)
		{
			if (ContainerView == scrollView){
				_lastPageNumber = PageForContentOffset(scrollView.ContentOffset.X);
				_lastContentOffset = scrollView.ContentOffset.X;
			}
		}

		[Foundation.Export ("scrollViewDidEndScrollingAnimation:")]
		public virtual void ScrollAnimationEnded (UIKit.UIScrollView scrollView)
		{
			if (ContainerView == scrollView && _originalPagerTabStripChildViewControllers != null) {
				_pagerTabStripChildViewControllers = _originalPagerTabStripChildViewControllers;
				_originalPagerTabStripChildViewControllers = null;
				if (NavigationController != null) {
					NavigationController.View.UserInteractionEnabled = true;
				} else {
					View.UserInteractionEnabled = true;
				}
				UpdateContent ();
			}
		}

		#endregion

		#region Override UIController Methods

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			View.AddSubview (ContainerView);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			_lastSize = ContainerView.Bounds.Size;
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			UpdateIfNeeded ();
		}

		public override void ViewDidLayoutSubviews ()
		{
			base.ViewDidLayoutSubviews ();
			UpdateIfNeeded();
		}

		public override void DidMoveToParentViewController (UIViewController parent)
		{
			if (parent == null) {
				Dispose (true);
			}
			base.DidMoveToParentViewController (parent);
		}

		#endregion

		#region Helpers Methods

		protected virtual void UpdateIfNeeded ()
		{
			if (_lastSize != ContainerView.Bounds.Size && PagerTabStripChildViewControllers != null){
				UpdateContent ();
			}
		}

		protected virtual void InitScrollView()
		{
			if (_containerView == null) {
				_containerView = new UIScrollView (new CGRect (0, 0, View.Bounds.Width, View.Bounds.Height));
				_containerView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
				_containerView.Bounces = true;
				_containerView.AlwaysBounceHorizontal = true;
				_containerView.AlwaysBounceVertical = false;
				_containerView.ScrollsToTop = false;
				_containerView.WeakDelegate = this;
				_containerView.ShowsVerticalScrollIndicator = false;
				_containerView.ShowsHorizontalScrollIndicator = false;
				_containerView.PagingEnabled = true;					
			}
		}

		protected virtual nfloat PageOffsetForChildIndex (nint index)
		{
			return index * ContainerView.Bounds.Width;
		}

		protected virtual nfloat OffsetForChildIndex (nint index)
		{
			return (index * ContainerView.Bounds.Width + (ContainerView.Bounds.Width - View.Bounds.Width) * 0.5f);
		}

		protected virtual nint VirtualPageForContentOffset(nfloat contentOffset)
		{
			nfloat result = (contentOffset + (1.5f * PageWidth)) / PageWidth;
			return (nint)result - 1;
		}

		protected virtual nint PageForContentOffset(nfloat contentOffset)
		{
			nint result = VirtualPageForContentOffset (contentOffset);
			return PageForVirtualPage (result);
		}


		public virtual nfloat PageWidth 
		{
			get { return ContainerView.Bounds.Width; }
		}

		protected virtual nint PageForVirtualPage (nint virtualPage)
		{
			if (virtualPage < 0){
				return 0;
			}
			if (virtualPage > _pagerTabStripChildViewControllers.Count - 1){
				return _pagerTabStripChildViewControllers.Count - 1;
			}
			return virtualPage;
		}


		protected virtual nfloat ScrollPercentage
		{
			get 
			{
				if (ScrollDirection == PagerTabStripDirection.Left || ScrollDirection == PagerTabStripDirection.None){
					return (ContainerView.ContentOffset.X % PageWidth) / PageWidth;
				}
				return 1 - ((ContainerView.ContentOffset.X >= 0 ? ContainerView.ContentOffset.X : PageWidth + ContainerView.ContentOffset.X) % PageWidth) / PageWidth;
			}
		}

		PagerTabStripDirection ScrollDirection
		{
			get {
				if (ContainerView.ContentOffset.X > _lastContentOffset) {
					return PagerTabStripDirection.Left;
				} else if (ContainerView.ContentOffset.X < _lastContentOffset) {
					return PagerTabStripDirection.Right;
				}
				return PagerTabStripDirection.None;
			}
		}


		private void UpdateContent ()
		{			
			if (_lastSize != ContainerView.Bounds.Size) {
				_lastSize = ContainerView.Bounds.Size;
				ContainerView.SetContentOffset (new CGPoint (PageOffsetForChildIndex (CurrentIndex), 0), false);
			}
			var childViewControllers = _pagerTabStripChildViewControllers;
			ContainerView.ContentSize = new CGSize (ContainerView.Bounds.Width * childViewControllers.Count, ContainerView.ContentSize.Height);
			for (int i = 0; i < childViewControllers.Count; i++) {
				UIViewController childController = childViewControllers [i] as UIViewController;
				nfloat pageOffsenForChild = PageOffsetForChildIndex (i);
				if (NMath.Abs (ContainerView.ContentOffset.X - pageOffsenForChild) < ContainerView.Bounds.Width) {
					if (childController.ParentViewController == null) {
						AddChildViewController (childController);
						childController.DidMoveToParentViewController (this);
						nfloat childPosition = OffsetForChildIndex (i);
						childController.View.Frame = new CGRect (childPosition, 0, View.Bounds.Width, ContainerView.Bounds.Height);
						childController.View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
						ContainerView.AddSubview (childController.View);
					} else {
						nfloat childPosition = OffsetForChildIndex (i);
						childController.View.Frame = new CGRect (childPosition, 0, View.Bounds.Width, ContainerView.Bounds.Height);
						childController.View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
					}
				} else {
					if (childController.ParentViewController != null) {
						childController.View.RemoveFromSuperview ();
						childController.WillMoveToParentViewController (null);
						childController.RemoveFromParentViewController ();
					}
				}
			}


			nint oldCurrentIndex = CurrentIndex;
			nint virtualPage = VirtualPageForContentOffset (ContainerView.ContentOffset.X);
			nint newCurrentIndex = PageForVirtualPage (virtualPage);
			CurrentIndex = newCurrentIndex;

			if (IsProgressiveIndicator) {
				nfloat scrollPercentage = ScrollPercentage;
				if (scrollPercentage > 0) {
					nint fromIndex = CurrentIndex;
					nint toIndex = CurrentIndex;
					PagerTabStripDirection scrollDirection = ScrollDirection;
					if (scrollDirection == PagerTabStripDirection.Left) {
						if (virtualPage > _pagerTabStripChildViewControllers.Count - 1) {
							fromIndex = _pagerTabStripChildViewControllers.Count - 1;
							toIndex = _pagerTabStripChildViewControllers.Count;
						} else {
							if (scrollPercentage > 0.5f) {
								fromIndex = NMath.Max (toIndex - 1, 0);
							} else {
								toIndex = fromIndex + 1;
							}								
						}
					} else if (scrollDirection == PagerTabStripDirection.Right) {
						if (virtualPage < 0) {
							fromIndex = 0;
							toIndex = -1;
						} else {
							if (scrollPercentage > 0.5f) {
								fromIndex = NMath.Min (toIndex + 1, _pagerTabStripChildViewControllers.Count - 1);
							} else {
								toIndex = fromIndex - 1;
							}
						}
					}
					UpdatePage (fromIndex, toIndex, IsElasticIndicatorLimit ? scrollPercentage : (toIndex < 0 || toIndex >= _pagerTabStripChildViewControllers.Count ? 0 : scrollPercentage));
				}
			} else {
				if (oldCurrentIndex != newCurrentIndex) {
					UpdatePage (NMath.Max (oldCurrentIndex, _pagerTabStripChildViewControllers.Count - 1), newCurrentIndex);
				}
			}
		}

		#endregion

		#region Dispose

		bool _isDisposed;
		protected override void Dispose (bool disposing)
		{
			if (disposing && !_isDisposed) {				
				if (_pagerTabStripChildViewControllers != null) {
					foreach (MvxViewController item in _pagerTabStripChildViewControllers) {
						if (item.IsViewLoaded) {
							item.View.RemoveFromSuperview ();
							item.RemoveFromParentViewController ();
							item.ViewDidUnload ();
						}
					}
				}
				_pagerTabStripChildViewControllers = null;
				_containerView.WeakDelegate = null;
				_originalPagerTabStripChildViewControllers = null;
				_isDisposed = true;
			}
			base.Dispose (disposing);
		}

		#endregion
	}
}

