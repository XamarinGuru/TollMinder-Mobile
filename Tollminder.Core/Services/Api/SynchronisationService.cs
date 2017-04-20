using System.Threading.Tasks;
using Tollminder.Core.Services.Settings;

namespace Tollminder.Core.Services.Api
{
    public class SynchronisationService : ISynchronisationService
    {
        readonly IServerApiService serverApiService;
        readonly IStoredSettingsService storedSettingsService;
        readonly IPaymentProcessing paymentProcessing;

        public SynchronisationService(IServerApiService serverApiService, IStoredSettingsService storedSettingsService, IPaymentProcessing paymentProcessing)
        {
            this.serverApiService = serverApiService;
            this.storedSettingsService = storedSettingsService;
            this.paymentProcessing = paymentProcessing;
        }

        public async Task<bool> AuthorizeTokenSynchronisationAsync()
        {
            if (storedSettingsService.ProfileId == null || storedSettingsService.AuthToken == null)
                return false;
            else
            {
                string result = await serverApiService.GetValidAuthorizeTokenAsync();
                if (result != null)
                    storedSettingsService.AuthToken = result;
                return storedSettingsService.IsAuthorized;
            }
        }

        public async Task DataSynchronisationAsync()
        {
            if (storedSettingsService.IsDataSynchronized || storedSettingsService.Profile == null)
            {
                storedSettingsService.Profile = await serverApiService.GetProfileAsync(storedSettingsService.ProfileId, storedSettingsService.AuthToken);
            }
            if (storedSettingsService.TripCompleted != null)
                await paymentProcessing.TripCompletedAsync(storedSettingsService.TripCompleted);
            else
            {
                var result = await serverApiService.SaveProfileAsync(storedSettingsService.ProfileId, storedSettingsService.Profile, storedSettingsService.AuthToken);
                if (result != null)
                {
                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                        storedSettingsService.IsDataSynchronized = true;
                }
            }
        }
    }
}