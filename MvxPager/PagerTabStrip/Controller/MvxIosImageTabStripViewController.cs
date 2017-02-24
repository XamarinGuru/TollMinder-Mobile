using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using MvxPagerTabStrip.Cells;
using MvxPagerTabStrip.Controller;
using MvxPagerTabStrip.Models;
using UIKit;

namespace MvxPagerTabStrip.Controller
{
    public class MvxIosImageTabStripViewController<TViewModel> : MvxIosButtonTabStripViewController<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public List<UIImage> ActiveImages { get; private set; }
        public List<UIImage> InactiveImages { get; private set; }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            ButtonBarView.RegisterClassForCell(typeof(ImageBarViewCell), ImageBarViewCell.Key);
            var cell = collectionView.DequeueReusableCell(ImageBarViewCell.Key, indexPath) as ImageBarViewCell;
            cell.UpdateView(ActiveImages[indexPath.Row], InactiveImages[indexPath.Row], indexPath.Row == CurrentIndex);
            return cell;
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            CurrentIndex = indexPath.Row;
            base.ItemSelected(collectionView, indexPath);
            collectionView.ReloadData();
        }

        public override void UpdatePage(nint fromIndex, nint toIndex, nfloat withProgressPercentage)
        {
            base.UpdatePage(fromIndex, toIndex, withProgressPercentage);
            if (_shouldUpdateButtonBarView)
                ButtonBarView.ReloadData();
        }

        public override void InitPagerTabStrip(params MvxPagerTab[] mvxViews)
        {
            ActiveImages = new List<UIImage>();
            InactiveImages = new List<UIImage>();
            foreach (var item in mvxViews)
            {
                var imageTab = (item as MvxPagerImageTab);
                if (imageTab != null)
                {
                    ActiveImages.Add(imageTab.ActiveImage);
                    InactiveImages.Add(imageTab.InactiveImage);
                }
            }

            base.InitPagerTabStrip(mvxViews);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (ActiveImages != null)
                {
                    foreach (var item in ActiveImages)
                        item.Dispose();
                    ActiveImages = null;
                }

                if(InactiveImages != null){
                    foreach (var item in InactiveImages)
                        item.Dispose();
                    InactiveImages = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}
