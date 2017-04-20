using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services.Api
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _client;
        public HttpClient Client
        {
            get { return _client; }
        }


        public static readonly int BufferSize = 4096;

        public HttpService()
        {
            _client = new HttpClient(new ModernHttpClient.NativeMessageHandler());
        }

        #region IHttpService implementation
        public Task<byte[]> FetchAsync(string url)
        {
            return FetchAsync(url, null);
        }

        public Task<byte[]> FetchAsync(string url, CancellationToken token)
        {
            return FetchAsync(url, null, token);
        }

        public Task<byte[]> FetchAsync(string url, IProgress<DownloadBytesProgress> progress)
        {
            return Task.Run(async () =>
            {
                List<byte> data = new List<byte>();
                var response = await Client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(string.Format("The request returned with HTTP status code {0}", response.StatusCode));
                }

                var total = response.Content.Headers.ContentLength.HasValue ? response.Content.Headers.ContentLength.Value : -1L;
                var canReportProgress = total != -1 && progress != null;

                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    var totalRead = 0L;
                    var buffer = new byte[BufferSize];
                    var isMoreToRead = true;

                    do
                    {
                        var read = await stream.ReadAsync(buffer, 0, buffer.Length);
                        if (read == 0)
                        {
                            isMoreToRead = false;
                        }
                        else
                        {
                            data.AddRange(buffer);
                            totalRead += read;
                            if (canReportProgress)
                            {
                                progress.Report(new DownloadBytesProgress(url, totalRead, total));
                            }
                        }
                    } while (isMoreToRead);
                }
                return data.ToArray();
            });
        }

        public Task<byte[]> FetchAsync(string url, IProgress<DownloadBytesProgress> progress, CancellationToken token)
        {
            return Task.Run(async () =>
            {
                List<byte> data = new List<byte>();
                var response = await Client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, token);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(string.Format("The request returned with HTTP status code {0}", response.StatusCode));
                }

                var total = response.Content.Headers.ContentLength.HasValue ? response.Content.Headers.ContentLength.Value : -1L;
                var canReportProgress = total != -1 && progress != null;

                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    var totalRead = 0L;
                    var buffer = new byte[BufferSize];
                    var isMoreToRead = true;

                    do
                    {
                        token.ThrowIfCancellationRequested();

                        var read = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                        if (read == 0)
                        {
                            isMoreToRead = false;
                        }
                        else
                        {
                            data.AddRange(buffer);
                            totalRead += read;
                            if (canReportProgress)
                            {
                                progress.Report(new DownloadBytesProgress(url, totalRead, total));
                            }
                        }
                    } while (isMoreToRead);
                }
                return data.ToArray();
            });
        }

        public async Task<TResponse> GetAsync<TResponse>(string url, CancellationToken token, IProgress<DownloadBytesProgress> progress = null) where TResponse : class
        {
            var stringBuilder = new StringBuilder();
            var response = await Client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(string.Format("The request returned with HTTP status code {0}", response.StatusCode));
            }

            var total = response.Content.Headers.ContentLength.HasValue ? response.Content.Headers.ContentLength.Value : -1L;
            var canReportProgress = total != -1 && progress != null;

            using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
            {
                var totalRead = 0L;
                var buffer = new byte[BufferSize];
                var isMoreToRead = true;

                do
                {
                    token.ThrowIfCancellationRequested();

                    var read = await stream.ReadAsync(buffer, 0, buffer.Length, token).ConfigureAwait(false);
                    if (read == 0)
                    {
                        isMoreToRead = false;
                    }
                    else
                    {
                        totalRead += read;
                        if (canReportProgress)
                        {
                            progress.Report(new DownloadBytesProgress(url, totalRead, total));
                        }
                    }
                    stringBuilder.Append(Encoding.UTF8.GetString(buffer, 0, buffer.Length));
                } while (isMoreToRead);
            }
            var returnObject = JsonConvert.DeserializeObject<TResponse>(stringBuilder.ToString());
            return returnObject;
        }
        #endregion
    }
}

