using System;
using System.IO;
using System.Net;
using Android.App;
using Android.Content;
using Tollminder.Core.Services;

namespace Tollminder.Droid.Services
{
    public class DownloadManager : IFileManager
    {
        private ProgressDialog progressBar;

        public void Download(string uri, string filename, object progressBar)
        {
            var webClient = new WebClient();
            string localFilename = "payhistory.pdf";
            string documentsPathExternalStoragePublicDirectory = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).ToString();
            this.progressBar = (ProgressDialog)progressBar;
            var url = new System.Uri(uri);

            webClient.DownloadDataAsync(url);
            webClient.DownloadDataCompleted += (s, e) =>
            {
                ShowDownloadedComplete();
                var data = e.Result;
                File.WriteAllBytes(Path.Combine(documentsPathExternalStoragePublicDirectory, localFilename), data);
            };
        }

        public void OpenIn(string _documentUrl, string documentName)
        {
        }

        private void ShowDownloadedComplete()
        {
            if (progressBar.IsShowing)
                progressBar.Dismiss();

            AlertDialog.Builder builder = new AlertDialog.Builder(progressBar.Context);
            AlertDialog dialog = null;

            builder.SetMessage("File is downloaded!");
            builder.SetCancelable(true);
            builder.SetPositiveButton("Ok", delegate{
                dialog.Dismiss();
            });
            dialog = builder.Create();
            dialog.Show();
        }
    }
}
