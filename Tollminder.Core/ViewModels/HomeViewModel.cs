using Cirrious.MvvmCross.ViewModels;
using Tollminder.Core.Services;
using Tollminder.Core.Models;
using Tollminder.Core.Helpers;
using Cirrious.CrossCore;
using MessengerHub;

namespace Tollminder.Core.ViewModels
{
    public class HomeViewModel 
		: ViewModelBase
    {
		IGeoLocationWatcher _geoLocation;
		IMotionActivity _motionalActivity;
		IMessengerHub _messengerHub;

		public HomeViewModel (IGeoLocationWatcher geoLocation, IMotionActivity motionalActivity,IMessengerHub messengerHub)
		{
			this._messengerHub = messengerHub;
			this._geoLocation = geoLocation;			
			this._motionalActivity = motionalActivity;
		}

		public override void Start ()
		{
			base.Start ();
			Location = _geoLocation.Location;
			_geoLocation.StartGeolocationWatcher ();

			WeakSubscribe<MotionTypeChangedMessage> ((s)=> RaisePropertyChanged(()=> MotionTypeString));
		}

		private GeoLocation _Location;
		public GeoLocation Location {
			get { return _Location; } 
			set {
				_Location = value; 
				RaisePropertyChanged (() => Location);
				RaisePropertyChanged (() => LocationString);
			}
		}

		public string LocationString {
			get { return _Location.ToString(); } 
		}

		public string MotionTypeString {
			get { return _motionalActivity.MotionType.ToString(); }
		}

		public void SubscribeOnGeolocationUpdate()
		{
			_geoLocation.LocationUpdatedEvent += UpdateUILocation;
		}

		public void UnSubscribeOnGeolocationUpdate()
		{
			_geoLocation.LocationUpdatedEvent -= UpdateUILocation;
		}

		void UpdateUILocation (object sender, LocationUpdatedEventArgs e)
		{
			Location = e.Location;
		}

    }
}
