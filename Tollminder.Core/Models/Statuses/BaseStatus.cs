using System;
using System.Threading.Tasks;
using MvvmCross.Platform;
using Tollminder.Core.Services.GeoData;
using Tollminder.Core.Services.Notifications;
using Tollminder.Core.Services.RoadsProcessing;
using Tollminder.Core.Services.Settings;
using Tollminder.Core.Services.SpeechRecognition;

namespace Tollminder.Core.Models.Statuses
{
    public abstract class BaseStatus
    {
        IGeoLocationWatcher _geoWatcher;
        protected IGeoLocationWatcher GeoWatcher
        {
            get
            {
                return _geoWatcher ?? (_geoWatcher = Mvx.Resolve<IGeoLocationWatcher>());
            }
        }

        IMotionActivity _motionActivity;
        protected IMotionActivity MotionActivity
        {
            get
            {
                return _motionActivity ?? (_motionActivity = Mvx.Resolve<IMotionActivity>());
            }
        }

        IDistanceChecker _distanceChecker;
        protected IDistanceChecker DistanceChecker
        {
            get
            {
                return _distanceChecker ?? (_distanceChecker = Mvx.Resolve<IDistanceChecker>());
            }
        }

        IWaypointChecker _waypointChecker;
        protected IWaypointChecker WaypointChecker
        {
            get
            {
                if (_waypointChecker == null)
                {
                    _waypointChecker = Mvx.Resolve<IWaypointChecker>();
                }
                return _waypointChecker;
            }
        }

        IGeoDataService _dataService;
        protected IGeoDataService GeoDataService
        {
            get
            {
                return _dataService ?? (_dataService = Mvx.Resolve<IGeoDataService>());
            }
        }

        INotifyService _notifyService;
        protected INotifyService NotifyService
        {
            get
            {
                return _notifyService ?? (_notifyService = Mvx.Resolve<INotifyService>());
            }
        }

        ISpeechToTextService _speechToTextService;
        public ISpeechToTextService SpeechToTextService
        {
            get
            {
                return _speechToTextService ?? (_speechToTextService = Mvx.Resolve<ISpeechToTextService>());
            }
        }

        IBatteryDrainService _batteryDrainService;
        protected IBatteryDrainService BatteryDrainService
        {
            get
            {
                return _batteryDrainService ?? (_batteryDrainService = Mvx.Resolve<IBatteryDrainService>());
            }
        }

        public abstract Task<TollGeolocationStatus> CheckStatus();
        public abstract bool CheckBatteryDrain();
    }
}

