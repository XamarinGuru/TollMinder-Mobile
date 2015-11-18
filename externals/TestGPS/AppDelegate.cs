using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using MonoTouch.CoreLocation;
using System.ComponentModel;




namespace TestGPS
{




	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;

		HomeViewControler  homeController{ get; set;}
		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen siz
			RectangleF rect = new RectangleF (0,0,320,598);
			window = new UIWindow (rect);
			homeController=new HomeViewControler();
			           UINavigationController navigation = new UINavigationController(homeController);
			// If you have defined a root view controller, set it here:
			// window.RootViewController = myViewController;
			
			// make the window visible
			window.RootViewController = navigation;
			window.BackgroundColor = UIColor.White;
			window.MakeKeyAndVisible ();



			return true;
		}
		public override void OnActivated(UIApplication application)
		{

//			UIAlertView alert = new UIAlertView (@"Alert", @"", null, "ok", null);
//			alert.Show ();
			Console.WriteLine("OnActivated called, App is active.");
			homeController.updateLocation ();
		}
		public override void WillEnterForeground(UIApplication application)
		{
			Console.WriteLine("App will enter foreground");
		}
		public override void OnResignActivation(UIApplication application)
		{
			Console.WriteLine("OnResignActivation called, App moving to inactive state.");
		}

		public override void DidEnterBackground (UIApplication application) {
			int taskID = UIApplication.SharedApplication.BeginBackgroundTask( () => {});
			new Task ( () => {
				DoWork();
				UIApplication.SharedApplication.EndBackgroundTask(taskID);
			}).Start();
		}
		public void DoWork(){
			InvokeOnMainThread (()=>{

				homeController.updateLocation ();

			});
		}
		// not guaranteed that this will run
		public override void WillTerminate(UIApplication application)
		{
			Console.WriteLine("App is terminating.");
		}
	}
}

