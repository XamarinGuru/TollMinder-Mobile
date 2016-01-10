using System;
using Tollminder.Core.Services;
using Cirrious.CrossCore;

namespace Tollminder.Core.ServicesHelpers
{
	public class TrackFacade
	{
		#region Services
		private readonly IGeoDataServiceAsync _geoData;
		private readonly IGeoLocationWatcher _geoWatcher;
		private readonly IMotionActivity _activity;
		private readonly ITextToSpeechService _textToSpeech;
		#endregion

		public TrackFacade ()
		{
			this._textToSpeech = Mvx.Resolve<ITextToSpeechService> ();
			this._activity = Mvx.Resolve<IMotionActivity> ();
			this._geoWatcher = Mvx.Resolve<IGeoLocationWatcher> ();
			this._geoData = Mvx.Resolve<IGeoDataServiceAsync> ();
		}

		public void StartServices () 
		{			
			_geoWatcher.StartGeolocationWatcher ();
			_activity.StartDetection ();
		}

		public void StopServices () 
		{			
			_geoWatcher.StopGeolocationWatcher ();
			_activity.StopDetection ();
		}
	}
}

