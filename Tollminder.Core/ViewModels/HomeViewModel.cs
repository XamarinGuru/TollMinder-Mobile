using Cirrious.MvvmCross.ViewModels;
using Tollminder.Core.Services;
using Tollminder.Core.Models;
using Tollminder.Core.Helpers;
using Cirrious.CrossCore;

namespace Tollminder.Core.ViewModels
{
    public class HomeViewModel 
		: MvxViewModel
    {
		IGeoLocationWatcher _geoLocation;

		public HomeViewModel (IGeoLocationWatcher geoLocation)
		{
			this._geoLocation = geoLocation;			
		}

		public override void Start ()
		{
			base.Start ();
			Location = _geoLocation.Location;
			_geoLocation.StartGeolocationWatcher ();

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
