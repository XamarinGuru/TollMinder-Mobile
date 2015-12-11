using System;
using Android.OS;

namespace Tollminder.Droid.Helpers
{
	public static class DroidMessanging
	{
		public static void SendMessage(int command, Messenger sender, Messenger replyTo)
		{
			Message msg = Message.Obtain (null, command);
			msg.ReplyTo = replyTo;
			sender.Send(msg);
		}

		public static void SendMessage(int command, Messenger sender, Messenger replyTo, Bundle data)
		{
			Message msg = Message.Obtain (null, command);
			msg.Data = data;
			msg.ReplyTo = replyTo;
			sender.Send(msg);
		}
	}
}

