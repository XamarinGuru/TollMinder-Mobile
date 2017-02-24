using System;
using System.Collections.Generic;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Tollminder.Core.ServicesHelpers;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;

namespace Tollminder.Core.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        readonly IMvxMessenger _messenger;
        readonly ITrackFacade _track;
        readonly IStoredSettingsService _storedSettingsService;
        readonly ISynchronisationService synchronisationService;
        readonly IGeoLocationWatcher _geoWatcher;

        IList<MvxSubscriptionToken> _tokens;

        public HomeViewModel(IMvxMessenger messenger, ITrackFacade track, IGeoLocationWatcher geoWatcher, IStoredSettingsService storedSettingsService)
        {
            _messenger = messenger;
            _track = track;
            _geoWatcher = geoWatcher;
            _storedSettingsService = storedSettingsService;

            synchronisationService = Mvx.Resolve<ISynchronisationService>();

            logoutCommand = new MvxCommand(() =>
            {
                _track.StopServices();
                _storedSettingsService.IsAuthorized = false;
                Close(this);
                ShowViewModel<LoginViewModel>();
            });
            _profileCommand = new MvxCommand(() => { ShowViewModel<ProfileViewModel>(); });
            _payCommand = new MvxCommand(() => { });
            _payHistoryCommand = new MvxCommand(() => { ShowViewModel<PayHistoryViewModel>(); });
            _trackingCommand = new MvxCommand(async () =>
            {
                var result = IsBound ? _track.StopServices() : await _track.StartServices();
                if (result)
                    IsBound = _geoWatcher.IsBound;
            });

            _tokens = new List<MvxSubscriptionToken>();
        }

        public void Init(string name, string message)
        {
            if (name != null)
                Mvx.Resolve<IProgressDialogManager>().CloseAndShowMessage(message + name, "");
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
            _tokens.Add(_messenger.SubscribeOnMainThread<GeoWatcherStatusMessage>((s) => IsBound = s.Data, MvxReference.Strong));
            _tokens.Add(_messenger.SubscribeOnThreadPoolThread<LocationMessage>(x => Location = x.Data, MvxReference.Strong));
            _tokens.Add(_messenger.SubscribeOnThreadPoolThread<StatusMessage>(x => StatusString = x.Data.ToString(), MvxReference.Strong));
            _tokens.Add(_messenger.SubscribeOnMainThread<TollRoadChangedMessage>((s) => TollRoadString = s.Data?.Name, MvxReference.Strong));
            _tokens.Add(_messenger.SubscribeOnMainThread<DistanceToNearestTollpoint>((s) => DistanceToNearestTollpoint = s.Data, MvxReference.Strong));

            await synchronisationService.DataSynchronisation();

            IsBound = _geoWatcher.IsBound;
            StatusString = _track.TollStatus.ToString();
            TollRoadString = Mvx.Resolve<IWaypointChecker>().TollRoad?.Name;

            if (_geoWatcher.Location != null)
                Location = _geoWatcher.Location;
            try
            {
                if (Mvx.Resolve<IWaypointChecker>().TollPointsInRadius != null)
                    DistanceToNearestTollpoint = double.Parse(string.Join("\n", Mvx.Resolve<IWaypointChecker>().TollPointsInRadius.Select(x => x.Distance)));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        Task RefreshToolRoads()
        {
            return ServerCommandWrapper(() => Mvx.Resolve<IGeoDataService>().RefreshTollRoads(CancellationToken.None));
        }

        MvxCommand _trackingCommand;
        public ICommand TrackingCommand { get { return _trackingCommand; } }

        MvxCommand _profileCommand;
        public ICommand ProfileCommand { get { return _profileCommand; } }

        MvxCommand _payCommand;
        public ICommand PayCommand { get { return _payCommand; } }

        MvxCommand _payHistoryCommand;
        public ICommand PayHistoryCommand { get { return _payHistoryCommand; } }

        MvxCommand logoutCommand;
        public ICommand LogoutCommand { get { return logoutCommand; } }

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

        public string TrackingText
        {
            get { return IsBound ? "TRACKING IS ON" : "TRACKING IS OFF"; }
        }

        string _supportText = $"Call Center:{Environment.NewLine}+(1) 305 335 85 08";
        public string SupportText
        {
            get { return _supportText; }
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

        private double _distanceToNearestTollpoint;
        public double DistanceToNearestTollpoint
        {
            get { return _distanceToNearestTollpoint; }
            set
            {
                _distanceToNearestTollpoint = value;
                RaisePropertyChanged(() => DistanceToNearestTollpoint);
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
    }
}