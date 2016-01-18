using UIKit;
using MvvmCross.iOS.Views.Presenters;

namespace Tollminder.Touch
{
	public class AppPresenter : MvxIosViewPresenter
	{
		public AppPresenter(UIApplicationDelegate appDelegate, UIWindow window)
			: base(appDelegate, window)
		{  
		}
	}
}

