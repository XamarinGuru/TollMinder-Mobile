using System;

namespace PeggyPiston
{
	public static class PeggyLocationCalculations {

		private const string logChannel = "PeggyLocationCalculations";

		private static int lastDistCount = 0;
		private static double lastDistTotal = 0;
		private static long lastDistTime = 0;
		private static long lastDistTimeTotal = 0;
		private static double lastLat = 0;
		private static double lastLong = 0;


		public static string determineActivity (double newLat, double newLong ) {

			// init
			if (lastLat <= 0) {
				lastLat = newLat;
				lastLong = newLong;
			}
			lastDistCount++;

			// gather data
			double latDist = Math.Abs(lastLat - newLat);
			double longDist = Math.Abs(lastLong - newLong);
			lastDistTotal += Math.Sqrt(latDist*latDist + longDist*longDist);

			lastDistTimeTotal += PeggyUtils.getTime() - lastDistTime;
			lastDistTime = PeggyUtils.getTime();


			double moveSpeed = 0;

			// we've got enough segments to make some guesses
			if (lastDistCount > PeggyConstants.activityDetermineSegmentCount) {

				// calculate average meters per second to determine activity.
				if (lastDistTimeTotal == 0) lastDistTimeTotal = PeggyConstants.defaultTimeDenominator;
				moveSpeed = lastDistTotal / lastDistTimeTotal;

				// reset the values
				lastDistCount = 0;
				lastDistTotal = 0;
				lastDistTimeTotal = 0;
				lastDistTime = PeggyUtils.getTime();
			}

			// handle the results.
			if (moveSpeed >= PeggyConstants.drivingMetersPerSec) {
				PeggyUtils.DebugLog ("We're driving.", logChannel);
				return PeggyConstants.inVehicle;

			} else {
				PeggyUtils.DebugLog ("We're hanging out or walking around.", logChannel);
				return PeggyConstants.onFoot;
			}

		}

	
	}
}

