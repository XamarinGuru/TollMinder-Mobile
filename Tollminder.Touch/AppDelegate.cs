﻿using Foundation;
using UIKit;
using System;
using MvvmCross.iOS.Platform;
using MvvmCross.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Models;
using AVFoundation;
using System.Threading.Tasks;
using Tollminder.Core.ServicesHelpers;
using Google.SignIn;
using Tollminder.Core.Services.Notifications;

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

            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(
                    UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null
                );
                application.CancelAllLocalNotifications();
                application.RegisterUserNotificationSettings(notificationSettings);
            }

            var session = AVAudioSession.SharedInstance();
            NSError categoryError;
            session.SetCategory(AVAudioSessionCategory.Playback);
            session.SetActive(true, out categoryError);
            // check for a notification
            if (launchOptions != null)
            {
                // check for a local notification
                if (launchOptions.ContainsKey(UIApplication.LaunchOptionsLocalNotificationKey))
                {
                    var localNotification = launchOptions[UIApplication.LaunchOptionsLocalNotificationKey] as UILocalNotification;
                    if (localNotification != null)
                    {
                        UIAlertController okayAlertController = UIAlertController.Create(localNotification.AlertAction, localNotification.AlertBody, UIAlertControllerStyle.Alert);
                        okayAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

                        Window.RootViewController.PresentViewController(okayAlertController, true, null);

                        // reset our badge
                        UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
                    }
                }
            }

            return true;//Facebook.CoreKit.ApplicationDelegate.SharedInstance.FinishedLaunching(application, launchOptions);

        }

        public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
        {
            // show an alert
            //UIAlertController okayAlertController = UIAlertController.Create(notification.AlertAction, notification.AlertBody, UIAlertControllerStyle.Alert);
            //okayAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

            //Window.RootViewController.PresentViewController(okayAlertController, true, null);

            //// reset our badge
            //UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
        }

        SocialNetworks GetSocialNetworkForUrl(NSUrl url)
        {
            if (url.Scheme == GetUrlScheme(SocialNetworks.Facebook))
            {
                return SocialNetworks.Facebook;
            }
            if (url.Scheme == GetUrlScheme(SocialNetworks.GooglePlus))
            {
                return SocialNetworks.GooglePlus;
            }

            return SocialNetworks.Unknown;
        }

        string GetUrlScheme(SocialNetworks socialNetwork)
        {
            var predicate = NSPredicate.FromFormat("%K = %@", new NSObject[] { new NSString("CFBundleURLName"), new NSString(socialNetwork.ToString()) });
            var urlTypes = (NSMutableArray)NSBundle.MainBundle.InfoDictionary.ObjectForKey(new NSString("CFBundleURLTypes"));
            var urlSchemes = (NSArray)urlTypes.Filter(predicate).ValueForKey(new NSString("CFBundleURLSchemes"));
            var neededScheme = urlSchemes.GetItem<NSMutableArray>(0).GetItem<NSMutableString>(0).ToString();
            return neededScheme;
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            switch (GetSocialNetworkForUrl(url))
            {
                case SocialNetworks.Facebook:
                    return Facebook.CoreKit.ApplicationDelegate.SharedInstance.OpenUrl(application, url, sourceApplication, annotation);
                case SocialNetworks.GooglePlus:
                    return SignIn.SharedInstance.HandleUrl(url, sourceApplication, annotation);
            }
            NSNotificationCenter.DefaultCenter.PostNotificationName("OpenUrl", url);

            return base.OpenUrl(application, url, sourceApplication, annotation);
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
            Console.WriteLine("App entering background state.");
            Mvx.Resolve<IMvxMessenger>().Publish(new AppInBackgroundMessage(this));
            nint taskID = UIApplication.SharedApplication.BeginBackgroundTask(() => { });
            new Task(() =>
            {
                CheckBatteryDrainTimeout();
                UIApplication.SharedApplication.EndBackgroundTask(taskID);
            }).Start();
        }

        public override void WillEnterForeground(UIApplication application)
        {
            Console.WriteLine("App will enter foreground");
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

        async void CheckBatteryDrainTimeout()
        {
            Mvx.Trace("CheckBatteryDrainTimeout from background");
            Mvx.Resolve<ITrackFacade>().StopServices();
            Mvx.Resolve<ITrackFacade>().StartServicesAsync();
            Mvx.Resolve<INotificationSender>().SendLocalNotification("Background mode", $"App is still working");
            await Task.Delay(600000);
            nint taskID = UIApplication.SharedApplication.BeginBackgroundTask(() => { });
            new Task(() =>
            {
                CheckBatteryDrainTimeout();
                UIApplication.SharedApplication.EndBackgroundTask(taskID);

            }).Start();
        }

        enum SocialNetworks
        {
            Unknown,
            Facebook,
            GooglePlus
        }
    }
}