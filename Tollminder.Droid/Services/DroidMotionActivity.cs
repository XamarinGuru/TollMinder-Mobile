using MvvmCross.Platform;
using Tollminder.Core.Models;
using Tollminder.Droid.AndroidServices;
using MvvmCross.Plugins.Messenger;
using Android.Content;
using Tollminder.Core.Services.RoadsProcessing;
using Tollminder.Core.Services.Notifications;

namespace Tollminder.Droid.Services
{
    public class DroidMotionActivity : DroidServiceStarter, IMotionActivity
    {
        readonly INotifyService _notifyService;
        private MotionType _motionType;

        public DroidMotionActivity(INotifyService notifyService)
        {
            this._notifyService = notifyService;
            ServiceIntent = new Intent(ApplicationContext, typeof(MotionActivityService));
        }

        public virtual MotionType MotionType
        {
            get { return _motionType; }
            set
            {
                _motionType = value;
                Mvx.Resolve<IMvxMessenger>().Publish(new MotionMessage(this, value));
            }
        }

        public bool IsAutomove { get { return _motionType == MotionType.Automotive; } }

        public bool IsBound { get; private set; } = false;

        #region IMotionActivity implementation

        public void StartDetection()
        {
            if (!IsBound)
            {
                Start();
                IsBound = true;
            }
        }

        public void StopDetection()
        {
            if (IsBound)
            {
                Stop();
                IsBound = false;
            }
        }

        protected virtual void SpeakMotion(MotionType value)
        {
            if (value != MotionType)
            {
                if (IsAutomove)
                {
                    _notifyService.NotifyAsync("You start moving on the car");
                }
                else
                {
                    _notifyService.NotifyAsync(value.ToString());
                }
            }
        }

        public virtual bool IsStartMovingOnTheCar(MotionType value) => value == MotionType.Automotive;

        #endregion
    }
}

