using System;
using System.Threading.Tasks;

namespace Tollminder.Core.Services
{
    public interface ICalendarDialog
    {
        Task<DateTime> ShowDialog(DateTime currentValue);
    }
}
