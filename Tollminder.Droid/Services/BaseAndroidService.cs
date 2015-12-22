using System;
using Android.Content;
using Cirrious.CrossCore;
using Android.OS;
using Tollminder.Droid.Handlers;
using Cirrious.CrossCore.Droid.Platform;
using Android.App;

namespace Tollminder.Droid.Services
{
	public class BaseAndroidService
	{
		public readonly Context ApplicationContext;
		public readonly Activity Activity;

		public BaseAndroidService ()
		{
			Activity =  Mvx.Resolve<IMvxAndroidCurrentTopActivity> ().Activity;
			ApplicationContext = Activity.ApplicationContext;
		}

		public virtual void Start()
		{
		}

		public virtual void Stop()
		{
		}
	}
}

