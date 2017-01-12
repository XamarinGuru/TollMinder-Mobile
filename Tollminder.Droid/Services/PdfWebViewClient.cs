using System;
using Android.App;
using Android.Content;
using Android.Webkit;
using MvvmCross.Platform;
using Tollminder.Core.Services;

namespace Tollminder.Droid.Services
{
    public class PdfWebViewClient : WebViewClient
    {
        private ProgressDialog progressBar;
        private Context context;
        private bool isLoaded = false;

        public PdfWebViewClient(ProgressDialog progressBar)
        {
            this.progressBar = progressBar;
            context = progressBar.Context;
        }

        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {
            if (!isLoaded)
            {
                view.LoadUrl(url);
                isLoaded = true;
            }
            else {
                Mvx.Resolve<IDownloadManager>().Download(url, null, ProgressDialog.Show(context, "Pdf file", "Downloading..."));
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
