using System;
using System.Threading.Tasks;

using System.Net;

namespace PeggyPiston
{
	public static class LocationWebServiceClient
	{

		public static async Task<int> FetchCurrentAddress(double lattitude, double longitude)
		{
			httpClient = new HttpClient(); // Xamarin supports HttpClient!

			Task<string> contentsTask = httpClient.GetStringAsync("http://xamarin.com"); // async method!

			// await! control returns to the caller and the task continues to run on another thread
			string contents = await contentsTask;

			ResultEditText.Text += "DownloadHomepage method continues after async call. . . . .\n";

			// After contentTask completes, you can calculate the length of the string.
			int exampleInt = contents.Length;

			ResultEditText.Text += "Downloaded the html and found out the length.\n\n\n";

			ResultEditText.Text += contents; // just dump the entire HTML

			return exampleInt; // Task<TResult> returns an object of type TResult, in this case int
		}

	}
}

