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
			_tokens.Add (_messenger.SubscribeOnMainThread<LocationMessage> (x => Location = x.Data));
			_tokens.Add (_messenger.SubscribeOnMainThread<MotionMessage> (x => MotionType = x.Data));
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

		private float _Percent;
		public float Percent {
		    get { return _Percent; } 
		    set {
		     _Percent = value; 
		     RaisePropertyChanged(() => Percent);
		    }
		}

		private MvxCommand _stoppCommand;
		public ICommand StoppCommand {
			get {
				return _stoppCommand ?? (_stoppCommand = new MvxCommand (TaskDo));
			}  
		}

		private async void TaskINeedToDoto()
		{
//			if (await Mvx.Resolve<IUserInteraction>().ConfirmAsync("Are you sure?")) {
//				var asd = "string";
//			}
		}

		private async void TaskDo()
		{
			Progress<DownloadBytesProgress> dataProg = new Progress<DownloadBytesProgress> ();
			dataProg.ProgressChanged += (object sender, DownloadBytesProgress e) => {
				Percent = e.PercentComplete;
			};
			var data = await Mvx.Resolve<IHttpService> ().FetchAsync (@"http://eoimages.gsfc.nasa.gov/images/imagerecords/74000/74393/world.topo.200407.3x5400x2700.jpg", dataProg);
		}
	}
}