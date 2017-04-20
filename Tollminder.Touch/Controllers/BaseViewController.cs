using System;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using Tollminder.Core.ViewModels;
using Tollminder.Touch.Extensions;
using UIKit;

namespace Tollminder.Touch.Controllers
{
    public class BaseViewController<TViewModel> : MvxTextFieldResponderController<TViewModel> where TViewModel : BaseViewModel
    {
        UIActivityIndicatorView _activityView;

        public BaseViewController() : base()
        {
        }

        public BaseViewController(IntPtr handle)
            : base(handle)
        {
        }

        public BaseViewController(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            AddIndicatorView();
            //NavigationItem.BackBarButtonItem = new UIBarButtonItem(string.Empty, UIBarButtonItemStyle.Plain, null);
            this.CreateBinding(this).For(v => v.Title).To((BaseViewModel vm) => vm.Title).Apply();

            InitializeObjects();
            InitializeBindings();
        }

        protected void SetBackground(string imagePath)
        {
            var imageViewBackground = new UIImageView(new CGRect(0, 0, View.Frame.Width, View.Frame.Height));
            imageViewBackground.Image = new UIImage(imagePath);
            imageViewBackground.ContentMode = UIViewContentMode.ScaleAspectFill;

            View.AddIfNotNull(imageViewBackground);
            View.SendSubviewToBack(imageViewBackground);
            View.AddConstraints(
                imageViewBackground.WithSameWidth(View),
                imageViewBackground.WithSameHeight(View)
            );
        }

        protected virtual void InitializeObjects()
        {
        }

        protected virtual void InitializeBindings()
        {
        }

        protected virtual bool ShowActivityIndicator { get; set; } = false;

        void AddIndicatorView()
        {
            _activityView = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
            _activityView.Alpha = 0.5f;
            _activityView.HidesWhenStopped = true;
            _activityView.BackgroundColor = UIColor.Black;
            View.AddIfNotNull(_activityView);
            View.AddConstraints(
                _activityView.AtTopOf(View),
                _activityView.AtLeftOf(View),
                _activityView.AtRightOf(View),
                _activityView.AtBottomOf(View)
            );
        }

        public override void DidMoveToParentViewController(UIKit.UIViewController parent)
        {
            if (parent == null)
                Dispose(true);

            base.DidMoveToParentViewController(parent);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ViewModel.OnResume();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            if (IsViewLoaded)
                ViewModel.OnPause();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                ViewModel?.OnDestroy();

            base.Dispose(disposing);
        }
    }
}
