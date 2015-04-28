namespace PeggyPiston
{
	public static class PeggyConstants
	{

		public static readonly string channelDebug = "Debug";
		public static readonly string channelScreen = "DebugScreen";
		public static readonly string channelVoice = "DebugVoice";
		public static readonly string channelLocationUnavailable = "LocationUnavailable";
		public static readonly string channelLocationService = "LocationService";
		public static readonly string channelLocationAccuracyReady = "LocationAccuracyReady";

		public static readonly int activityDetermineSegmentCount = 3;

		// these are in miliseconds
		public static readonly long startupInterval = 1000;
		public static readonly long highAccuracyInterval = 1000 * 60 * 1;
		public static readonly long energySaverInterval = 1000 * 60 * 10;
		public static readonly long defaultTimeDenominator = 1000 * 60 * 10;

		// these are in meters.
		public static readonly float significantAccuracyRequirement = 20;
		public static readonly float highAccuracyRequirement = 20;
		public static readonly float distanceRequirement = 20;
		public static readonly float drivingMetersPerSec = 5;

		// activity types
		public static readonly string inVehicle = "inVehicle";
		public static readonly string onFoot = "onFoot";

	}
}

