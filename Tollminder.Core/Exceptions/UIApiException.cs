using System;

namespace Tollminder.Core.Exceptions
{
    public class UiApiException : Exception
    {
        public string Title { get; set; }

        public UiApiException(string title, string message) : base(message)
        {
            Title = title;
        }
    }
}
