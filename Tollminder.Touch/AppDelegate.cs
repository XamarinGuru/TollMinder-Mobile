﻿using Foundation;
using UIKit;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using System;
using Cirrious.MvvmCross.Touch.Platform;
using MessengerHub;

namespace Tollminder.Touch
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate
    {
        UIWindow _window;

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            _window = new UIWindow(UIScreen.MainScreen.Bounds);

			// setup presenter
			var presenter = new AppPresenter(this, _window);
			var setup = new Setup(this, presenter);
			setup.Initialize();

			var startup = Mvx.Resolve<IMvxAppStart>();
            startup.Start();

            _window.MakeKeyAndVisible();

            return true;
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
			Console.WriteLine ("App entering background state.");
			Mvx.Resolve<IMessengerHub>().Publish(new AppInForegroundMessage(this, false));
        }

        public override void WillEnterForeground(UIApplication application)
        {
			Console.WriteLine ("App will enter foreground");
			Mvx.Resolve<IMessengerHub>().Publish(new AppInForegroundMessage(this, true));
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }
    }
}

