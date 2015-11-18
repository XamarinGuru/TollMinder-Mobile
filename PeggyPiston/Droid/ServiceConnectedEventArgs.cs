using System;
using Android.OS;

namespace PeggyPiston.Droid
{
	public class ServiceConnectedEventArgs : EventArgs
	{
		public IBinder Binder { get; set; }
	}
}