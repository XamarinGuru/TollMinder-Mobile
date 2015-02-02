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

			MessagingCenter.Subscribe<IGeoLocation,string>(this, "init state string", HandleLocationUpdate);

		}

		void OnButtonClicked(object sender, EventArgs args)
		{
			DependencyService.Get<ITextToSpeech>().Speak("You clicked a button!");

/*
			if (_currentLocation == null)
			{
				_addressText.Text = "Can't determine the current address.";
				return;
			}

			Geocoder geocoder = new Geocoder(this);
			IList<Address> addressList = await geocoder.GetFromLocationAsync(_currentLocation.Latitude, _currentLocation.Longitude, 10);

			Address address = addressList.FirstOrDefault();
			if (address != null)
			{
				StringBuilder deviceAddress = new StringBuilder();
				for (int i = 0; i < address.MaxAddressLineIndex; i++)
				{
					deviceAddress.Append(address.GetAddressLine(i))
						.AppendLine(",");
				}
				_addressText.Text = deviceAddress.ToString();
			}
			else
			{
				_addressText.Text = "Unable to determine the address.";
			}
*/

		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_locationProvider.Start();
		}

		private static void HandleLocationUpdate(IGeoLocation service, string newLocation)
		{
			DependencyService.Get<ITextToSpeech>().Speak("new location at " + newLocation);
		}

	}
}

