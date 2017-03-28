using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Linq;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Models;
using Tollminder.Core.Services.RoadsProcessing;

namespace Tollminder.Core.ServicesHelpers
{
    public class AutomoveActivity : IDisposable
    {
        private IObservable<long> _intervalUpdates;
        private readonly IList<MotionType> _activities;
        private readonly IMvxMessenger _messenger;
        private readonly IMotionActivity _activity;
        private const int Percentage = 50;
        public event EventHandler<bool> AutomovedChandged;

        private readonly IDisposable _intervalToken;
        private readonly MvxSubscriptionToken _messengerToken;


        public AutomoveActivity(IList<MotionType> activities, IMvxMessenger messenger, IMotionActivity activity)
        {
            _activities = activities;
            _messenger = messenger;
            _activity = activity;

            _intervalUpdates = Observable.Interval(TimeSpan.FromSeconds(60));
            _intervalToken = _intervalUpdates.Subscribe(x => GetMostProbableResult());
            _messengerToken = _messenger.SubscribeOnThreadPoolThread<MotionMessage>(x =>
            {
                _activities.Add(_activity.MotionType);
            });
        }

        protected virtual void GetMostProbableResult()
        {
            int automovePercentage = 0;
            try
            {
                automovePercentage = _activities.Count / _activities.Count(x => x == MotionType.Automotive) * 100;
            }
            catch (DivideByZeroException)
            {
            }
            if (_activity.MotionType == MotionType.Automotive)
            {
                AutomovedChandged?.Invoke(this, true);
            }
            else
            {
                AutomovedChandged?.Invoke(this, automovePercentage > Percentage);
            }
            _activities.Clear();
        }

        public void Dispose()
        {
            _intervalToken?.Dispose();
            _messengerToken?.Dispose();
        }
    }
}

