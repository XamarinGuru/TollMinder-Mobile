using System.Collections.Generic;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services.RoadsProcessing
{
    public interface IDistanceChecker
    {
        List<TollPointWithDistance> GetMostClosestTollPoint(GeoLocation center, IList<TollPoint> points, double radius = double.MaxValue);
    }
}

