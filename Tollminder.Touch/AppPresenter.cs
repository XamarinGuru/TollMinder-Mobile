using UIKit;
using MvvmCross.iOS.Views.Presenters;
using Tollminder.Touch.Views;
using Tollminder.Touch.Interfaces;

namespace Tollminder.Touch
{
	public class AppPresenter : MvxIosViewPresenter
	{
		public AppPresenter(UIApplicationDelegate appDelegate, UIWindow window)
			: base(appDelegate, window)
		{  
		}

        public override void Show(MvvmCross.iOS.Views.IMvxIosView view)
        {
            if (view is ICleanBackStack)
            {
                ShowFirstView(view as UIViewController);
                return;
            }
            base.Show(view);
        }

        public override void Show(MvvmCross.Core.ViewModels.MvxViewModelRequest request)
        {
            base.Show(request);
        }
	}
}

