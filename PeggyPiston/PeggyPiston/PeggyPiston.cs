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
			DependencyService.Get<ITextToSpeech> ().Speak ("Peggy Piston has gone to sleep.");
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
			DependencyService.Get<ITextToSpeech> ().Speak ("Peggy Piston has woken up!");
		}
	}
}
