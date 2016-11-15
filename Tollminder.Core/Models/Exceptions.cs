using System;
namespace Tollminder.Core.Models
{
    public class Exceptions
    {
        public class ApiException : Exception
        {
            public ApiException(string message) : base(message)
            {
            }
        }

        public class UiApiException : Exception
        {
            public string Title { get; set; }

            public UiApiException(string message) : base(message)
            {
            }
        }
    }
}
