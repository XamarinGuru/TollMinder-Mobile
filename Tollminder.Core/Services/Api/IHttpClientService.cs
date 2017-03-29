using System;
using System.Threading;
using System.Threading.Tasks;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services.Api
{
    public interface IHttpClientService
    {
        Task<byte[]> FetchAsync(string url);
        Task<byte[]> FetchAsync(string url, IProgress<DownloadBytesProgress> progressReporter);
        Task<byte[]> FetchAsync(string url, IProgress<DownloadBytesProgress> progressReporter, CancellationToken token);
        Task<TResponse> SendAsync<TRequest, TResponse>(TRequest data, string url);
        Task<TResponse> SendAsync<TRequest, TResponse>(TRequest data, string url, string authToken);
        Task<TResponse> SendAsync<TRequest, TResponse>(TRequest data, string url, string authToken, IProgress<ProgressCompleted> progress);
        Task<TResponse> SendAsync<TRequest, TResponse>(TRequest data, string url, string authToken, IProgress<ProgressCompleted> progress, CancellationToken token);
        Task<TResponse> GetAsync<TResponse>(string url, string authToken = null);
        Task<TResponse> GetAsync<TResponse>(string url, CancellationToken token, string authToken = null);
        Task DeleteAsync<TRequest>(TRequest data, string url, CancellationToken token);
    }
}
