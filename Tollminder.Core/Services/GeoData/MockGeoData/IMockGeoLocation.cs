using Tollminder.Core.Models;

namespace Tollminder.Core.Services.GeoData
{
    public interface IMockGeoLocation
    {
        void PlayPauseIteration();
        void NextTollPoint();
        TollRoad MockTollRoad(string fileName);
    }
}
