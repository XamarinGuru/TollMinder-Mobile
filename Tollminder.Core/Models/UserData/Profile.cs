using System;
using System.Net;
using Newtonsoft.Json;
using SQLite;
using Tollminder.Core.Models.DriverData;

namespace Tollminder.Core.Models
{
    [Table("Profile")]
    public class Profile : IDatabaseEntry
    {
        [PrimaryKey]
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "firstname")]
        public string FirstName { get; set; }
        [JsonProperty(PropertyName = "lastname")]
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
        [JsonProperty(PropertyName = "facebookId")]
        public string FacebookId { get; set; }
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
        // For social network, to check is there user like this one
        public HttpStatusCode StatusCode { get; set; }
    }
}
