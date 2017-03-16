namespace Tollminder.Core.Services
{
    public interface IBatteryDrainService
    {
        bool CheckGpsTrackingSleepTime(double distance);
    }
}

