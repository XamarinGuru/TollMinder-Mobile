using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using MvxPagerTabStrip.Views;
using MvxPagerTabStrip.Cells;
using UIKit;
using MvxPagerTabStrip.Models;

namespace MvxPagerTabStrip.Controller
{
	public class MvxIosButtonTabStripViewController<TViewModel> : MvxIosPagerTabStripViewController<TViewModel> ,
	IUICollectionViewDelegateFlowLayout, IUICollectionViewDataSource, IUICollectionViewDelegate  where TViewModel : class, IMvxViewModel
	{		
		public MvxIosButtonTabStripViewController ()
		{
			_shouldUpdateButtonBarView = true;
		}

		public bool _shouldUpdateButtonBarView;

		ButtonBarView _buttonBarView;
		public ButtonBarView ButtonBarView
		{
			get { 
				if (_buttonBarView != null) 
					return _buttonBarView;
				UICollectionViewFlowLayout flowLayout = new UICollectionViewFlowLayout();
                //flowLayout.ScrollDirection = UICollectionViewScrollDirection.Horizontal;
				flowLayout.SectionInset = new UIEdgeInsets(0, 0, 0, 0);
                flowLayout.MinimumLineSpacing = 0;
                flowLayout.MinimumInteritemSpacing = 0;
				_buttonBarView = new ButtonBarView(new CGRect(0, 0, View.Frame.Width, 44.0f) , flowLayout);
				return _buttonBarView;
			}
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			View.BackgroundColor = UIColor.White;
		}

		public override void InitPagerTabStrip (params MvxPagerTab[] mvxViews)
		{
			base.InitPagerTabStrip (mvxViews);
			ContainerView.Frame = new CGRect (ContainerView.Frame.X, ContainerView.Frame.Y + ButtonBarView.Frame.Height, ContainerView.Frame.Width, ContainerView.Frame.Height);
			if (ButtonBarView.Superview == null) {
				View.AddSubview (ButtonBarView);
			}
			_buttonBarView.WeakDelegate = this;
			_buttonBarView.WeakDataSource = this;
			ReloadPagerTabStripView ();
		}

		public override void ReloadPagerTabStripView ()
		{
			base.ReloadPagerTabStripView ();
			if (IsViewLoaded) {
				ButtonBarView.ReloadData ();
				ButtonBarView.MoveToIndex (CurrentIndex, false);
			}
		}

		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			Dispose ();
		}

		[Foundation.Export ("collectionView:didSelectItemAtIndexPath:")]
		public virtual void ItemSelected (UIKit.UICollectionView collectionView, Foundation.NSIndexPath indexPath)
		{
			ButtonBarView.MoveToIndex (indexPath.Item, true);
			_shouldUpdateButtonBarView = false;
			MoveToViewControllerAtIndex ((int)indexPath.Item);
		}

		public virtual nint GetItemsCount (UICollectionView collectionView, nint section)
		{			
			return PagerTabStripChildViewControllers.Count;
		}

		public virtual UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath)
		{
			var cell = collectionView.DequeueReusableCell (ButtonBarViewCell.Key, indexPath);
			if (cell == null) {
				cell = new ButtonBarViewCell (new CGRect (0, 0, 50, ButtonBarView.Frame.Size.Height));
			}
			var buttonBarCell = cell as ButtonBarViewCell;
			buttonBarCell.UpdateView (Titles[(int)indexPath.Item]);
			return buttonBarCell;
		}

		#region implemented abstract members of MvxIosPagerTabStripViewController

		public override void UpdatePage (nint fromIndex, nint toIndex)
		{
			if (_shouldUpdateButtonBarView){				
				ButtonBarView.MoveToIndex (toIndex, true);
			}
		}

		public override void UpdatePage (nint fromIndex, nint toIndex, nfloat withProgressPercentage)
		{
			if (_shouldUpdateButtonBarView){
				ButtonBarView.MoveFromIndex (fromIndex, toIndex, withProgressPercentage);
			}
		}

		#endregion

		[Foundation.Export ("collectionView:layout:sizeForItemAtIndexPath:")]
		public virtual CoreGraphics.CGSize GetSizeForItem (UIKit.UICollectionView collectionView, UIKit.UICollectionViewLayout layout, Foundation.NSIndexPath indexPath)
		{
			UILabel label = new UILabel ();
			label.TranslatesAutoresizingMaskIntoConstraints = false;
			label.Font = ButtonBarView.LabelFont;
			label.Text = Titles [(int)indexPath.Item];
            //CGSize labelSize = label.IntrinsicContentSize;
            return new CGSize(collectionView.Frame.Width / 3, collectionView.Frame.Size.Height);
		}

		[Export("numberOfSectionsInCollectionView:")]
		public System.nint NumberOfSections(UIKit.UICollectionView collectionView)
		{
			return 1;
		}

		public override void ScrollAnimationEnded (UIScrollView scrollView)
		{
			base.ScrollAnimationEnded (scrollView);
			if (scrollView == ContainerView){
				_shouldUpdateButtonBarView = true;
			}
		}

		protected override void Dispose (bool disposing)
		{
            if (disposing)
            {
                if (ButtonBarView != null)
                {
                    ButtonBarView.WeakDelegate = null;
                    ButtonBarView.WeakDataSource = null;
                    ButtonBarView.Dispose();
                    _buttonBarView = null;
                }
            }

            base.Dispose (disposing);
		}
	}
}