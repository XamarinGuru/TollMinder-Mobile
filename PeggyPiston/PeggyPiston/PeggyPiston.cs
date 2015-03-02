using System;

using Xamarin.Forms;

namespace PeggyPiston
{
	public class App : Application
	{
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
			MessagingCenter.Send<App, string> (this, "Debug", "Peggy Piston has gone to sleep.");
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
			MessagingCenter.Send<App, string> (this, "Debug", "Peggy Piston has woken up.");
		}
	}
}
