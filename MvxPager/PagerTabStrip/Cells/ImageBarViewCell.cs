using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MvxPagerTabStrip.Cells
{
    public class ImageBarViewCell : UICollectionViewCell
    {       
        public static readonly NSString Key = new NSString("ImageBarViewCell");

        public ImageBarViewCell(IntPtr handle) : base (handle)
        {
            InitObjects();
        }

        public ImageBarViewCell(CGRect frame) : base(frame)
        {
            InitObjects();
        }

        void InitObjects()
        {
            var size = Bounds.Height - 10;
            ImageView = new UIImageView(new CGRect((Bounds.Width - size)/2 ,(Bounds.Height - size) / 2, size, size));
            ContentView.AddSubview(ImageView);
        }

        public UIImageView ImageView { get; private set; }

        public void UpdateView(UIImage activeImage, UIImage inactiveImage, bool isSelected)
        {
            ImageView.Image = isSelected ? activeImage : inactiveImage;
        }
    }
}

