using System;
using System.Collections.Generic;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Tollminder.Core.ServicesHelpers;
using Chance.MvvmCross.Plugins.UserInteraction;
using Tollminder.Core.Helpers;

namespace Tollminder.Core.ViewModels
{
    public class HomeViewModel 
		: ViewModelBase
    {		
		readonly IMvxMessenger _messenger;
		readonly ITrackFacade _track;
		readonly IGeoLocationWatcher _geoWatcher;

		private IList<MvxSubscriptionToken> _tokens;

		public HomeViewModel (IMvxMessenger messenger, ITrackFacade track, IGeoLocationWatcher geoWatcher)
		{
			_track = track;
			_messenger = messenger;
			_geoWatcher = geoWatcher;
			_tokens = new List<MvxSubscriptionToken> ();
		}

		public override void Start ()
		{
			base.Start ();

			_tokens.Add (_messenger.SubscribeOnThreadPoolThread<LocationMessage> (x => Location = x.Data, MvxReference.Strong));
			_tokens.Add(_messenger.SubscribeOnThreadPoolThread<StatusMessage>(x => StatusString = x.Data.ToString(), MvxReference.Strong));

			_tokens.Add (_messenger.SubscribeOnMainThread<LogUpdated> ((s) => LogText = Log._messageLog.ToString()));
			_tokens.Add(_messenger.SubscribeOnMainThread<GeoWatcherStatusMessage>((s) => IsBound = s.Data));
			IsBound = _geoWatcher.IsBound;
			if (_geoWatcher.Location != null)
				Location = _geoWatcher.Location;

			StatusString = _track.TollStatus.ToString();
			RaisePropertyChanged(LogText);
		}

		protected override void OnDestroy ()
		{
			base.OnDestroy ();
			foreach (var item in _tokens) {
				item.Dispose ();
			}
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
		public GeoLocation Location {
			get { return _location; } 
			set {
				_location = value;
				RaisePropertyChanged (() => Location);
				RaisePropertyChanged (() => LocationString);
			}
		}

		private string _logText;
		public string LogText {
			get { return _logText; }
			set {
				_logText = value;
				RaisePropertyChanged (() => LogText);
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

		private MvxCommand _startCommand;
		public ICommand StartCommand {
			get {
				return _startCommand ?? (_startCommand = new MvxCommand (async () =>
				{
					if (await _track.StartServices())
						IsBound = _geoWatcher.IsBound;
				}));
			}  
		}

		private MvxCommand _stopCommand;
		public ICommand StopCommand {
			get {
				return _stopCommand ?? (_stopCommand = new MvxCommand (() =>
				{ 
					if (_track.StopServices())
						IsBound = _geoWatcher.IsBound;
				}));
			}  
		}
	}
}