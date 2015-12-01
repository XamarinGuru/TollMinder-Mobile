using System;
using MessengerHub;

namespace Tollminder.Core.Models
{
	public class MotionTypeChangedMessage : GenericMessageBase<MotionType>
	{
		public MotionTypeChangedMessage (object sender, MotionType motionType)
			: base (sender, motionType)
		{			
		}	
	}
}

