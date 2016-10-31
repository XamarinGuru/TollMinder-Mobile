using System;
using Tollminder.Core.Models;

namespace Tollminder.Core.Extensions
{
	public static class Extensions
	{
		public static bool CheckIsMovingByTheCar (this MotionType motionType)
		{				
			return MotionType.Automotive == motionType;
		}
	}
}

