using System.Collections.Generic;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services.ProfileData
{
    public class LicenseDataService : ILoadResourceData<string>
    {
        public string LicensePlate { get; set; }
        public StatesData States { get; set; }
        public string VehicleClass { get; set; }
        private List<string> vehicleClasses;

        public LicenseDataService()
        {
            vehicleClasses = new List<string>();
        }

        public List<string> GetData(string fileName)
        {
            vehicleClasses.Add("1 - axle");
            vehicleClasses.Add("2 - axle");
            vehicleClasses.Add("3 - axle");
            vehicleClasses.Add("4 - axle");
            vehicleClasses.Add("5 - axle");
            vehicleClasses.Add("6 - axle");
            vehicleClasses.Add("7 - axle");
            vehicleClasses.Add("8 - axle");
            vehicleClasses.Add("9 - axle");
            vehicleClasses.Add("10 - axle");
            vehicleClasses.Add("11 - axle");
            vehicleClasses.Add("12 - axle");

            return vehicleClasses;
        }
    }
}
