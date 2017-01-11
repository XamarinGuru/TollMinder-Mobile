using System;
using Android.App;
using Android.Webkit;
using MvvmCross.Platform;
using Tollminder.Core.Services;

namespace Tollminder.Droid.Services
{
    public class PdfWebViewClient : WebViewClient
    {
        private ProgressDialog progressBar;
        private bool isLoaded = false;

        public PdfWebViewClient(ProgressDialog progressBar)
        {
            this.progressBar = progressBar;
        }

        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {
            if (!isLoaded)
            {
                view.LoadUrl(url);
                isLoaded = true;
            }
            else {
                Mvx.Resolve<IDownloadManager>().Download(url, null);
            }
            view.Reload();
            return false;
        }

        public override void OnPageFinished(WebView view, string url)
        {
            if (progressBar.IsShowing)
                progressBar.Dismiss();
        }
    }
}
