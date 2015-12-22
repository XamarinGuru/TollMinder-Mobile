using System;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
	public interface IMotionActivity
	{
		bool AuthInProgress { get; set; }
		MotionType MotionType { get; }
		void StartDetection ();
		void StopDetection ();
	}
}

