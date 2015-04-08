namespace PeggyPiston
{
	public static class PeggyConstants
	{

		public static readonly string channelDebug = "Debug";
		public static readonly string channelVoice = "DebugVoice";
		public static readonly string channelLocationUnavailable = "LocationUnavailable";
		public static readonly string channelLocationService = "LocationService";
		public static readonly string channelLocationAccuracyReady = "LocationAccuracyReady";


		// these are in meters.
		public static readonly float significantAccuracyRequirement = 20;
		public static readonly float highAccuracyRequirement = 20;
		public static readonly float distanceRequirement = 20;


		// activity types
		public static readonly string inVehicle = "inVehicle";
		public static readonly string onFoot = "onFoot";

	}
}

