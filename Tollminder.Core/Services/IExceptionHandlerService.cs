using System;
using System.Threading.Tasks;
using Tollminder.Core.Exceptions;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
    public interface IExceptionHandlerService
    {
        void HandleException(Exception ex);
        void HandleException(ServerUnavailableException ex);
        void HandleException(TaskCanceledException ex);
        void HandleExceptionWithoutNotify(Exception ex);
    }
}
