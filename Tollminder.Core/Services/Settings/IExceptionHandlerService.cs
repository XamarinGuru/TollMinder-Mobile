using System;
using System.Threading.Tasks;
using Tollminder.Core.Exceptions;

namespace Tollminder.Core.Services.Settings
{
    public interface IExceptionHandlerService
    {
        void HandleException(Exception ex);
        void HandleException(ServerUnavailableException ex);
        void HandleException(TaskCanceledException ex);
        void HandleExceptionWithoutNotify(Exception ex);
    }
}
