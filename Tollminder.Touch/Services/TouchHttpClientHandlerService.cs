using System;
using ModernHttpClient;
using Tollminder.Core.Services;

namespace Tollminder.Touch.Services
{
    public class TouchHttpClientHandlerService : NativeMessageHandler, IHttpClientHandlerService
    {
        public TouchHttpClientHandlerService() : base(false, false)
        { }
    }
}