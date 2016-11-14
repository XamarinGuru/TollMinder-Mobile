using System;
using Android.App;
using Android.Content;
using Android.OS;

namespace Tollminder.Droid.Inerfaces
{
	public interface IDroidSocialLogin
	{
        void OnActivityResult(int requestCode, Result resultCode, Intent data);
	}
}

