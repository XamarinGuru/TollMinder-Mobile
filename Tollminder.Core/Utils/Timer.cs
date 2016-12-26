using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tollminder.Core.Utils
{
	//internal delegate void TimerCallback(object state);

	//internal sealed class Timer : CancellationTokenSource, IDisposable
	//{
	//	public Timer(TimerCallback callback, object state, TimeSpan dueTime, TimeSpan period)
	//	{
	//		Task.Delay(dueTime, Token).ContinueWith(async (t, s) =>
	//		{
	//			var tuple = (Tuple<TimerCallback, object>)s;

	//			while (true)
	//			{
	//				if (IsCancellationRequested)
	//					break;
	//				#pragma warning disable 4014
	//				Task.Run(() => tuple.Item1(tuple.Item2));
	//				#pragma warning restore 4014
	//				await Task.Delay(period);
	//			}
	//		}, Tuple.Create(callback, state), CancellationToken.None,
	//			TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
	//			TaskScheduler.Default);
	//	}

	//	public new void Dispose()
	//	{
	//		Cancel();
	//	}
	//}
    public class Timer : CancellationTokenSource
    {
        public Timer(Action<object> callback, object state, TimeSpan millisecondsDueTime, TimeSpan millisecondsPeriod, bool waitForCallbackBeforeNextPeriod = false)
        {
            //Contract.Assert(period == -1, "This stub implementation only supports dueTime.");

            Task.Delay(millisecondsDueTime, Token).ContinueWith(async (t, s) =>
            {
                var tuple = (Tuple<Action<object>, object>)s;

                while (!IsCancellationRequested)
                {
                    if (waitForCallbackBeforeNextPeriod)
                        tuple.Item1(tuple.Item2);
                    else
                        Task.Run(() => tuple.Item1(tuple.Item2));

                    await Task.Delay(millisecondsPeriod, Token).ConfigureAwait(false);
                }

            }, Tuple.Create(callback, state), CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Default);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                Cancel();

            base.Dispose(disposing);
        }
    }
}
