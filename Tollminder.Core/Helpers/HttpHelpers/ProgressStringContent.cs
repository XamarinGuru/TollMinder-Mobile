using System;
using System.Net.Http.Headers;
using System.Text;

namespace Tollminder.Core.Helpers.HttpHelpers
{
    public class ProgressStringContent : ProgressByteArrayContent
    {
        public ProgressStringContent(string content, Action<double> progress)
            : this(content, null, null, progress)
        {
        }

        public ProgressStringContent(string content, Encoding encoding, Action<double> progress)
            : this(content, encoding, null, progress)
        {
        }

        public ProgressStringContent(string content, Encoding encoding, string mediaType, Action<double> progress)
            : base(GetByteArray(content, encoding), progress)
        {
            Headers.ContentType = new MediaTypeHeaderValue(mediaType ?? "text/plain")
            {
                CharSet = (encoding ?? Encoding.UTF8).WebName
            };
        }

        static byte[] GetByteArray(string content, Encoding encoding)
        {
            return (encoding ?? Encoding.UTF8).GetBytes(content);
        }
    }
}
