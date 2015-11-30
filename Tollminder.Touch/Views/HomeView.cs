using System;

using UIKit;
using Cirrious.MvvmCross.Touch.Views;
using Tollminder.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace Tollminder.Touch.Views
{
	public partial class HomeView : MvxViewController
	{
		public HomeViewModel ViewModel { get { return base.ViewModel as HomeViewModel; } }

		public HomeView () : base ("HomeView", null)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			ViewModel.UnSubscribeOnGeolocationUpdate ();
			var set = this.CreateBindingSet<HomeView, HomeViewModel>();
			set.Bind (GeoLabelData).To (v => v.LocationString);
			set.Apply ();
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			ViewModel.SubscribeOnGeolocationUpdate ();
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


