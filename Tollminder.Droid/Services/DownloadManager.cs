using System.IO;
using System.Net;
using Tollminder.Core.Services;

namespace Tollminder.Droid.Services
{
    public class DownloadManager : IDownloadManager
    {
        public void Download(string uri, string filename)
        {
            var webClient = new WebClient();
            string localFilename = "payhistory.pdf";
            string documentsPathExternalStoragePublicDirectory = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).ToString();
            webClient.DownloadDataCompleted += (s, e) =>
            {
                var data = e.Result;
                File.WriteAllBytes(Path.Combine(documentsPathExternalStoragePublicDirectory, localFilename), data);
            };
            var url = new System.Uri(uri);
            webClient.DownloadDataAsync(url);
        }
    }
}
