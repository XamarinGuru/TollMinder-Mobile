using Tollminder.Core.Models;

namespace Tollminder.Core.Services.GeoData
{
    public interface IMockGeoLocation
    {
        void NextTollPoint(WaypointAction waypointAction);
    }
}
