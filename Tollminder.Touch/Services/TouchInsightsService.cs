using System;
using System.Threading.Tasks;
using Tollminder.Core.Services;
using Xamarin;

namespace Tollminder.Touch.Services
{
    public class TouchInsightsService : IInsightsService
    {
        string InsightsAPIKey = "2b455f0ac1fe12ddfc5b5ffae045c69e33a79c33";

        public TouchInsightsService()
        {
            Initialize();
        }

        public void LogError(Exception e)
        {
            Insights.Report(e, Insights.Severity.Error);
        }

        void Initialize()
        {
            if (!Insights.IsInitialized)
            {

                Insights.Initialize(InsightsAPIKey);

                Insights.HasPendingCrashReport += (sender, isStartupCrash) =>
                {
                    if (isStartupCrash)
                        Task.Run(async () => await Insights.PurgePendingCrashReports()).ConfigureAwait(false);
                };
            }
        }
    }
}
