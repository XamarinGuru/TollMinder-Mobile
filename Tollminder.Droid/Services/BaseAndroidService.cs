using Android.App;
using Android.Content;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;

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

