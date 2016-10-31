using System;

namespace Tollminder.Core.Models
{
	public enum UserMessageType
	{
		Attention,
		Error,
		Success,
		Info
	}

	public enum GeofenceStatus
	{
		OnRemoveGeofence,
		OnAddGeofencePoint,
		OnBuildGeoFenceRequest,
		OnAddGeofence,
		None
	}

	public enum AnswerType
	{
		Positive,
		Negative,
		Unknown
	}
}
