using System;

using UIKit;
using CoreGraphics;
using Foundation;
using MvxPagerTabStrip.Cells;

namespace MvxPagerTabStrip.Views
{
	[Register("ButtonBarView")]
	public class ButtonBarView : UICollectionView
	{
		nint _selectedOptionIndex;

		public ButtonBarView (NSCoder coder)
			: base(coder)
		{
			InitializeXLButtonBarView();
		}

		public ButtonBarView (CGRect rect, UICollectionViewLayout layout)
			: base(rect, layout)
		{
			InitializeXLButtonBarView();
		}

		protected void InitializeXLButtonBarView()
		{
			_selectedOptionIndex = 0;
			if (this.SelectedBar.Superview == null){
				this.AddSubview(SelectedBar);
			}

			SelectedBar.BackgroundColor = UIColor.Brown;

			ScrollsToTop = false;
			ShowsHorizontalScrollIndicator = false;
			BackgroundColor = UIColor.White;
			AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
			RegisterClassForCell(typeof(ButtonBarViewCell), ButtonBarViewCell.Key);
			LabelFont = UIFont.FromName ("Helvetica-Light", 18f);
			LeftRightMargin = 8;
		}

		UIView _selectedBar;
		public UIView SelectedBar {
			get {             
				if (_selectedBar != null)
					return _selectedBar;
				_selectedBar = new UIView(new CGRect(0, this.Frame.Size.Height - 2, this.Frame.Size.Width, 2));
				_selectedBar.Layer.ZPosition = 9999;
				_selectedBar.BackgroundColor = UIColor.Black;
				return _selectedBar; 
			}
		}

		public UIFont LabelFont { get; set; }
		public nuint LeftRightMargin { get; set; }

		public void MoveToIndex(nint index, bool animated)
		{
			this._selectedOptionIndex = index;
			this.UpdateSelectedBarPositionWithAnimation(animated);
		}

		public void MoveFromIndex(nint fromIndex, nint toIndex, nfloat progressPercentage)
		{
			this._selectedOptionIndex = (progressPercentage > 0.5f ) ? toIndex : fromIndex;

			UICollectionViewLayoutAttributes attributes = GetLayoutAttributesForItem(NSIndexPath.FromItemSection(fromIndex, 0));
			CGRect fromFrame = attributes.Frame;
			nint numberOfItems = DataSource.GetItemsCount (this, 0);
			CGRect toFrame;
			if (toIndex < 0 || toIndex > numberOfItems - 1){
				if (toIndex < 0) {
					UICollectionViewLayoutAttributes cellAtts = GetLayoutAttributesForItem(NSIndexPath.FromItemSection(0, 0));
					toFrame = new CGRect (-cellAtts.Frame.Size.Width, cellAtts.Frame.Y, cellAtts.Frame.Size.Width, cellAtts.Frame.Size.Height);
				}
				else{
					UICollectionViewLayoutAttributes cellAtts = this.GetLayoutAttributesForItem(NSIndexPath.FromItemSection(numberOfItems - 1, 0));
					toFrame = new CGRect (cellAtts.Frame.Size.Width + cellAtts.Frame.X , cellAtts.Frame.Y, cellAtts.Frame.Size.Width, cellAtts.Frame.Size.Height);
				}
			}
			else{
				toFrame = this.GetLayoutAttributesForItem(NSIndexPath.FromItemSection(toIndex, 0)).Frame;
			}
			CGRect targetFrame = fromFrame;
			targetFrame.Height =  SelectedBar.Frame.Size.Height;
			targetFrame.Width += (toFrame.Size.Width - fromFrame.Size.Width) * progressPercentage;
			var originX = targetFrame.Location.X;
			var updatedOriginX = originX + (toFrame.Location.X - fromFrame.Location.X) * progressPercentage;
			targetFrame.Location = new CGPoint (updatedOriginX, targetFrame.Location.Y);
			nuint offset = 35;
			nfloat xValue = 0;
			if (ContentSize.Width > this.Frame.Size.Width){
				xValue = (nfloat)Math.Min(ContentSize.Width - Frame.Size.Width, targetFrame.Location.X - offset <= 0.0 ? 0.0 : targetFrame.Location.X - offset);
			}
			//NSLog(@"X value: %@", @(xValue));
			this.SetContentOffset(new CGPoint(xValue, 0), false);
			this.SelectedBar.Frame = new CGRect(targetFrame.Location.X, SelectedBar.Frame.Location.Y, targetFrame.Size.Width, SelectedBar.Frame.Size.Height);
		}


		void UpdateSelectedBarPositionWithAnimation(bool animation)
		{
			CGRect frame = SelectedBar.Frame;
            if (DataSource == null)
                return;
			UICollectionViewCell cell = this.DataSource.GetCell(this, NSIndexPath.FromItemSection(_selectedOptionIndex, 0));
			UpdateContentOffset(cell);

			frame.Width = cell.Frame.Size.Width;
			frame.Location = new CGPoint (cell.Frame.X, frame.Location.Y);
			if (animation){
				UIView.AnimateAsync(0.2, () => {
					SelectedBar.Frame = frame;
				});
			}
			else{
				SelectedBar.Frame = frame;
			}
		}

		void UpdateContentOffset(UICollectionViewCell cell)
		{
			if (cell != null){
				nuint offset = 16;
				nfloat xValue = NMath.Min(
					NMath.Max(0,
						this.ContentSize.Width - this.Frame.Size.Width), // dont scroll if we are at the end of scroll view, if content is smaller than container Width we scroll 0
					NMath.Max(((UICollectionViewFlowLayout)CollectionViewLayout).SectionInset.Left - cell.Frame.Location.X,
						cell.Frame.Location.X - ((UICollectionViewFlowLayout)CollectionViewLayout).SectionInset.Left -  offset)

				);
				this.SetContentOffset(new CGPoint(xValue, 0), true);
			}
		}

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			OnViewDidUnload ();
		}

		protected virtual void OnViewDidUnload ()
		{
			SelectedBar.RemoveFromSuperview ();
			DataSource = null;
			WeakDataSource = null;
			Delegate = null;
			WeakDelegate = null;
			_selectedBar = null;
			LabelFont = null;
		}
	}
}