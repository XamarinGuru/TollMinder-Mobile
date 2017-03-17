using System;
using System.Threading.Tasks;

namespace Tollminder.Core.Services
{
    public interface ICalendarDialog
    {
        Task<DateTime> ShowDialogAsync(DateTime currentValue);
    }
}
