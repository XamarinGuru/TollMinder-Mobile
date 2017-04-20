using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Xamarin.Forms;
using System.Reflection;
using Tollminder.Core.Services;

namespace Tollminder.Core.Models
{
    public class StatesData
    {
        [JsonProperty("name")]
        public string Name { get; set;}
        [JsonProperty("abbreviation")]
        public string Abbreviation { get; set;}
        private List<StatesData> states;

        public override string ToString()
        {
            return string.Format("{0} {1}", Name, Abbreviation);
        }
    }
}
