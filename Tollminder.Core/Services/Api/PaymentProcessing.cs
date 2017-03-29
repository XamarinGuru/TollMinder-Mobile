using System;
using System.Threading.Tasks;
using Chance.MvvmCross.Plugins.UserInteraction;
using MvvmCross.Platform;
using Tollminder.Core.Models.PaymentData;
using Tollminder.Core.Services.Settings;

namespace Tollminder.Core.Services.Api
{
    public class PaymentProcessing : IPaymentProcessing
    {
        readonly IStoredSettingsService storedSettingsService;
        readonly IServerApiService serverApiService;

        public PaymentProcessing(IStoredSettingsService storedSettingsService, IServerApiService serverApiService)
        {
            this.storedSettingsService = storedSettingsService;
            this.serverApiService = serverApiService;
        }

        public async Task AddCreditCardAsync(AddCreditCard crediCard)
        {
            var result = await serverApiService.AddCreditCardAsync(crediCard);
            if (result == null)
            {
                await Mvx.Resolve<IUserInteraction>().AlertAsync("Problem with server connection! Please try again later.", "Error", "Ok");
            }
        }

        public Task<string> PayForTripAsync(PayForTrip tripRequest)
        {
            throw new NotImplementedException();
        }

        public async Task TripCompletedAsync(TripCompleted tripRequest)
        {
            storedSettingsService.TripCompleted = tripRequest;

            var result = await serverApiService.TripCompletedAsync(tripRequest);
            if (result != null)
            {
                storedSettingsService.TripCompleted = null;
            }
            else
            {
                await Mvx.Resolve<IUserInteraction>().AlertAsync("Problem with server connection! Your trip result will be sended later.", "Warning", "Ok");
            }
        }
    }
}
