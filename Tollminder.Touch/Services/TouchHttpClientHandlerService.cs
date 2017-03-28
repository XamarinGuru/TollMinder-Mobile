using ModernHttpClient;
using Tollminder.Core.Services.Api;

namespace Tollminder.Touch.Services
{
    public class TouchHttpClientHandlerService : NativeMessageHandler, IHttpClientHandlerService
    {
        public TouchHttpClientHandlerService() : base(false, false)
        { }
    }
}