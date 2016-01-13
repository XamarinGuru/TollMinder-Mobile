using Cirrious.MvvmCross.ViewModels;
using Tollminder.Core.Services;
using Tollminder.Core.Models;
using Tollminder.Core.Helpers;
using Cirrious.CrossCore;
using System.Threading.Tasks;
using System;
using System.Windows.Input;
using MvvmCross.Plugins.Messenger;
using System.Collections.Generic;

namespace Tollminder.Core.ViewModels
{
    public class HomeViewModel 
		: ViewModelBase
    {
		private readonly IGeoLocationWatcher _geoLocation;
		private readonly IMotionActivity _motionalActivity;
		private readonly IGeoDataServiceAsync _geoData;
		private readonly IMvxMessenger _messenger;

		private IList<MvxSubscriptionToken> _tokens;

		public HomeViewModel (IGeoLocationWatcher geoLocation, IMotionActivity motionalActivity,IGeoDataServiceAsync geoData, IMvxMessenger messenger)
		{
			this._messenger = messenger;
			this._geoData = geoData;
			this._geoLocation = geoLocation;			
			this._motionalActivity = motionalActivity;
			this._tokens = new List<MvxSubscriptionToken> ();
		}

		public override void Start ()
		{
			base.Start ();
			_tokens.Add (_messenger.SubscribeOnMainThread<LocationMessage> (x => Location = x.Data));
			_tokens.Add (_messenger.SubscribeOnMainThread<MotionMessage> (x => MotionType = x.Data));

//			StartActivityDetection ();
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
				return _startCommand ?? (_startCommand = new MvxCommand (_geoLocation.StartGeolocationWatcher));
			}  
		}

		private MvxCommand _stopCommand;
		public ICommand StopCommand {
			get {
				return _stopCommand ?? (_stopCommand = new MvxCommand (_geoLocation.StopGeolocationWatcher));
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

		private async void TaskDo()
		{
			Progress<DownloadBytesProgress> dataProg = new Progress<DownloadBytesProgress> ();
			dataProg.ProgressChanged += (object sender, DownloadBytesProgress e) => {
				Percent = e.PercentComplete;
			};
			var data = await Mvx.Resolve<IHttpService> ().FetchAsync (@"http://eoimages.gsfc.nasa.gov/images/imagerecords/74000/74393/world.topo.200407.3x5400x2700.jpg", dataProg);
			var asdasd = "dad";
		}

		private MvxCommand _addNewLocation;
		public ICommand AddNewLocation {
			get {
				return _addNewLocation ?? (_addNewLocation = new MvxCommand (async () => {
//					await _geoData.
					await _geoData.InsertAsync(new GeoLocation(50.4021698,30.3922658));
					_locations = await _geoData.CountAsync;
					RaisePropertyChanged(() => CountOfLocations);
				}));
			}  
		}

		int _locations;
		public int CountOfLocations {
			get { return _locations; }
		}

		public void StartActivityDetection()
		{
			_motionalActivity.StartDetection ();
		}
	}
}