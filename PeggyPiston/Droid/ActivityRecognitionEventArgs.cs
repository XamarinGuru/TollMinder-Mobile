using System;

namespace PeggyPiston.Droid
{
	public class ActivityRecognitionEventArgs : EventArgs
	{

		public bool isDriving = false;

		public ActivityRecognitionEventArgs (bool id) {
			isDriving = id;
		}
	}
}
