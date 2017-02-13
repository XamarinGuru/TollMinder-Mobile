using System;
using System.IO;
using System.Net;
using System.Text;
using Foundation;
using QuickLook;
using Tollminder.Core.Services;
using Tollminder.Touch.Views;
using UIKit;

namespace Tollminder.Touch.Services
{
    public class DownloadManager : IFileManager
    {
        UINavigationController NavigationController;

        public void Download(string uri, string filename = null, object progressBar = null)
        {
            NavigationController = new UINavigationController();
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            var url = new Uri(uri);

            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string localPath = Path.Combine(documentsPath, filename);

            webClient.DownloadDataAsync(url);
            webClient.DownloadDataCompleted += (s, e) =>
            {
                var data = e.Result; // get the downloaded text
                File.WriteAllBytes(localPath, data);
                new UIAlertView("Done", "File downloaded and saved", null, "OK", null).Show();
            };
        }

        public void OpenIn(string _documentUrl, string documentName)
        {
            
            var topViewController = UIApplication.SharedApplication.KeyWindow;
            var currentViewController = topViewController.RootViewController;
            UIActivityViewController activityVC = new UIActivityViewController(
                new NSObject[] { new NSString(documentName), new NSUrl(_documentUrl) }, null);
            if(currentViewController != null)
                currentViewController.PresentViewController(activityVC, true, null);
        }
    }
}
