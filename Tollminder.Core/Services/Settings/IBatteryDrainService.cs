namespace Tollminder.Core.Services.Settings
{
    public interface IBatteryDrainService
    {
        bool CheckGpsTrackingSleepTime(double distance);
    }
}

