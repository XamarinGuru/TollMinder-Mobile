using System;
using PropertyChanged;
namespace Tollminder.Core.Models.DriverData
{
    [ImplementPropertyChanged]
    public class Vehicle
    {
        public string Id { get; set; }
        public string PlateNumber { get; set; }
        public string State { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Color { get; set; }
    }
}
