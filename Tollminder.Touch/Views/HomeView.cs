using System;

using UIKit;
using Cirrious.MvvmCross.Touch.Views;

namespace Tollminder.Touch.Views
{
	public partial class HomeView : MvxViewController
	{
		public HomeView () : base ("HomeView", null)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.

		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


