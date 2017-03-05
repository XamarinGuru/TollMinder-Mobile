using System;
using System.Collections.Generic;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Tollminder.Core.ServicesHelpers;
using Tollminder.Core.Helpers;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tollminder.Core.ViewModels
{
    public class HomeDebugViewModel : BaseViewModel
    {
        readonly IMvxMessenger _messenger;
        readonly ITrackFacade _track;
        readonly IStoredSettingsService _storedSettingsService;
        readonly IGeoLocationWatcher _geoWatcher;
        readonly ISynchronisationService synchronisationService;

        IList<MvxSubscriptionToken> _tokens;
        public HomeDebugViewModel(IMvxMessenger messenger, ITrackFacade track, IGeoLocationWatcher geoWatcher, IStoredSettingsService storedSettingsService, ISynchronisationService synchronisationService)
        {
            _track = track;
            _messenger = messenger;
            _geoWatcher = geoWatcher;
            IsBound = _geoWatcher.IsBound;
            this.synchronisationService = synchronisationService;
            _storedSettingsService = storedSettingsService;

            _tokens = new List<MvxSubscriptionToken>();
        }

        Task RefreshToolRoads()
        {
            return ServerCommandWrapper(() => Mvx.Resolve<IGeoDataService>().RefreshTollRoads(CancellationToken.None));
        }

        public async override void Start()
        {
            if (await synchronisationService.AuthorizeTokenSynchronisation())
                await Task.Run(RefreshToolRoads);
            else
            {
                Close(this);
                ShowViewModel<LoginViewModel>();
                return;
            }

            base.Start();

            _tokens.Add(_messenger.SubscribeOnThreadPoolThread<LocationMessage>(x => Location = x.Data, MvxReference.Strong));
            _tokens.Add(_messenger.SubscribeOnThreadPoolThread<StatusMessage>(x => StatusString = x.Data.ToString(), MvxReference.Strong));
            _tokens.Add(_messenger.SubscribeOnThreadPoolThread<MotionMessage>(x => MotionType = x.Data, MvxReference.Strong));

            _tokens.Add(_messenger.SubscribeOnMainThread<LogUpdated>((s) => LogText = Log._messageLog.ToString(), MvxReference.Strong));
            _tokens.Add(_messenger.SubscribeOnMainThread<GeoWatcherStatusMessage>((s) => IsBound = s.Data, MvxReference.Strong));

            _tokens.Add(_messenger.SubscribeOnMainThread<CurrentTollpointChangedMessage>((s) => CurrentWaypointString = string.Join("\n", s.Data?.Select(x => x.Name)), MvxReference.Strong));
            _tokens.Add(_messenger.SubscribeOnMainThread<TollRoadChangedMessage>((s) => TollRoadString = s.Data?.Name, MvxReference.Strong));

            if (_geoWatcher.Location != null)
                Location = _geoWatcher.Location;

            LogText = Log._messageLog.ToString();

            StatusString = _track.TollStatus.ToString();
            TollRoadString = Mvx.Resolve<IWaypointChecker>().TollRoad?.Name;
            if (Mvx.Resolve<IWaypointChecker>().TollPointsInRadius != null)
                CurrentWaypointString = string.Join("\n", Mvx.Resolve<IWaypointChecker>().TollPointsInRadius?.Select(x => x.Name));
        }

        bool _isBound;
        public bool IsBound
        {
            get { return _isBound; }
            set
            {
                _isBound = value;
                RaisePropertyChanged(() => IsBound);
            }
        }

        private GeoLocation _location;
        public GeoLocation Location
        {
            get { return _location; }
            set
            {
                _location = value;
                RaisePropertyChanged(() => Location);
                RaisePropertyChanged(() => LocationString);
            }
        }

        private string _logText;
        public string LogText
        {
            get { return _logText; }
            set
            {
                _logText = value;
                RaisePropertyChanged(() => LogText);
            }
        }

        private string _tollRoadString;
        public string TollRoadString
        {
            get { return _tollRoadString; }
            set
            {
                _tollRoadString = value;
                RaisePropertyChanged(() => TollRoadString);
            }
        }

        private string _currentWaypointString;
        public string CurrentWaypointString
        {
            get { return _currentWaypointString; }
            set
            {
                _currentWaypointString = value;
                RaisePropertyChanged(() => CurrentWaypointString);
            }
        }

        public string LocationString
        {
            get { return _location.ToString(); }
        }

        string _statusString;
        public string StatusString
        {
            get { return _statusString; }
            set
            {
                _statusString = value;
                RaisePropertyChanged(() => StatusString);
            }
        }

        private MotionType _motionType;
        public MotionType MotionType
        {
            get { return _motionType; }
            set
            {
                _motionType = value;
                RaisePropertyChanged(() => MotionType);
                RaisePropertyChanged(() => MotionTypeString);
            }
        }

        public string MotionTypeString
        {
            get { return _motionType.ToString(); }
        }

        MvxCommand _startCommand;
        public ICommand StartCommand
        {
            get
            {
                return _startCommand ?? (_startCommand = new MvxCommand(async () =>
               {
                   if (await _track.StartServices())
                       IsBound = _geoWatcher.IsBound;
               }));
            }
        }

        MvxCommand _stopCommand;
        public ICommand StopCommand
        {
            get
            {
                return _stopCommand ?? (_stopCommand = new MvxCommand(() =>
               {
                   if (_track.StopServices())
                       IsBound = _geoWatcher.IsBound;
               }));
            }
        }

        MvxCommand _logOutCommand;
        public ICommand LogOutCommand
        {
            get
            {
                return _logOutCommand ?? (_logOutCommand = new MvxCommand(() =>
                {
                    _track.StopServices();
                    _storedSettingsService.IsAuthorized = false;
                    ShowViewModel<LoginViewModel>();
                }));
            }
        }
    }
}