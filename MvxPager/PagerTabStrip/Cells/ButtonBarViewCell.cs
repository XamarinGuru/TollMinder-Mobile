using System;
using UIKit;
using CoreGraphics;
using Foundation;

namespace MvxPagerTabStrip.Cells
{
	[Register("ButtonBarViewCell")]
	public sealed class ButtonBarViewCell : UICollectionViewCell
	{		
		public static readonly NSString Key = new NSString ("CategoryCellView");

		public ButtonBarViewCell (IntPtr handle) : base (handle)
		{
			Label = new UILabel(this.Bounds);
			Label.TextAlignment = UITextAlignment.Center;
			ContentView.AddSubview(Label);
		}

		public ButtonBarViewCell(CGRect frame) : base(frame)
		{
			Label = new UILabel(this.Bounds);
            Label.Font = UIFont.FromName("Helvetica", 14);
			Label.TextAlignment = UITextAlignment.Center;
			ContentView.AddSubview(Label);
		}

		public UIImageView ImageView { get; private set; }
		public UILabel Label { get; private set; }


		public void UpdateView(string text)
		{
			Label.Text = text;
		}
	}
}
