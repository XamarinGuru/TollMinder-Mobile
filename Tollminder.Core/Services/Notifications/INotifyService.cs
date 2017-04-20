using System;
using System.Threading.Tasks;

namespace Tollminder.Core.Services.Notifications
{
    public interface INotifyService
    {
        Task NotifyAsync(string message);
    }
}

