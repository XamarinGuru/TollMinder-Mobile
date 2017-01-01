using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Tollminder.Touch.Services.SpeechServices
{
    //public class Timer : CancellationTokenSource
    //{
    //    public Timer(Action<object> callback, object state, int millisecondsDueTime, int millisecondsPeriod, bool waitForCallbackBeforeNextPeriod = false)
    //    {
    //        //Contract.Assert(period == -1, "This stub implementation only supports dueTime.");

    //        Task.Delay(millisecondsDueTime, Token).ContinueWith(async (t, s) =>
    //        {
    //            var tuple = (Tuple<Action<object>, object>)s;

    //            while (!IsCancellationRequested)
    //            {
    //                if (waitForCallbackBeforeNextPeriod)
    //                    tuple.Item1(tuple.Item2);
    //                else
    //                    Task.Run(() => tuple.Item1(tuple.Item2));

    //                await Task.Delay(millisecondsPeriod, Token).ConfigureAwait(false);
    //            }

    //        }, Tuple.Create(callback, state), CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Default);
    //    }

    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //            Cancel();

    //        base.Dispose(disposing);
    //    }
    //}
}
