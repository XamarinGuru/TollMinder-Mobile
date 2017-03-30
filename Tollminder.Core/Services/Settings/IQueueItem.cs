using System;
namespace Tollminder.Core.Services.Settings
{
    public interface IQueueItem
    {
        ItemPriority Priority { get; }
    }

    public enum ItemPriority
    {
        FirstAlways,
        Any,
        LastAlways
    }
}
