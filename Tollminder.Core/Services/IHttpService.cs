using System;
using System.Threading.Tasks;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
	public interface IHttpService
	{
		Task<byte[]> FetchAsync (string url);
		Task<byte[]> FetchAsync (string url, IProgress<DownloadBytesProgress> progressReporter);
	}
}

