using System;
using System.Threading.Tasks;
using Tollminder.Core.Models;
using System.Threading;

namespace Tollminder.Core.Services
{
	public interface IHttpService
	{
		Task<byte[]> FetchAsync (string url);
		Task<byte[]> FetchAsync (string url, CancellationToken token);
		Task<byte[]> FetchAsync (string url, IProgress<DownloadBytesProgress> progressReporter);
		Task<byte[]> FetchAsync (string url, IProgress<DownloadBytesProgress> progressReporter, CancellationToken token);
	}
}

