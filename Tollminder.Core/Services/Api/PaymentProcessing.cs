using System;
using System.Collections.Generic;
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

        public Task<NotPayedTrip> GetNotPayedTripsAsync()
        {
            return serverApiService.GetNotPayedTripsAsync();
        }

        public Task<string> PayForTripAsync(PayForTrip tripRequest)
        {
            return serverApiService.PayForTripAsync(tripRequest);
        }

        public async Task AddCreditCardAsync(AddCreditCard crediCard)
        {
            var result = await serverApiService.AddCreditCardAsync(crediCard);
            if (result == null)
            {
                await Mvx.Resolve<IUserInteraction>().AlertAsync("Problem with server connection! Please try again later.", "Error", "Ok");
            }
        }

        public async Task<List<CreditCardAuthorizeDotNet>> GetCreditCardsAsync()
        {
            var result = await serverApiService.GetCreditCardsAsync();
            if (result != null)
                return result;
            else
            {
                await Mvx.Resolve<IUserInteraction>().AlertAsync("Sorry, there is no credit cards for now.", "Warning");
                return null;
            }
        }

        public Task RemoveCreditCardAsync(string paymentProfileId)
        {
            return serverApiService.RemoveCreditCardAsync(paymentProfileId);
        }

        public Task<List<PayHistory>> GetPayHistoryAsync(DateTime dateFrom, DateTime dateTo)
        {
            return serverApiService.GetPayHistoryAsync(dateFrom, dateTo);
        }

        public Task<string> DownloadPayHistoryAsync(DateTime dateFrom, DateTime dateTo)
        {
            return serverApiService.DownloadPayHistoryAsync(dateFrom, dateTo);
        }
    }
}
