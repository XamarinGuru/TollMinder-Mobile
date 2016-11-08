using System;
namespace Tollminder.Core.Models
{
    public class PersonData
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Photo { get; set; }
        public string Token { get; set; }
        public AuthorizationType Source { get; set; }
    }
}
