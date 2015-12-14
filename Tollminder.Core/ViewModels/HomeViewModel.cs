using Cirrious.MvvmCross.ViewModels;
using Tollminder.Core.Services;
using Tollminder.Core.Models;
using Tollminder.Core.Helpers;
using Cirrious.CrossCore;
using MessengerHub;
using System.Threading.Tasks;

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
			_geoLocation.StartGeolocationWatcher ();
			WeakSubscribe<MotionTypeChangedMessage> ((s)=> RaisePropertyChanged(()=> MotionTypeString));
			WeakSubscribe<LocationUpdatedMessage> ((s)=> {
				Location = s.Content;
			});
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
	}
}