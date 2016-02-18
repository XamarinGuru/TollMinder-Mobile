using System;
using Android.OS;

namespace Tollminder.Droid.Handlers
{
	public class BaseHandler : Handler
	{
		public virtual object Service { get; set; }

		public BaseHandler (object service)
		{
			Service = service;
		}
	}
}