using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tollminder.Core.Helpers.HttpHelpers
{
    public class ProgressByteArrayContent : HttpContent
    {
        readonly byte[] content;
        readonly int offset, count;
        const int chunkSize = 1024;
        readonly Action<double> progress;

        public virtual bool CanProgress
        {
            get { return progress != null; }
        }

        public ProgressByteArrayContent(byte[] content, Action<double> progress)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            this.progress = progress;
            this.content = content;
            this.count = content.Length;
        }

        public ProgressByteArrayContent(byte[] content, int offset, int count, Action<double> progress)
            : this(content, progress)
        {
            if (offset < 0 || offset > this.count)
                throw new ArgumentOutOfRangeException(nameof(offset));

            if (count < 0 || count > (this.count - offset))
                throw new ArgumentOutOfRangeException(nameof(count));

            this.offset = offset;
            this.count = count;
        }

        protected override Task<Stream> CreateContentReadStreamAsync()
        {
            return Task.FromResult<Stream>(new MemoryStream(content, offset, count));
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return Task.Run(async () =>
            {
                for (int i = 0; i < this.content.Length; i += chunkSize)
                {
                    await stream.WriteAsync(this.content, i, Math.Min(chunkSize, this.content.Length - i));
                    if (CanProgress)
                        this.progress(((double)i + 1) / this.content.Length);
                }
            });
        }

        protected override bool TryComputeLength(out long length)
        {
            length = count;
            return true;
        }
    }
}
