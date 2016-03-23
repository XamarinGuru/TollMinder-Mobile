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
		private readonly IMvxMessenger _messenger;
		private readonly ITrackFacade _track;

		private IList<MvxSubscriptionToken> _tokens;

		public HomeViewModel (IMvxMessenger messenger, ITrackFacade track)
		{
			this._track = track;
			this._messenger = messenger;
			this._tokens = new List<MvxSubscriptionToken> ();
		}

		public override void Start ()
		{
			base.Start ();
			//_tokens.Add (_messenger.SubscribeOnMainThread<LocationMessage> (x => Location = x.Data));
			//_tokens.Add (_messenger.SubscribeOnMainThread<LogUpdated> ((s) => LogText = Log._messageLog.ToString()));
		}

		protected override void OnDestroy ()
		{
			base.OnDestroy ();
			foreach (var item in _tokens) {
				item.Dispose ();
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
				return _startCommand ?? (_startCommand = new MvxCommand (_track.StartServices));
			}  
		}

		private MvxCommand _stopCommand;
		public ICommand StopCommand {
			get {
				return _stopCommand ?? (_stopCommand = new MvxCommand (_track.StopServices));
			}  
		}
	}
}