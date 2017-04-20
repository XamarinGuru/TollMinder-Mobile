using System;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services.RoadsProcessing
{
    public interface IMotionActivity
    {
        bool IsBound { get; }
        MotionType MotionType { get; set; }
        //bool IsAutomove { get; }
        void StartDetection();
        void StopDetection();
    }
}

