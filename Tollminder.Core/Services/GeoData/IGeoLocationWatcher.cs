using System;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services.GeoData
{
    public interface IGeoLocationWatcher
    {
        GeoLocation Location { get; set; }
        void StopGeolocationWatcher();
        void StartGeolocationWatcher();
        void StartUpdatingHighAccuracyLocation();
        void StopUpdatingHighAccuracyLocation();
        bool IsBound { get; }
    }
}