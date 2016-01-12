using System;
using System.Net.Http;
using System.Threading.Tasks;
using Tollminder.Core.Models;
using System.Collections.Generic;
using Tollminder.Core.Helpers;

namespace Tollminder.Core.Services.Implementation
{
	public class HttpService : IHttpService
	{
		private readonly HttpClient _client;
		public HttpClient Client {
			get { return _client; }
		}


		public static readonly int BufferSize = 1024;

		public HttpService ()
		{
			_client = new HttpClient (new ModernHttpClient.NativeMessageHandler ());
		}

		#region IHttpService implementation
		public Task<byte[]> FetchAsync (string url)
		{
			return FetchAsync (url, null);
		}
		public Task<byte[]> FetchAsync (string url, IProgress<DownloadBytesProgress> progressReporter)
		{
			return Task.Run (async () => {
				int receivedBytes = 0;
				int totalBytes = 0;
				List<byte> data = new List<byte> ();
				using (var httpResponse = await Client.GetAsync (url)) {
					using (var stream = await httpResponse.Content.ReadAsStreamAsync ()) {
					totalBytes = (int)stream.Length;
					byte[] buffer = new byte[BufferSize];
					while (true) {
						int bytesRead = await stream.ReadAsync (buffer, 0, buffer.Length);
						if (bytesRead == 0) {			
							await Task.Yield ();
							break;
						}
						data.AddRange (buffer);
						receivedBytes += bytesRead;
						if (progressReporter != null) {
							DownloadBytesProgress args = new DownloadBytesProgress (url, receivedBytes, totalBytes);
							progressReporter.Report (args);
						}
					}
						return data.ToArray ();
					}
				}
			});

		}
		#endregion
	}
}

