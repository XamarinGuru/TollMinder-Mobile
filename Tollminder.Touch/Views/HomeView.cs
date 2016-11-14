using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using Tollminder.Core.ViewModels;
using Tollminder.Core.Helpers;
using Foundation;
using Tollminder.Touch.Interfaces;
using Tollminder.Core.Converters;

namespace Tollminder.Touch.Views
{
	public partial class HomeView : MvxViewController, ICleanBackStack
	{	
		#pragma warning disable 108		
		public new HomeViewModel ViewModel { get { return base.ViewModel as HomeViewModel; } }
		#pragma warning restore 108

		public HomeView () : base ("HomeView", null)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			NavigationController.NavigationBar.Translucent = false;
			LogArea.Font = UIKit.UIFont.FromName ("Helvetica", 12f);
			AutomaticallyAdjustsScrollViewInsets = true;

			var set = this.CreateBindingSet<HomeView, HomeViewModel>();
			set.Bind (GeoLabelData).To (v => v.LocationString);
			set.Bind (StartButton).To (v => v.StartCommand);
			set.Bind (StopButton).To (v => v.StopCommand);
			set.Bind (ActivityLabel).To (v => v.MotionTypeString);
			set.Bind(StartButton).For(x => x.Enabled).To(x => x.IsBound).WithConversion(new BoolInverseConverter());
			set.Bind(StopButton).For(x => x.Enabled).To(x => x.IsBound);
			set.Bind (LogArea).To (v => v.LogText);
			set.Bind(StatusLabel).To(v => v.StatusString);
            set.Bind(TollRoadString).To(v => v.TollRoadString);
            set.Bind(NextWaypointString).To(v => v.CurrentWaypointString);
            set.Bind(LogOut).To(v => v.LogOutCommand);
			set.Apply ();

			this.AddLinqBinding(ViewModel, vm => vm.LogText, (value) =>
			{
				NSRange bottom = new NSRange(0, value?.Length ?? 0);
				LogArea.ScrollRangeToVisible(bottom);
			});

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


