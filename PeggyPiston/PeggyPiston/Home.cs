using System;

using Xamarin.Forms;

namespace PeggyPiston
{
	public class Home : ContentPage
	{
		public Home ()
		{

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
	}
}


