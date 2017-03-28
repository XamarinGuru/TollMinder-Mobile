using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services.ProfileData
{
    public class StatesService : ILoadResourceData<StatesData>
    {
        private List<StatesData> states;

        public List<StatesData> GetData(string fileName)
        {
            var assembly = typeof(StatesData).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(fileName);
            using (var reader = new System.IO.StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                states = JsonConvert.DeserializeObject<List<StatesData>>(json);
            }
            return states;
        }
    }
}
