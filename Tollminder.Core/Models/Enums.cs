using System;
using System.Runtime.Serialization;

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

    public enum AuthorizationType
    {
        [EnumMember(Value = "email")]
        Email,
        [EnumMember(Value = "facebook")]
        Facebook,
        [EnumMember(Value = "gplus")]
        GPlus
    }
}
