using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Linq;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Models;
using Tollminder.Core.Services;

namespace Tollminder.Core.ServicesHelpers.Implementation
{
	public class AutomoveActivity : IDisposable
	{
		private IObservable<long> _intervalUpdates;
		private readonly IList<MotionType> _activities;
		private readonly IMvxMessenger _messenger;
		private readonly IMotionActivity _activity;
		private const int Percentage = 50;
		public event EventHandler<bool> Automoved;

		private readonly IDisposable _intervalToken;
		private readonly MvxSubscriptionToken _messengerToken;


		public AutomoveActivity ()
		{
			_intervalUpdates = Observable.Interval (TimeSpan.FromSeconds (10));
			_activity = Mvx.Resolve<IMotionActivity> ();
			_messenger = Mvx.Resolve<IMvxMessenger> ();
			_activities = new List<MotionType> ();
			_intervalToken = _intervalUpdates.Subscribe (x => GetMostProbableResult ());
			_messengerToken = _messenger.Subscribe<MotionMessage> (x => _activities.Add (_activity.MotionType));
		}

		protected virtual void GetMostProbableResult ()
		{
			int automovePercentage = 0;
			try {
				automovePercentage = _activities.Count / _activities.Where (x => x == MotionType.Automotive).Count () * 100;
			} catch (DivideByZeroException) {}
			Automoved?.Invoke (this, automovePercentage > Percentage);
			_activities.Clear ();
		}

		public void Dispose ()
		{
			_intervalToken?.Dispose ();
			_messengerToken?.Dispose ();
		}
	}
}

