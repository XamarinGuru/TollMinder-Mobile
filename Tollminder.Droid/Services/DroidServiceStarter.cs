using Android.Content;
using System;

namespace Tollminder.Droid.Services
{
	public class DroidServiceStarter : DroidGeolocationReciever
	{
		protected virtual Intent ServiceIntent { get; set; }

		public virtual void Start ()
		{
			ApplicationContext.StartService (ServiceIntent);
		}

		public virtual void Stop ()
		{
			ApplicationContext.StopService (ServiceIntent);
		}
	}
}