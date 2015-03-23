using System;

namespace PeggyPiston
{
	public interface IGeoLocation
	{
		double GetCurrentLongitude();
		double GetCurrentLattitude();
		double GetCurrentSpeed();
		double GetCurrentBearing();
		double GetCurrentAccuracy();
	}
}

