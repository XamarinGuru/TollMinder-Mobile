using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
	public interface IDistanceChecker
	{
        List<TollPointWithDistance> GetMostClosestTollPoint(GeoLocation center, IList<TollPoint> points, double radius = double.MaxValue);
	}
}

