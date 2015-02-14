using System;

using Xamarin.Forms;

namespace PeggyPiston
{
	public class MainScreen : ContentPage
	{

		private IGeoLocation _locationProvider;
		private string _currentLocation;

		public MainScreen ()
		{

			_locationProvider = DependencyService.Get<IGeoLocation>();
			_currentLocation = "";

			MessagingCenter.Subscribe<IGeoLocation,string>(this, "TestingLocation", HandleLocationUpdate);


			Content = new TableView {
				Intent = TableIntent.Form,
				Root = new TableRoot ("Table Title") {
					new TableSection ("Section 1 Title") {
						new TextCell {
							Text = "TextCell Text",
							Detail = "TextCell Detail"
						},
						new EntryCell {
							Label = "EntryCell:",
							Placeholder = "default keyboard",
							Keyboard = Keyboard.Default
						}
					},
					new TableSection ("Section 2 Title") {
						new EntryCell {
							Label = "Another EntryCell:",
							Placeholder = "phone keyboard",
							Keyboard = Keyboard.Telephone
						},
						new SwitchCell {
							Text = "SwitchCell:"
						}
					}
				}
			};

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


