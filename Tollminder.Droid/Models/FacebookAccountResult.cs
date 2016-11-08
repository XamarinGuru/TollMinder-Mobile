using System;
namespace Tollminder.Droid.Models
{
    public class FacebookAccountResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string PhotoUrl
        {
            get
            {
                return $"https://graph.facebook.com/{Id}/picture?type=large";
            }
        }
    }
}
