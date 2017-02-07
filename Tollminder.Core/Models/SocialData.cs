using System;
namespace Tollminder.Core.Models
{
    public class SocialData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Photo { get; set; }
        public string Token { get; set; }
        public AuthorizationType Source { get; set; }
        public string Id { get; set; }
    }
}
