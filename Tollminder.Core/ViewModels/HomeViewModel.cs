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

		public HomeViewModel (IGeoLocationWatcher geoLocation, IMotionActivity motionalActivity)
		{
			this._geoLocation = geoLocation;			
			this._motionalActivity = motionalActivity;
		}

		public override void Start ()
		{
			base.Start ();
			WeakSubscribe<MotionTypeChangedMessage> ((s)=> RaisePropertyChanged(()=> MotionTypeString));
			WeakSubscribe<LocationUpdatedMessage> ((s)=> {
				Location = s.Content;
			});
			WeakSubscribe<MotionTypeChangedMessage> ((s)=> {
				MotionType = s.Content;
			});
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

		public void StartActivityDetection()
		{
			Mvx.Resolve<IMotionActivity> ().StartDetection ();
		}
	}
}