using Cirrious.MvvmCross.ViewModels;
using Tollminder.Core.Services;
using Tollminder.Core.Models;
using Tollminder.Core.Helpers;
using Cirrious.CrossCore;
using MessengerHub;
using System.Threading.Tasks;
using System;
using System.Windows.Input;

namespace Tollminder.Core.ViewModels
{
    public class HomeViewModel 
		: ViewModelBase
    {
		private readonly IGeoLocationWatcher _geoLocation;
		private readonly IMotionActivity _motionalActivity;
		private readonly IGeoDataServiceAsync _geoData;

		public HomeViewModel (IGeoLocationWatcher geoLocation, IMotionActivity motionalActivity,IGeoDataServiceAsync geoData)
		{
			this._geoData = geoData;
			this._geoLocation = geoLocation;			
			this._motionalActivity = motionalActivity;

		}

		public override void Start ()
		{
			base.Start ();
			WeakSubscribe<MotionTypeChangedMessage> ((s)=>  RaisePropertyChanged(()=> MotionTypeString));
			WeakSubscribe<LocationUpdatedMessage> ((s)=> {
				Location = s.Content;
			});
			WeakSubscribe<MotionTypeChangedMessage> ((s)=> {
				MotionType = s.Content;
			});
			StartActivityDetection ();
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