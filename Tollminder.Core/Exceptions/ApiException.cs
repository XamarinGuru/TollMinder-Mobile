using System;
namespace Tollminder.Core.Exceptions
{
    public class ApiException : Exception
    {
        public ApiException(string message) : base(message)
        {
        }
    }
}
