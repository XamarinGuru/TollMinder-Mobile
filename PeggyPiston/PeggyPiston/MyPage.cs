using System;

using Xamarin.Forms;

namespace PeggyPiston
{
	public class MyPage : ContentPage
	{
		public MyPage ()
		{
			var speak = new Button {
				Text = "Say Something Pithy",
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};
			speak.Clicked += (sender, e) => {
				DependencyService.Get<ITextToSpeech>().Speak("What do you mean by Pithy?");
			};
			Content = speak;
		}
	}
}


