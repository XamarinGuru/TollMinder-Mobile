using System;
namespace Tollminder.Core.Exceptions.Interfaces
{
    public interface IHttpExceptionHandler
    {
        void Handle(System.Net.HttpStatusCode httpStatusCode, string message = null);
    }
}
