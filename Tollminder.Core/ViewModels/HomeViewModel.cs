using System;
using System.Collections.Generic;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Models;
using Tollminder.Core.ServicesHelpers;
using System.Threading;
using System.Threading.Tasks;
using Tollminder.Core.Helpers;
using Tollminder.Core.ViewModels.UserProfile;
using Tollminder.Core.ViewModels.Payments;
using Tollminder.Core.Services.Settings;
using Tollminder.Core.Services.Api;
using Tollminder.Core.Services.GeoData;
using Tollminder.Core.Services.RoadsProcessing;
using MvvmCross.Platform;
using Chance.MvvmCross.Plugins.UserInteraction;

namespace Tollminder.Core.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        readonly IMvxMessenger _messenger;
        readonly ITrackFacade _track;
        readonly IStoredSettingsService _storedSettingsService;
        readonly ISynchronisationService synchronisationService;
        readonly IGeoLocationWatcher _geoWatcher;
        public IWaypointChecker WaypointChecker { get; private set; }
        readonly IGeoDataService geoDataService;

        IList<MvxSubscriptionToken> _tokens;

        // for mock location
        public List<WaypointAction> WaypointActions { get; set; }
        public WaypointAction SelectedWaypointAction { get; set; }
        public MvxCommand NextGeoLocationCommand { get; set; }
        public MvxCommand PlayPauseIterationCommand { get; set; }

        public HomeViewModel(IMvxMessenger messenger, ITrackFacade track,
                             IGeoLocationWatcher geoWatcher, IStoredSettingsService storedSettingsService,
                             ISynchronisationService synchronisationService, IWaypointChecker waypointChecker,
                             IGeoDataService geoDataService)
        {
            _messenger = messenger;
            _track = track;
            _geoWatcher = geoWatcher;
            _storedSettingsService = storedSettingsService;
            this.WaypointChecker = waypointChecker;
            this.synchronisationService = synchronisationService;
            this.geoDataService = geoDataService;

            IsBound = _geoWatcher.IsBound;

            logoutCommand = new MvxCommand(() =>
            {
                _track.StopServices();
                _storedSettingsService.IsAuthorized = false;
                Close(this);
                ShowViewModel<LoginViewModel>();
            });
            _profileCommand = new MvxCommand(() => { ShowViewModel<ProfileViewModel>(); });
            _payCommand = new MvxCommand(() => { ShowViewModel<PayViewModel>(); });
            _payHistoryCommand = new MvxCommand(() => { ShowViewModel<PayHistoryViewModel>(); });
            _trackingCommand = new MvxCommand(async () =>
            {
                Log.LogMessage("Tracking button is pressed!");
                IsBound = !IsBound;

                var result = IsBound ? await _track.StartServicesAsync() : _track.StopServices();
                IsBound = _geoWatcher.IsBound;
            });

            _tokens = new List<MvxSubscriptionToken>();

            NextGeoLocationCommand = new MvxCommand(() => NextLocation());
            PlayPauseIterationCommand = new MvxCommand(() => PlayPauseLocationIteration());
            WaypointActions = new List<WaypointAction>(){
                WaypointAction.Entrance,
                WaypointAction.Bridge,
                WaypointAction.Exit
            };
            SelectedWaypointAction = WaypointAction.Entrance;
        }

        public async void Init(string name, string message)
        {
            if (name != null)
                await Mvx.Resolve<IUserInteraction>().AlertAsync("", message + name);
        }

        public async override void Start()
        {
            if (await synchronisationService.AuthorizeTokenSynchronisationAsync())
                await Task.Run(RefreshToolRoadsAsync);
            else
            {
                Close(this);
                if (IsBound)
                    _track.StopServices();
                ShowViewModel<LoginViewModel>();
                return;
            }

            base.Start();
            _tokens.Add(_messenger.SubscribeOnMainThread<GeoWatcherStatusMessage>((s) => IsBound = s.Data, MvxReference.Strong));
            _tokens.Add(_messenger.SubscribeOnThreadPoolThread<LocationMessage>(x => Location = x.Data, MvxReference.Strong));
            _tokens.Add(_messenger.SubscribeOnThreadPoolThread<StatusMessage>(x => StatusString = x.Data.ToString(), MvxReference.Strong));
            _tokens.Add(_messenger.SubscribeOnMainThread<TollRoadChangedMessage>((s) => TollRoadString = s.Data?.Name, MvxReference.Strong));

            await synchronisationService.DataSynchronisationAsync();

            StatusString = _track.TollStatus.ToString();
            TollRoadString = WaypointChecker.TollRoad?.Name;

            if (_geoWatcher.Location != null)
            {
                Location = _geoWatcher.Location;
                WaypointChecker.DetectWeAreInsideSomeTollPoint(Location);
            }
        }

        async void PlayPauseLocationIteration()
        {
            if (IsBound)
                Mvx.Resolve<IMockGeoLocation>().PlayPauseIteration();
            else
                await Mvx.Resolve<IUserInteraction>().AlertAsync("You need start tracking first.", "Warning");
        }

        async void NextLocation()
        {
            SettingsService.waypointAction = SelectedWaypointAction;
            if (IsBound)
                Mvx.Resolve<IMockGeoLocation>().NextTollPoint();
            else
                await Mvx.Resolve<IUserInteraction>().AlertAsync("You need start tracking first.", "Warning");
        }

        Task RefreshToolRoadsAsync()
        {
            return ServerCommandWrapperAsync(() => geoDataService.RefreshTollRoadsAsync(CancellationToken.None));
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