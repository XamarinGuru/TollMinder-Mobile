using System;
using Newtonsoft.Json;

namespace Tollminder.Core.Models
{
    public class Profile
    {
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "emailValidate")]
        public bool EmailValidate { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }
        [JsonProperty(PropertyName = "phoneCode")]
        public string PhoneCode { get; set; }
        [JsonProperty(PropertyName = "phoneValidate")]
        public bool PhoneValidate { get; set; }
        [JsonProperty(PropertyName = "photo")]
        public string Photo { get; set; }
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
        // Google Plus, Facebook or by phone number
        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }
        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }
        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
        [JsonProperty(PropertyName = "zipCode")]
        public string ZipCode { get; set; }
        [JsonProperty(PropertyName = "driverLicense")]
        public DriverLicense DriverLicense { get; set; }
        // Here could be your credit cards
    }
}
