using Foundation;
using UIKit;
using System;
using MvvmCross.iOS.Platform;
using MvvmCross.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Models;
using AVFoundation;
using System.Threading.Tasks;
using Tollminder.Core.Services;
using Tollminder.Core.ServicesHelpers;
using Google.Core;
using Google.SignIn;

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

			if (UIDevice.CurrentDevice.CheckSystemVersion (8,0)) {
				var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes (
					UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null
				);
				application.CancelAllLocalNotifications ();
				application.RegisterUserNotificationSettings (notificationSettings);
			}

            var session = AVAudioSession.SharedInstance ();
            NSError categoryError;
            session.SetCategory (AVAudioSessionCategory.Playback);
            session.SetActive (true, out categoryError);

            string clientId = "194561500997971";

            NSError configureError;
            Context.SharedInstance.Configure(out configureError);
            if (configureError != null)
            {
                // If something went wrong, assign the clientID manually
                Console.WriteLine("Error configuring the Google context: {0}", configureError);
                SignIn.SharedInstance.ClientID = clientId;
            }

            return true;
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            return SignIn.SharedInstance.HandleUrl(url, sourceApplication, annotation);
        }

		public override void ReceivedLocalNotification (UIApplication application, UILocalNotification notification)
		{
			
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
			Mvx.Resolve<IMvxMessenger> ().Publish (new AppInBackgroundMessage (this));
            nint taskID = UIApplication.SharedApplication.BeginBackgroundTask(() => { });
            new Task(() =>
            {
                CheckBatteryDrainTimeout();
                UIApplication.SharedApplication.EndBackgroundTask(taskID);
            }).Start();
        }

        public override void WillEnterForeground(UIApplication application)
        {
			Console.WriteLine ("App will enter foreground");
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

        void CheckBatteryDrainTimeout()
        {
            Mvx.Trace("CheckBatteryDrainTimeout from background");
            Mvx.Resolve<ITrackFacade>().StopServices();
            Mvx.Resolve<ITrackFacade>().StartServices();
            Mvx.Resolve<INotificationSender>().SendLocalNotification("Background mode", $"App is still working");
            Task.Delay(600000).Wait();
            nint taskID = UIApplication.SharedApplication.BeginBackgroundTask(() => { });
            new Task(() =>
            {
                CheckBatteryDrainTimeout();
                UIApplication.SharedApplication.EndBackgroundTask(taskID);

            }).Start();
        }
    }
}


