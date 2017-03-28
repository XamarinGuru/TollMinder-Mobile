using System;
using System.Threading.Tasks;

namespace Tollminder.Core.Services.Notifications
{
    public interface ICalendarDialog
    {
        Task<DateTime> ShowDialogAsync(DateTime currentValue);
    }
}
