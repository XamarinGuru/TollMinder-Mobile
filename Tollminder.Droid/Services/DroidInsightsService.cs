using System;
using System.Threading.Tasks;
using Android.App;
using Tollminder.Core.Services.Settings;
using Xamarin;

namespace Tollminder.Droid.Services
{
    public class DroidInsightsService : IInsightsService
    {
        string InsightsAPIKey = "60c11eb0abc64aac0ec93fe827b92385f7094cab";

        public DroidInsightsService()
        {
            Initialize();
        }

        void Initialize()
        {
            if (!Insights.IsInitialized)
            {

                Insights.Initialize(InsightsAPIKey, Application.Context);

                Insights.HasPendingCrashReport += (sender, isStartupCrash) =>
                {
                    if (isStartupCrash)
                        Task.Run(async () => await Insights.PurgePendingCrashReports()).ConfigureAwait(false);
                };
            }
        }


        #region IInsightsService implementation
        public void LogError(Exception e)
        {
            Insights.Report(e, Insights.Severity.Error);
        }
        #endregion
    }
}
