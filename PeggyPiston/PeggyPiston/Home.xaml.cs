using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PeggyPiston
{
	public partial class Home : ContentPage
	{
		private IGeoLocation _locationProvider;

		public Home ()
		{
			// figure out why this is red someday...
			InitializeComponent ();

			_locationProvider = DependencyService.Get<IGeoLocation>();

			MessagingCenter.Subscribe<IGeoLocation,string>(this, "TestingLocation", HandleLocationUpdate);

		}

		void OnButtonClicked(object sender, EventArgs args)
		{
			DependencyService.Get<ITextToSpeech>().Speak("You clicked a button!");

		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_locationProvider.Start();
		}

		private static void HandleLocationUpdate(IGeoLocation service, string newLocation)
		{
			DependencyService.Get<ITextToSpeech>().Speak("your current address is " + newLocation);
		}

	}
}

