using System;
using System.Net;
using Tollminder.Core.Exceptions.Interfaces;

namespace Tollminder.Core.Exceptions
{
    public class HttpStatusCodeHandler : IHttpExceptionHandler
    {
        public void Handle(HttpStatusCode httpStatusCode, string message = null)
        {
            switch (httpStatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UiApiException("Unauthorized", message);
                case HttpStatusCode.Ambiguous:
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.Conflict:
                case HttpStatusCode.Continue:
                case HttpStatusCode.Created:
                case HttpStatusCode.ExpectationFailed:
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.Found:
                case HttpStatusCode.Gone:
                case HttpStatusCode.HttpVersionNotSupported:
                case HttpStatusCode.InternalServerError:
                case HttpStatusCode.LengthRequired:
                case HttpStatusCode.MethodNotAllowed:
                case HttpStatusCode.Moved:
                case HttpStatusCode.NoContent:
                case HttpStatusCode.NonAuthoritativeInformation:
                case HttpStatusCode.NotAcceptable:
                case HttpStatusCode.NotFound:
                case HttpStatusCode.NotImplemented:
                case HttpStatusCode.NotModified:
                case HttpStatusCode.PartialContent:
                case HttpStatusCode.PaymentRequired:
                case HttpStatusCode.PreconditionFailed:
                case HttpStatusCode.ProxyAuthenticationRequired:
                case HttpStatusCode.RedirectKeepVerb:
                case HttpStatusCode.RedirectMethod:
                case HttpStatusCode.RequestedRangeNotSatisfiable:
                case HttpStatusCode.RequestEntityTooLarge:
                case HttpStatusCode.RequestUriTooLong:
                case HttpStatusCode.ResetContent:
                case HttpStatusCode.SwitchingProtocols:
                case HttpStatusCode.UnsupportedMediaType:
                case HttpStatusCode.Unused:
                case HttpStatusCode.UseProxy:
                    throw new ApiException($"Oops. Something went wrong. (HTTP status code {httpStatusCode}");
                case HttpStatusCode.RequestTimeout:
                case HttpStatusCode.ServiceUnavailable:
                case HttpStatusCode.GatewayTimeout:
                case HttpStatusCode.BadGateway:
                    throw new ServerUnavailableException();
            }
        }
    }
}
