using System;
namespace Tollminder.Core.Utils.Slack
{
    public class SlackManager
    {
        public static void SendMessage(string notificationText)
        {
            string urlWithAccessToken = "https://tollminder.slack.com/services/hooks/incoming-webhook?token=https://hooks.slack.com/services/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";

            SlackClient client = new SlackClient(urlWithAccessToken);

            client.InitializeMessage(username: " @rsarafanov",
                       text: notificationText,
                       channel: "#general");
        }
    }
}