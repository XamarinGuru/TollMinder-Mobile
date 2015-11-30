using System;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using UIKit;

namespace Tollminder.Touch
{
	public class AppPresenter : MvxTouchViewPresenter
	{
		public AppPresenter(UIApplicationDelegate appDelegate, UIWindow window)
			: base(appDelegate, window)
		{  
		}
	}
}

