using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Platform;
using Newtonsoft.Json;
using Tollminder.Core.Exceptions;
using Tollminder.Core.Exceptions.Interfaces;
using Tollminder.Core.Models;
using Tollminder.Core.Helpers.HttpHelpers;
using System.Net.Http.Headers;
using System.Linq;
using System.Diagnostics;
using System.Net;

namespace Tollminder.Core.Services.Implementation
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _client;
        private static readonly int BufferSize = 1024;
        private HttpStatusCode statusCode;

        public HttpClientService()
        {
            _client = new HttpClient(Mvx.Resolve<IHttpClientHandlerService>() as HttpClientHandler);
            _client.Timeout = TimeSpan.FromSeconds(10);
            HttpExceptionHandler = new HttpStatusCodeHandler();
        }
        public HttpClient Client
        {
            get { return _client; }
        }

        public IHttpExceptionHandler HttpExceptionHandler { get; set; }
        #region Fetch Methods
        public virtual Task<byte[]> FetchAsync(string url)
        {
            return FetchAsync(url, null);
        }
        public virtual Task<byte[]> FetchAsync(string url, IProgress<DownloadBytesProgress> progress)
        {
            return FetchAsync(url, progress, CancellationToken.None);
        }
        public virtual Task<byte[]> FetchAsync(string url, IProgress<DownloadBytesProgress> progress, CancellationToken token)
        {
            return Task.Run(async () =>
            {
                List<byte> data = new List<byte>();
                var response = await Client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(string.Format("Oops. Something went wrong. (HTTP status code {0}.)", response.StatusCode));
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
        #endregion
        #region SendMethods
        public virtual async Task<TResponse> SendAsync<TRequest, TResponse>(TRequest data, string url, IProgress<ProgressCompleted> progress, CancellationToken token)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    List<byte> byteData = new List<byte>();
                    var jsonSerialization = JsonConvert.SerializeObject(data);
                    Action<double> actionProgress = null;
                    if (progress != null)
                    {
                        actionProgress = (progressValue) => progress.Report(new ProgressCompleted(progressValue));
                    }
                    request.Content = new ProgressStringContent(jsonSerialization, System.Text.Encoding.UTF8, "application/json", actionProgress);
                    using (var response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(false))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            statusCode = response.StatusCode;
                            return default(TResponse);
                        }
                        var total = response.Content.Headers.ContentLength.HasValue ? response.Content.Headers.ContentLength.Value : -1L;
                        var canReportProgress = total != -1 && progress != null;
                        if (!response.IsSuccessStatusCode)
                        {
                            var stringJson = await response.Content.ReadAsStringAsync();

                            var error = JsonConvert.DeserializeObject<ErrorApiResponse>(stringJson);

                            HttpExceptionHandler.Handle(response.StatusCode, error.Message);
                        }
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
                                    if (read != buffer.Length)
                                    {
                                        byte[] cpArray = new byte[read];
                                        Array.Copy(buffer, cpArray, read);
                                        byteData.AddRange(cpArray);
                                    }
                                    else {
                                        byteData.AddRange(buffer);
                                    }
                                    totalRead += read;
                                    if (canReportProgress)
                                    {
                                        progress.Report(new ProgressCompleted((double)totalRead / total * 100));
                                    }
                                }
                            } while (isMoreToRead);
                        }
                        var responseJson = System.Text.Encoding.UTF8.GetString(byteData.ToArray(), 0, byteData.Count);
                        var returnObject = JsonConvert.DeserializeObject<TResponse>(responseJson);
                        return returnObject;
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, ex.StackTrace);
                return default(TResponse);
            }
        }
        public virtual Task<TResponse> SendAsync<TRequest, TResponse>(TRequest data, string url, IProgress<ProgressCompleted> progress)
        {
            return SendAsync<TRequest, TResponse>(data, url, progress, CancellationToken.None);
        }
        public virtual Task<TResponse> SendAsync<TRequest, TResponse>(TRequest data, string url)
        {
            return SendAsync<TRequest, TResponse>(data, url, null);
        }

        public async Task<Profile> CheckProfile<TRequest, TResponse>(TRequest data, string url)
        {
            var result = await SendAsync<TRequest, Profile>(data, url);
            if (result == null)
                return new Tollminder.Core.Models.Profile() { StatusCode = statusCode };
            else
            {
                result.StatusCode = statusCode;
                return result;
            }
        }

        /// <summary>
        /// Sends the async.
        /// </summary>
        /// <returns>Waiting HttpStatusCode for profile saving.</returns>
        /// <param name="data">Data.</param>
        /// <param name="url">URL.</param>
        /// <param name="token">Token.</param>
        public virtual async Task<System.Net.HttpStatusCode> SendAsync<TRequest>(TRequest data, string url, CancellationTokenSource token, string authToken)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Put, url))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Authorization = new AuthenticationHeaderValue(authToken);
                List<byte> byteData = new List<byte>();
                var jsonSerialization = JsonConvert.SerializeObject(data);
                Action<double> actionProgress = null;
                request.Content = new ProgressStringContent(jsonSerialization, System.Text.Encoding.UTF8, "application/json", actionProgress);
                using (var response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, token.Token).ConfigureAwait(false))
                {
                    return response.StatusCode;
                }
            }
        }

        #endregion
        #region GetMethods
        public virtual Task<TResponse> GetAsync<TResponse>(string url, string authToken = null)
        {
            return GetAsync<TResponse>(url, CancellationToken.None, authToken);
        }
        public virtual async Task<TResponse> GetAsync<TResponse>(string url, CancellationToken token, string authToken = null)
        {
            try
            {
                using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    if (!string.IsNullOrEmpty(authToken))
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authToken);

                    using (var response = await Client.SendAsync(requestMessage, token).ConfigureAwait(false))
                    {
                        var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                        if (!response.IsSuccessStatusCode)
                        {
                            var error = JsonConvert.DeserializeObject<ErrorApiResponse>(responseJson);
                            HttpExceptionHandler.Handle(response.StatusCode, error.Message);
                        }
                        var returnObject = JsonConvert.DeserializeObject<TResponse>(responseJson);
                        return returnObject;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, ex.StackTrace);
                return default(TResponse);
            }
        }
        #endregion
    }
}
