using System;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;

namespace Tollminder.Core.Utils.Slack
{
    public class SlackClient
    {
        private readonly Uri _uri;

        public SlackClient(string urlWithAccessToken)
        {
            _uri = new Uri(urlWithAccessToken);
        }

        //Post a message using simple strings  
        public void InitializeMessage(string text, string username = null, string channel = null)
        {
            Payload payload = new Payload()
            {
                Channel = channel,
                Username = username,
                Text = text
            };

            PostMessage(payload);
        }

        //Post a message using a Payload object  
        public void PostMessage(Payload payload)
        {
            string payloadJson = JsonConvert.SerializeObject(payload);

            using (HttpClient client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", payload.Username),
                    new KeyValuePair<string, string>("text", payload.Text),
                    new KeyValuePair<string, string>("channel", payload.Channel)

                });
                var response = client.PostAsync(_uri, content).Result;
                string responseText = response.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
