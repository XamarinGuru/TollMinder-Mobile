using System;
using Android.OS;

namespace Tollminder.Droid.Inerfaces
{
	public interface IDroidMessenger
	{
		Messenger Messenger { get; set; } 
		Messenger MessengerService { get; set; }
	}
}

