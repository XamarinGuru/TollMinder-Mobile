using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PeggyPiston
{
	public partial class Home : ContentPage
	{
		public Home ()
		{
			InitializeComponent ();
		}

		void OnButtonClicked(object sender, EventArgs args)
		{
			DependencyService.Get<ITextToSpeech>().Speak("You clicked a button!");
		}

	}
}

