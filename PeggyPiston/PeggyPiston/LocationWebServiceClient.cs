using System;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;

namespace PeggyPiston
{
	public static class LocationWebServiceClient
	{

		static readonly string logChannel = "LocationWebServiceClient";
		static HttpClient httpClient;


		/*
		public static async Task<int> ValidateUser(){

			_httpClient = httpClient;


			var client = RestClient (URL);
			var request = new RestRequest ("AAA ={0}&BBB={1}&CCC={2}", "111", "222","333");

			client.ExecuteAsync (request, response => {

				WebApiResponse webApiResponse = new WebApiResponse ();

				webApiResponse.Content = response.Content;
				webApiResponse.StatusCode = response.StatusCode;
				webApiResponse.ResponseStatus = (WebApiResponseStatus)response.ResponseStatus;

				return webApiResponse.Content;
			});         


			return -1;

		}
		*/




		public static async Task<int> FetchCurrentAddress(double lattitude, double longitude)
		{

			PeggyUtils.DebugLog ("Hitting the google book api", logChannel);

			httpClient = new HttpClient(); // Xamarin supports HttpClient!

			Task<string> contentsTask = httpClient.GetStringAsync("https://www.googleapis.com/books/v1/volumes?key=AIzaSyA0zkWIqwDeRibh-Ue9ko76kKL5T3ORwUI&maxResults=1&q=brandon%20sanderson%20legion"); // async method!

			// await! control returns to the caller and the task continues to run on another thread
			string contents = await contentsTask;

			//ResultEditText.Text += "DownloadHomepage method continues after async call. . . . .\n";


			// After contentTask completes, you can calculate the length of the string.
			int exampleInt = contents.Length;

			//ResultEditText.Text += "Downloaded the html and found out the length.\n\n\n";

			//ResultEditText.Text += contents; // just dump the entire HTML
			PeggyUtils.DebugLog (contents, logChannel);

			return exampleInt; // Task<TResult> returns an object of type TResult, in this case int
		}

	}
}

