using System;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;

namespace PeggyPiston
{
	public static class LocationWebServiceClient
	{

		static readonly string logChannel = "LocationWebServiceClient";
		static readonly HttpClient httpClient = new HttpClient();


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

			// google's demo: https://maps.googleapis.com/maps/api/place/search/json?location=-33.88471,151.218237&radius=100&sensor=true&key=AIzaSyBXEUF4f94N8y9gnhLQ8PmuznhySW080pU
			// looking for car services: https://maps.googleapis.com/maps/api/place/nearbysearch/json?key=AIzaSyBXEUF4f94N8y9gnhLQ8PmuznhySW080pU&rankby=distance&location=33.138953,-117.174768&types=convenience_store%7Ccar_repair%7Ccar_dealer%7Cgas_station
			// looking for any establishment: https://maps.googleapis.com/maps/api/place/nearbysearch/json?key=AIzaSyBXEUF4f94N8y9gnhLQ8PmuznhySW080pU&rankby=distance&location=33.138953,-117.174768&types=establishment

			Task<string> contentsTask = httpClient.GetStringAsync("https://maps.googleapis.com/maps/api/place/nearbysearch/json?key=AIzaSyBXEUF4f94N8y9gnhLQ8PmuznhySW080pU&location=" + lattitude.ToString() + "," + longitude.ToString() + "&rankby=distance&types=establishment"); // async method!
			string contents = await contentsTask;
			int exampleInt = contents.Length;

			PeggyUtils.DebugLog (contents, logChannel);

			return exampleInt; // Task<TResult> returns an object of type TResult, in this case int
		}

	}
}

