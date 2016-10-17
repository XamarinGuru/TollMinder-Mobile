using System;
using MvvmCross.Plugins.Messenger;

namespace Tollminder.Core.Models
{
	public class GenericMessage<T> : MvxMessage 
	{
		public T Data { get; private set; }

		public GenericMessage (object sender, T obj)
			: base (sender)
		{		
			Data = obj;	
		}
	}

	public class AppInBackgroundMessage : MvxMessage
	{
		public AppInBackgroundMessage (object sender)
			: base (sender)
		{
		}
	}

	public class LocationMessage : GenericMessage<GeoLocation>
	{
		public LocationMessage (object sender, GeoLocation geo)
			: base (sender, geo)
		{				
		}
	}

	public class StatusMessage : GenericMessage<TollGeolocationStatus>
	{
		public StatusMessage(object sender, TollGeolocationStatus status)
			: base(sender, status)
		{
		}
	}

	public class SpechRecognitionActivityLoadedMessage : MvxMessage
	{
		public SpechRecognitionActivityLoadedMessage(object sender)
			: base(sender)
		{
		}
	}

	public class GeoWatcherStatusMessage : GenericMessage<bool>
	{
		public GeoWatcherStatusMessage(object sender, bool status)
			: base(sender, status)
		{
		}
	}

	public class MotionMessage : GenericMessage<MotionType>
	{
		public MotionMessage (object sender, MotionType motion)
			: base (sender, motion)
		{				
		}
	}

    public class TollRoadChangedMessage : GenericMessage<TollRoad>
    {
        public TollRoadChangedMessage(object sender, TollRoad road)
            : base(sender, road)
        {
        }
    }

    public class CurrentWaypointChangedMessage : GenericMessage<TollPoint>
    {
        public CurrentWaypointChangedMessage(object sender, TollPoint point)
            : base(sender, point)
        {
        }
    }

	public class LogUpdated : MvxMessage
	{
		public LogUpdated (object sender)
			: base (sender)
		{				
		}
	}

	public class GeoFenceEnterceMessage : MvxMessage
	{
		public GeoFenceEnterceMessage (object sender)
			: base (sender)
		{				
		}
	}
}