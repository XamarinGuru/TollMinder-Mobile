using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Webkit;
using Tollminder.Droid.Services;

namespace Tollminder.Droid.Controls
{
    [Register("tollminder.droid.controls.PdfView")]
    public class PdfView: WebView
    {
        private string _url;
        private ProgressDialog progressBar;
        
        public PdfView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            progressBar = ProgressDialog.Show(context, "Pay history", "Loading...");
            SetWebViewClient(new PdfWebViewClient(progressBar));
            Settings.AllowFileAccess = true;
            Settings.JavaScriptEnabled = true;
            Settings.BuiltInZoomControls = true;
        }

        public string PdfUrl
        {
            get { return _url; }
            set
            {
                if (string.IsNullOrEmpty(value)) 
                    return;

                _url = value;
                LoadUrl("http://docs.google.com/viewer?url=" + _url);
            }
        }
        public override void SetDownloadListener(IDownloadListener listener)
        {
            base.SetDownloadListener(listener);
        }
    }
}
