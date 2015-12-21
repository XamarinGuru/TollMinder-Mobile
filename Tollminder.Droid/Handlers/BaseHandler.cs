using System;
using Android.OS;

namespace Tollminder.Droid.Handlers
{
	public class BaseHandler : Handler
	{
		protected virtual object Service { get; set; }

		public BaseHandler (object service)
		{
			Service = service;
		}
	}
}

