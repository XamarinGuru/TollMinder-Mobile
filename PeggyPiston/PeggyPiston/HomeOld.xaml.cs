﻿using System;
using Xamarin.Forms;

namespace PeggyPiston
{
	public partial class HomeOld : ContentPage
	{
		private IGeoLocation _locationProvider;
		private string _currentLocation;

		public HomeOld ()
		{
			// figure out why this is red someday...
			InitializeComponent ();

			_locationProvider = DependencyService.Get<IGeoLocation>();
			_currentLocation = "";

			MessagingCenter.Subscribe<IGeoLocation,string>(this, "TestingLocation", HandleLocationUpdate);

		}

		void OnButtonClicked(object sender, EventArgs args)
		{
			DependencyService.Get<ITextToSpeech>().Speak("You clicked a button!");

		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			//_locationProvider.Start();
		}

		public void HandleLocationUpdate(IGeoLocation service, string newLocation)
		{
			if (_currentLocation != newLocation) {
				DependencyService.Get<ITextToSpeech> ().Speak ("your current address is " + newLocation);
				_currentLocation = newLocation;
			}
		}

	}
}

