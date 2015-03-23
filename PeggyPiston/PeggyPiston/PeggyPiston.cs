using System;

using Xamarin.Forms;

namespace PeggyPiston
{
	public class App : Application
	{

		protected readonly string logChannel = "PeggyPiston";

		public App ()
		{
			MainPage = new MainScreen();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
			PeggyUtils.DebugLog ("Peggy Piston has gone to sleep.", PeggyConstants.channelVoice);
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
			PeggyUtils.DebugLog ("Peggy Piston has woken up.", PeggyConstants.channelVoice);
		}
	}
}
