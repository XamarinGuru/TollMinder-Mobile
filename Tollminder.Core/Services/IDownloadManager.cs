using System;
namespace Tollminder.Core.Services
{
    public interface IDownloadManager
    {
        /// <summary>
        /// Do Download
        /// </summary>
        /// <param name="uri">Full Url to Download</param>
        /// <param name="filename">Name of File</param>
        /// <param name="progressBar">Dialog to show download progress</param>
        void Download(string uri, string filename = null, object progressBar = null);
    }
}
