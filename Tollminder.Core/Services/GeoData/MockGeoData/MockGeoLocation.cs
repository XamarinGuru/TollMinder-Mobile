using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using Tollminder.Core.Services.Settings;
using System.Threading.Tasks;
using System.Collections.Generic;
using MvvmCross.Binding.ExtensionMethods;
using Tollminder.Core.Services.RoadsProcessing;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace Tollminder.Core.Services.GeoData
{
    public class MockGeoLocation : IGeoLocationWatcher, IMotionActivity, IMockGeoLocation
    {
        readonly IStoredSettingsService _storedSettingsService;
        readonly IMvxMessenger _messenger;
        readonly IDataBaseService dataBaseService;
        private IList<TollRoad> getRoads;
        private int roadId = 6;
        private bool updateAgain = true;
        private bool isStartLocation;
        private WaypointAction waypointAction;
        private bool getDataFromDataBase;

        public MockGeoLocation(IStoredSettingsService storedSettingsService, IMvxMessenger messenger, IDataBaseService dataBaseService)
        {
            this._storedSettingsService = storedSettingsService;
            this._messenger = messenger;
            this.dataBaseService = dataBaseService;
            //getDataFromDataBase = true;
        }

        public bool IsBound
        {
            get
            {
                return _storedSettingsService.GeoWatcherIsRunning;
            }
            set
            {
                _storedSettingsService.GeoWatcherIsRunning = value;
                _messenger.Publish(new GeoWatcherStatusMessage(this, value));
            }
        }

        private GeoLocation location;
        public GeoLocation Location
        {
            get { return location; }
            set
            {
                Log.LogMessage($"Received location in geowatcher {value}");
                if (IsBound && (!location?.Equals(value) ?? true))
                {
                    location = value;
                    Mvx.Resolve<IMvxMessenger>().Publish(new LocationMessage(this, value));
                    Log.LogMessage($"New location {value}");
                }
            }
        }

        private MotionType _motionType;
        public MotionType MotionType
        {
            get { return _motionType; }
            set
            {
                _motionType = value;
                Mvx.Resolve<IMvxMessenger>().Publish(new MotionMessage(this, value));
            }
        }

        private async void StartLocationUpdates()
        {
            //await Task.Delay(20000);
            //IsBound = true;
            //Location = new GeoLocation()
            //{
            //    Latitude = 50.512752,
            //    Longitude = 30.497996,
            //    TollPointId = "dfghjkl"
            //};
            if (getDataFromDataBase)
                ProcessDataFromDataBase();
            else
                ProcessLocalData();
        }

        private async void ProcessDataFromDataBase()
        {
            getRoads = getRoads ?? await dataBaseService.GetTollRoadList();
            if (getRoads.Count != 0)
            {
                if (isStartLocation)
                {
                    var getTollRoad = (TollRoad)getRoads.ElementAt(roadId);
                    ProcessMockLocation(getTollRoad);
                }
            }
        }

        private void ProcessLocalData()
        {
            if (isStartLocation)
            {
                var getTollRoad = MockTollRoad("Tollminder.Core.Services.GeoData.MockGeoData.mockTheRoad.json");
                ProcessMockLocation(getTollRoad);
            }
        }

        private async void ProcessMockLocation(TollRoad getTollRoad)
        {
            await Task.Delay(20000);
            IsBound = true;

            foreach (var road in getTollRoad.WayPoints)
            {
                if (isStartLocation)
                {
                    foreach (var point in road.TollPoints)
                    {
                        if (isStartLocation)
                        {
                            //if (point.WaypointAction == SettingsService.waypointAction)
                            //{
                            Location = new GeoLocation()
                            {
                                Latitude = point.Latitude,
                                Longitude = point.Longitude,
                                TollPointId = point.Id
                            };
                            //updateAgain = false;
                            //return;
                            //}
#if DEBUG
                            await Task.Delay(point.Interval);
#elif REALEASE
                            await Task.Delay(20000);
#endif
                        }
                    }
                }
            }
        }

        private void StopLocationUpdates()
        {
            isStartLocation = false;
        }

        public virtual void StartGeolocationWatcher()
        {
            if (!IsBound)
            {
                isStartLocation = true;
                StartLocationUpdates();
                IsBound = true;
            }
        }

        public virtual void StopGeolocationWatcher()
        {
            if (IsBound)
            {
                isStartLocation = false;
                StopLocationUpdates();
                IsBound = false;
            }
        }

        public virtual void StartUpdatingHighAccuracyLocation()
        {
            UpdateAccuracyLocation(SettingsService.DistanceIntervalHighDefault);
        }

        public virtual void StopUpdatingHighAccuracyLocation()
        {
            UpdateAccuracyLocation(SettingsService.DistanceIntervalDefault);
        }

        void UpdateAccuracyLocation(int distanceInterval)
        {
            if (IsBound)
            {
                StopLocationUpdates();
                StartLocationUpdates();
            }
        }

        public void StartDetection()
        {
            if (!IsBound)
            {
                MotionType = MotionType.Automotive;
            }
        }

        public void StopDetection()
        {
            if (IsBound)
                IsBound = false;
        }

        public void NextTollPoint()
        {
            //this.waypointAction = SettingsService.waypointAction;
            isStartLocation = true;
            StartLocationUpdates();
        }

        public void PlayPauseIteration()
        {
            isStartLocation = !isStartLocation ? true : false;
        }

        public TollRoad MockTollRoad(string fileName)
        {
            List<TollPoint> tollPoints;
            var assembly = typeof(StatesData).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(fileName);
            using (var reader = new System.IO.StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                tollPoints = JsonConvert.DeserializeObject<List<TollPoint>>(json);
            }
            var tollRoad = new TollRoad("Minskiy Area - Glorium", tollPoints)
            {
                Id = "584a4b53f44d57dc336998ca"
            };
            return tollRoad;
        }
    }
}
