using System;

using UIKit;
using Cirrious.MvvmCross.Touch.Views;
using Tollminder.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace Tollminder.Touch.Views
{
	public partial class HomeView : MvxViewController
	{	
		#pragma warning disable 108		
		public HomeViewModel ViewModel { get { return base.ViewModel as HomeViewModel; } }
		#pragma warning restore 108

		public HomeView () : base ("HomeView", null)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			NavigationController.NavigationBar.Translucent = false;

			AutomaticallyAdjustsScrollViewInsets = true;

			ViewModel.StartActivityDetection ();

			var set = this.CreateBindingSet<HomeView, HomeViewModel>();
			set.Bind (GeoLabelData).To (v => v.Percent);
			set.Bind (StartButton).To (v => v.StoppCommand);
			set.Bind (StopButton).To (v => v.StopCommand);
			set.Bind (ActivityLabel).To (v => v.MotionTypeString);
			set.Apply ();

			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			GeoLabelData.Text = ViewModel.Location?.ToString ();
		}

		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();

		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


