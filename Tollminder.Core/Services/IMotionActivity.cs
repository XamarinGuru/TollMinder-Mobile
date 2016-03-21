using System;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
	public interface IMotionActivity
	{
		bool IsBound { get; }
		MotionType MotionType { get; }
		//bool IsAutomove { get; }
		void StartDetection ();
		void StopDetection ();
	}
}

