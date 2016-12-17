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
using System.Diagnostics;

namespace Tollminder.Core.ViewModels
{
    public class HomeTestViewModel : BaseViewModel
    {		
		readonly IMvxMessenger _messenger;
		readonly ITrackFacade _track;
        readonly IStoredSettingsService _storedSettingsService;
		readonly IGeoLocationWatcher _geoWatcher;

		IList<MvxSubscriptionToken> _tokens;
        public HomeTestViewModel(IMvxMessenger messenger, ITrackFacade track, IGeoLocationWatcher geoWatcher, IStoredSettingsService storedSettingsService)
        {
            _track = track;
            _messenger = messenger;
            _geoWatcher = geoWatcher;
            _storedSettingsService = storedSettingsService;

            _tokens = new List<MvxSubscriptionToken>();
        }

        Task RefreshToolRoads()
        {
            return ServerCommandWrapper(() => Mvx.Resolve<IGeoDataService>().RefreshTollRoads(CancellationToken.None));
        }

		public override void Start ()
		{
			base.Start ();

            Task.Run(RefreshToolRoads);
            try
            {
                _tokens.Add(_messenger.SubscribeOnThreadPoolThread<LocationMessage>(x => Location = x.Data, MvxReference.Strong));
                _tokens.Add(_messenger.SubscribeOnThreadPoolThread<StatusMessage>(x => StatusString = x.Data.ToString(), MvxReference.Strong));
                _tokens.Add(_messenger.SubscribeOnThreadPoolThread<MotionMessage>(x => MotionType = x.Data, MvxReference.Strong));

                _tokens.Add(_messenger.SubscribeOnMainThread<DistanceToNearestTollpoint>((s) => Distance = s.Data, MvxReference.Strong));
                _tokens.Add(_messenger.SubscribeOnMainThread<GeoWatcherStatusMessage>((s) => IsBound = s.Data, MvxReference.Strong));

                _tokens.Add(_messenger.SubscribeOnMainThread<CurrentTollpointChangedMessage>((s) => NearestTollpointsString = string.Join("\n", s.Data?.Select(x => x.Name)), MvxReference.Strong));
                _tokens.Add(_messenger.SubscribeOnMainThread<TollRoadChangedMessage>((s) => TollRoadString = s.Data?.Name, MvxReference.Strong));

                IsBound = _geoWatcher.IsBound;
                if (_geoWatcher.Location != null)
                    Location = _geoWatcher.Location;

                //LogText = Log._messageLog.ToString();

                StatusString = _track.TollStatus.ToString();
                TollRoadString = Mvx.Resolve<IWaypointChecker>().TollRoad?.Name;
                Distance = Mvx.Resolve<IWaypointChecker>().DistanceToNearestTollpoint;
                if (Mvx.Resolve<IWaypointChecker>().TollPointsInRadius != null)
                NearestTollpointsString = string.Join("\n", Mvx.Resolve<IWaypointChecker>().TollPointsInRadius?.Select(x => x.Name));
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Message: {0} StackTrace: {1}", ex.Message, ex.StackTrace);
            }
		}

		bool _isBound;
		public bool IsBound
		{
			get { return _isBound; }
			set
            {
                SetProperty(ref _isBound, value);
                RaisePropertyChanged(() => TrackingText);
            }
		}

        private double _distance;
        public double Distance
        {
            get { return _distance; }
            set
            {
                _distance = value;
                RaisePropertyChanged(() => Distance);
            }
        }

        public string TrackingText
        {
            get { return IsBound ? "TRACKING IS ON" : "TRACKING IS OFF"; }
        }

        MvxCommand _trackingCommand;
        public ICommand TrackingCommand
        {
            get
            {
                return _trackingCommand ?? (_trackingCommand = new MvxCommand(async () =>
                {
                    var result = false;
                    if (IsBound)
                        result = _track.StopServices();
                    else
                        result = await _track.StartServices();

                    if (result)
                        IsBound = _geoWatcher.IsBound;
                }));
            }
        }

        //Now works as logout
        MvxCommand _profileCommand;
        public ICommand ProfileCommand
        {
            get
            {
                return _profileCommand ?? (_profileCommand = new MvxCommand(() =>
                {
                    _track.StopServices();
                    _storedSettingsService.IsAuthorized = false;
                    ShowViewModel<LoginViewModel>();
                }));
            }
        }

        MvxCommand _payCommand;
        public ICommand PayCommand
        {
            get
            {
                return _payCommand ?? (_payCommand = new MvxCommand(() =>
                {
                    return;
                }));
            }
        }

        MvxCommand _payHistoryCommand;
        public ICommand PayHistoryCommand
        {
            get
            {
                return _payHistoryCommand ?? (_payHistoryCommand = new MvxCommand(() =>
                {
                    return;
                }));
            }
        }

		private GeoLocation _location;
		public GeoLocation Location {
			get { return _location; } 
			set {
				_location = value;
				RaisePropertyChanged (() => Location);
				RaisePropertyChanged (() => LocationString);
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

        private string _nearestTollpointsString;
        public string NearestTollpointsString
        {
            get { return _nearestTollpointsString; }
            set
            {
                _nearestTollpointsString = value;
                RaisePropertyChanged(() => NearestTollpointsString);
            }
        }

		public string LocationString {
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
		public MotionType MotionType {
			get { return _motionType; } 
			set {
				_motionType = value;
				RaisePropertyChanged (() => MotionType);
				RaisePropertyChanged (() => MotionTypeString);
			}
		}

		public string MotionTypeString {
			get { return _motionType.ToString(); }
		}


	}
}