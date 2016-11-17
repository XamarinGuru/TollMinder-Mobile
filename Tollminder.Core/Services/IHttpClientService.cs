using System;
using System.Threading;
using System.Threading.Tasks;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
    public interface IHttpClientService
    {
        Task<byte[]> FetchAsync(string url);
        Task<byte[]> FetchAsync(string url, IProgress<DownloadBytesProgress> progressReporter);
        Task<byte[]> FetchAsync(string url, IProgress<DownloadBytesProgress> progressReporter, CancellationToken token);
        Task<TResponse> SendAsync<TRequest, TResponse>(TRequest data, string url);
        Task<TResponse> SendAsync<TRequest, TResponse>(TRequest data, string url, IProgress<ProgressCompleted> progress);
        Task<TResponse> SendAsync<TRequest, TResponse>(TRequest data, string url, IProgress<ProgressCompleted> progress, CancellationToken token);
        Task<TResponse> GetAsync<TResponse>(string url, string authToken = null);
        Task<TResponse> GetAsync<TResponse>(string url, CancellationToken token, string authToken = null);
    }
}
